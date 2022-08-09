using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsBulletsManager : MonoBehaviour
{
    
    [SerializeField] private BulletData[] bulletDatas = new BulletData[1];
    [SerializeField] private List<int> unlockedBulletsID = new List<int>();

    public BulletData[] BulletDatas { get { return bulletDatas; } }
    public List<int> UnlockedBulletsID { get { return unlockedBulletsID; } }

    private Dictionary<int, int> bulletsCount = new Dictionary<int, int>(); //1-id,2-bullets count
    private Dictionary<int, int> bulletsMax = new Dictionary<int, int>(); //1-id,2-max bullets

    public Dictionary<int, int> BulletsMax { get { return bulletsMax; } }


    private void Awake()
    {
        foreach (var item in bulletDatas)
        {
            if (IsIdUnlocked(item.Id))
            {
                bulletsCount.Add(item.Id, item.MaxBullets);
                bulletsMax.Add(item.Id, item.MaxBullets);
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

        throw new System.Exception("пиздец");
    }

    public bool CheckBullets(int bulletID)
    {
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
        if (bulletsCount.ContainsKey(bulletID))
        {

            if (bulletsCount[bulletID] + count >= bulletsMax[bulletID])
                bulletsCount[bulletID] = bulletsMax[bulletID];

            else if (bulletsCount[bulletID] + count <= 0)
                bulletsCount[bulletID] = 0;

            else
                bulletsCount[bulletID] += count;
        }
    }

    public void SubtractBullets(int bulletID, int count)
    {
        if (bulletsCount.ContainsKey(bulletID))
        {

            if (bulletsCount[bulletID] - count >= bulletsMax[bulletID])
                bulletsCount[bulletID] = bulletsMax[bulletID];

            else if (bulletsCount[bulletID] - count <= 0)
                bulletsCount[bulletID] = 0;

            else
                bulletsCount[bulletID] -= count;
        }
    }

    public void SubtractOneBullet(int bulletID)
    {
        if (bulletsCount.ContainsKey(bulletID))
        {
            if (bulletsCount[bulletID] - 1 <= 0)
                bulletsCount[bulletID] = 0;

            else
                bulletsCount[bulletID] -= 1;
        }
    }

    public void AddNewBullet(int bulletID)
    {
        //Check errors
        foreach (var data in bulletDatas)
        {
            if(data.Id == bulletID)
            {
                foreach (var item in unlockedBulletsID)
                {
                    if (item == bulletID)
                        print($"BulletID {bulletID} now then added!");
                }

                //if errors not find: new bullet add
                unlockedBulletsID.Add(bulletID);
                bulletsCount.Add(bulletID, 0);
                bulletsMax.Add(bulletID, data.MaxBullets);

            } 
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

        throw new System.Exception($"BullerID {id} not find!");
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



}
