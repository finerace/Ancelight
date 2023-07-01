using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerWeaponsBulletsManager : MonoBehaviour
{
    
    [SerializeField] private BulletData[] bulletDatas = new BulletData[1];
    [SerializeField] private List<int> unlockedBulletsID = new List<int>();
    [SerializeField] private int[] unlockedBulletsStartCount;

    public BulletData[] BulletDatas { get { return bulletDatas; } }
    public List<int> UnlockedBulletsID { get { return unlockedBulletsID; } }

    [SerializeField] private Dictionary<int, int> bulletsCount = new Dictionary<int, int>(); //1-id,2-bullets count
    [SerializeField] private Dictionary<int, int> bulletsMax = new Dictionary<int, int>(); //1-id,2-max bullets

    public Dictionary<int, int> BulletsCount => bulletsCount;
    public Dictionary<int, int> BulletsMax => bulletsMax;

    [Space]
    
    [SerializeField] private Dictionary<string, float> plasmaReserves = new Dictionary<string, float>();

    [SerializeField] private Dictionary<string, float> plasmaMaxReserves = new Dictionary<string, float>();

    public Dictionary<string, float> PlasmaReserves => plasmaReserves;
    public Dictionary<string, float> PlasmaMaxReserves => plasmaMaxReserves;

    [Space] 
    [SerializeField] private float yellowPlasmaMaxReserve;

    [SerializeField] private float redPlasmaMaxReserve;

    [SerializeField] private float bluePlasmaMaxReserve;

    private bool isLoaded;

    [SerializeField] private bool infinityAmmoMode;
    
    public void Load(LevelSaveData.SavePlayerData playerData)
    {
        FixedJsonUtilityFunc.JsonToNormal(playerData.bulletsCount, bulletsCount);
        FixedJsonUtilityFunc.JsonToNormal(playerData.bulletsMax, bulletsMax);
        
        FixedJsonUtilityFunc.JsonToNormal(playerData.plasmaReserves, plasmaReserves);
        FixedJsonUtilityFunc.JsonToNormal(playerData.plasmaMaxReserves, plasmaMaxReserves);

        isLoaded = true;
    }

    private void Start()
    {
        if(isLoaded)
            return;
            
        InitializeUnlockedBullets();
        
        InitializePlasmaReserves();
        
        InitStartBullets();
        
        void InitializeUnlockedBullets()
        {
            foreach (var item in bulletDatas)
            {
                if (!IsIdUnlocked(item.Id)) 
                    continue;
                
                bulletsCount.Add(item.Id, 0);
                bulletsMax.Add(item.Id, item.MaxBullets);
            }
        }

        void InitializePlasmaReserves()
        {
            InitializePlasma("yellow",yellowPlasmaMaxReserve);
            
            InitializePlasma("red",redPlasmaMaxReserve);
            
            InitializePlasma("blue",bluePlasmaMaxReserve);
            
            void InitializePlasma(string plasmaId,float plasmaMaxReserve)
            {
                plasmaMaxReserves.Add(plasmaId,plasmaMaxReserve);
                plasmaReserves.Add(plasmaId,0);
            }
        }

        void InitStartBullets()
        {
            for (int i = 0; i < unlockedBulletsStartCount.Length; i++)
            {
                var bulletId = i;
                
                AddBullets(bulletId+1,unlockedBulletsStartCount[bulletId]);
            }
        }
    }

    public BulletData FindData(int id)
    {
        foreach (var item in bulletDatas)
        {
            if(item.Id == id && IsIdUnlocked(item.Id))
                return item;
        }

        throw new InvalidDataException($"Bullet id {id} not find!");
    }

    public bool CheckBullets(int bulletID)
    {
        if (infinityAmmoMode)
            return true;
        
        if (bulletID != 0)
        {
            if (bulletsCount.ContainsKey(bulletID))
            {
                return bulletsCount[bulletID] > 0;
            }
        }
        else return true;

        throw new System.Exception($"Bullet ID {bulletID} not find!");
    }

    public void AddBullets(int bulletID,int count)
    {
        if (!bulletsCount.ContainsKey(bulletID)) 
            return;
        
        if (bulletsCount[bulletID] + count >= bulletsMax[bulletID])
            bulletsCount[bulletID] = bulletsMax[bulletID];

        else if (bulletsCount[bulletID] + count <= 0)
            bulletsCount[bulletID] = 0;

        else
            bulletsCount[bulletID] += count;
    }

    public void SubtractBullets(int bulletID, int count)
    {
        if(infinityAmmoMode)
            return;
        
        if (!bulletsCount.ContainsKey(bulletID)) 
            return;
        
        if (bulletsCount[bulletID] - count >= bulletsMax[bulletID])
            bulletsCount[bulletID] = bulletsMax[bulletID];

        else if (bulletsCount[bulletID] - count <= 0)
            bulletsCount[bulletID] = 0;

        else
            bulletsCount[bulletID] -= count;
    }

    public void SubtractOneBullet(int bulletID)
    {
        if(infinityAmmoMode)
            return;
        
        if (!bulletsCount.ContainsKey(bulletID)) 
            return;
        
        if (bulletsCount[bulletID] - 1 <= 0)
            bulletsCount[bulletID] = 0;

        else
            bulletsCount[bulletID] -= 1;
    }

    public void UnlockBullet(int bulletID)
    {
        //Check errors
        foreach (var data in bulletDatas)
        {
            if (data.Id != bulletID) 
                continue;

            if(IsIdUnlocked(bulletID))
                return;
            
            //if errors not find: new bullet add
            unlockedBulletsID.Add(bulletID);
            bulletsCount.Add(bulletID, 0);
            bulletsMax.Add(bulletID, data.MaxBullets);
            
            return;
        }

        throw new System.Exception($"BulletID {bulletID} does not exist!");
    }

    public int GetBulletsCount(int id)
    {
        if (id != 0)
        {
            if (bulletsCount.ContainsKey(id))
                return bulletsCount[id];
        }
        else return 0;

        throw new System.Exception($"BulletID {id} not find!");
    }

    public bool IsIdUnlocked(int id)
    {
        if (id != 0)
            foreach (var item in unlockedBulletsID)
            {
                if (item == id)
                    return true;
            }
        else return true;

        return false;
    }

    public void AddPlasma(string plasmaId, float count)
    {
        if(count <= 0)
            return;
        
        if(!IsPlasmaIdExists(plasmaId))
            return;

        if (plasmaReserves[plasmaId] + count > plasmaMaxReserves[plasmaId])
        {
            plasmaReserves[plasmaId] = plasmaMaxReserves[plasmaId];
        }
        else
            plasmaReserves[plasmaId] += count;
    }
    
    public void SubtractPlasma(string plasmaId, float count)
    {
        if(count <= 0)
            return;
        
        if(!IsPlasmaIdExists(plasmaId))
            return;

        if (plasmaReserves[plasmaId] - count < 0)
        {
            throw new ArgumentException
                ($"{plasmaId} plasma subtract count is to many!.");
        }
        else
            plasmaReserves[plasmaId] -= count;
    }
    
    public bool IsPlasmaIdExists(string plasmaId)
    {
        return plasmaReserves.Keys.Any(localPlasmaId => plasmaId == localPlasmaId);
    }
    
}
