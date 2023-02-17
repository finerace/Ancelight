using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSaveData : MonoBehaviour
{
     public static LevelSaveData mainLevelSaveData;

     private LevelPassageService levelPassageService;
     [SerializeField] private string savedLevelPassageService;
     
     [SerializeField] private List<SaveEnemyData> toSaveEnemyData;

     [SerializeField] private List<SaveWeaponItemData> toSaveWeaponItemData;
     [SerializeField] private List<SaveHealthArmorItemData> toSaveHealthArmorItemData;
     [SerializeField] private List<SavePlasmaItemData> toSavePlasmaItemData;
     [SerializeField] private List<SaveBulletData> toSaveBulletData;

     [SerializeField] private List<SaveTriggerData> toSaveLevelTriggers;
     
     [SerializeField] private List<string> savedLevelSpawnScenarios;
     private List<LevelSpawnScenario> levelSpawnScenarios = new List<LevelSpawnScenario>();

     private List<LevelTransformAnimationSystem> levelTransformTranslateSystems = new List<LevelTransformAnimationSystem>();
     [SerializeField] private List<string> levelTransformTranslateSavedSystems;
     
     [SerializeField] private SavePlayerData playerData;
     public PlayerMainService playerMainService;
     
     [SerializeField] private string levelName;
     
     public static LevelSaveData MainLevelSaveData => mainLevelSaveData;

     public LevelPassageService LevelPassageService => levelPassageService;
     
     public string SavedLevelPassageService => savedLevelPassageService;
     
     public List<SaveEnemyData> ToSaveEnemyData => toSaveEnemyData;

     public List<SaveWeaponItemData> ToSaveWeaponItemData => toSaveWeaponItemData;

     public List<SaveHealthArmorItemData> ToSaveHealthArmorItemData => toSaveHealthArmorItemData;

     public List<SavePlasmaItemData> ToSavePlasmaItemData => toSavePlasmaItemData;

     public List<SaveBulletData> ToSaveBulletData => toSaveBulletData;

     public List<SaveTriggerData> ToSaveLevelTriggers => toSaveLevelTriggers;
     
     public List<LevelSpawnScenario> LevelSpawnScenarios => levelSpawnScenarios;

     public List<string> SavedLevelSpawnScenarios => savedLevelSpawnScenarios;

     public List<LevelTransformAnimationSystem> LevelTransformTranslateSystems => levelTransformTranslateSystems;

     public List<string> LevelTransformTranslateSavedSystems => levelTransformTranslateSavedSystems;

     public SavePlayerData PlayerData => playerData;

     public string LevelName => levelName;

     public void Awake()
    {
        mainLevelSaveData = this;
        levelName = SceneManager.GetActiveScene().name;
    }
     
     public string SaveToJson()
     {
         SetJsonnedData();

         return JsonUtility.ToJson(this);

         void SetJsonnedData()
         {
             foreach (var enemyData in toSaveEnemyData)
             {
                 enemyData.jsonEnemyPos = FixedJsonUtilityFunc.GetJsonVersion(enemyData.EnemyPos);
                 enemyData.jsonEnemyRb = FixedJsonUtilityFunc.GetJsonVersion(enemyData.EnemyRb);
                 
                 enemyData.jsonEnemyType = JsonUtility.ToJson(enemyData.EnemyType);
                 enemyData.jsonEnemyAttack = JsonUtility.ToJson(enemyData.EnemyAttack);
             }

             foreach (var weaponItemData in toSaveWeaponItemData)
             {
                 weaponItemData.jsonWeaponItemPos = FixedJsonUtilityFunc.GetJsonVersion(weaponItemData.WeaponItemPos);
                 weaponItemData.jsonWeaponGetItem = JsonUtility.ToJson(weaponItemData.WeaponGetItem);
             }

             foreach (var healthArmorItemData in toSaveHealthArmorItemData)
             {
                 healthArmorItemData.jsonHealthArmorItemPos =
                     FixedJsonUtilityFunc.GetJsonVersion(healthArmorItemData.HealthArmorItemPos);

                 healthArmorItemData.jsonHealthArmorItem = JsonUtility.ToJson(healthArmorItemData.HealthArmorItem);
             }

             foreach (var plasmaItemData in toSavePlasmaItemData)
             {
                 plasmaItemData.jsonPlasmaItemPos = FixedJsonUtilityFunc.GetJsonVersion(plasmaItemData.PlasmaItemPos);

                 plasmaItemData.jsonPlasmaItem = JsonUtility.ToJson(plasmaItemData.PlasmaItem);
             }

             foreach (var saveBulletData in toSaveBulletData)
             {
                 saveBulletData.jsonBulletT = FixedJsonUtilityFunc.GetJsonVersion(saveBulletData.BulletT);
                 
                 if(saveBulletData.BulletRb != null)
                    saveBulletData.jsonBulletRb = FixedJsonUtilityFunc.GetJsonVersion(saveBulletData.BulletRb);

                 saveBulletData.jsonBullet = JsonUtility.ToJson(saveBulletData.Bullet);
             }

             foreach (var triggerData in toSaveLevelTriggers)
             {
                 triggerData.jsonTriggerT = FixedJsonUtilityFunc.GetJsonVersion(triggerData.TriggerT);
                 triggerData.jsonLevelTrigger = JsonUtility.ToJson(triggerData.LevelTrigger);
             }

             foreach (var levelSpawnScenario in levelSpawnScenarios)
             {
                 savedLevelSpawnScenarios.Clear();
                 
                 var jsonSpawnScenario = JsonUtility.ToJson(levelSpawnScenario);
                 savedLevelSpawnScenarios.Add(jsonSpawnScenario);
             }

             foreach (var translateSystem in levelTransformTranslateSystems)
             {
                 levelTransformTranslateSavedSystems.Add(JsonUtility.ToJson(translateSystem));
             }
             
             SavePlayer();

             savedLevelPassageService = JsonUtility.ToJson(levelPassageService);
             
             void SavePlayer()
             {
                 playerData.jsonPlayerT = FixedJsonUtilityFunc.GetJsonVersion(playerData.PlayerMovement.Body);
                 playerData.jsonPlayerRb = FixedJsonUtilityFunc.GetJsonVersion(playerData.PlayerRb);

                 playerData.jsonPlayerMovement = JsonUtility.ToJson(playerData.PlayerMovement);

                 playerData.jsonPlayerDashsService = JsonUtility.ToJson(playerData.PlayerDashsService);

                 playerData.jsonPlayerHookService = JsonUtility.ToJson(playerData.PlayerHookService);

                 playerData.jsonPlayerMainService = JsonUtility.ToJson(playerData.PlayerMainService);

                 playerData.jsonPlayerImmediatelyProtectionService =
                     JsonUtility.ToJson(playerData.PlayerImmediatelyProtectionService);

                 playerData.jsonPlayerWeaponsManager = JsonUtility.ToJson(playerData.PlayerWeaponsManager);

                 playerData.jsonPlayerWeaponsBulletsManager = JsonUtility.ToJson(playerData.PlayerWeaponsBulletsManager);
                 playerData.bulletsCount =
                     FixedJsonUtilityFunc.GetJsonVersion(playerData.PlayerWeaponsBulletsManager.BulletsCount);
                 playerData.bulletsMax =
                     FixedJsonUtilityFunc.GetJsonVersion(playerData.PlayerWeaponsBulletsManager.BulletsMax);

                 playerData.plasmaReserves =
                     FixedJsonUtilityFunc.GetJsonVersion(playerData.PlayerWeaponsBulletsManager.PlasmaReserves);
                 playerData.plasmaMaxReserves =
                     FixedJsonUtilityFunc.GetJsonVersion(playerData.PlayerWeaponsBulletsManager.PlasmaMaxReserves);
             }
         }
     }
     
    public void AddToSaveData(DefaultBot bot)
    {
        var newEnemyData = new SaveEnemyData(
            bot.body,
            bot.bodyRB,
            bot,
            bot.botAttack);
        
        toSaveEnemyData.Add(newEnemyData);
    }
    public void RemoveFromSaveData(DefaultBot bot)
    {
        foreach (var enemyData in toSaveEnemyData)
        {
            if (enemyData.EnemyType == bot)
            {
                toSaveEnemyData.Remove(enemyData);
                
                break;
            }
        }
    }
    
    public void AddToSaveData(WeaponGetItem weaponItem)
    {
        var newWeaponItemData = new SaveWeaponItemData(
            weaponItem.transform,
            weaponItem);
        
        toSaveWeaponItemData.Add(newWeaponItemData);
    }
    public void RemoveFromSaveData(WeaponGetItem saveWeaponItemData)
    {
        foreach (var weaponItem in toSaveWeaponItemData)
        {
            if (weaponItem.WeaponGetItem == saveWeaponItemData)
            {
                toSaveWeaponItemData.Remove(weaponItem);
                
                break;
            }
        }
    }
    
    public void AddToSaveData(GetHealthArmorItem healthArmorItem)
    {
        var newHealthArmorItemData = new SaveHealthArmorItemData(
            healthArmorItem.transform,
            healthArmorItem);
        
        toSaveHealthArmorItemData.Add(newHealthArmorItemData);
    }
    public void RemoveFromSaveData(GetHealthArmorItem saveHealthArmorItem)
    {
        foreach (var healthArmorItem in toSaveHealthArmorItemData)
        {
            if (healthArmorItem.HealthArmorItem == saveHealthArmorItem)
            {
                toSaveHealthArmorItemData.Remove(healthArmorItem);
                
                break;
            }
        }
    }
    
    public void AddToSaveData(PlasmaGetItem plasmaItem)
    {
        var newPlasmaItemData = new SavePlasmaItemData(
            plasmaItem.transform,
            plasmaItem);
        
        toSavePlasmaItemData.Add(newPlasmaItemData);
    }
    public void RemoveFromSaveData(PlasmaGetItem plasmaSaveItem)
    {
        foreach (var plasmaItem in toSavePlasmaItemData)
        {
            if (plasmaSaveItem == plasmaItem.PlasmaItem)
            {
                toSavePlasmaItemData.Remove(plasmaItem);
                
                break;
            }
        }
    }
    
    public void AddToSaveData(Bullet bullet)
    {
        var newBulletData = new SaveBulletData(
            bullet.body_,
            bullet.rigidbody_,bullet);
        
        toSaveBulletData.Add(newBulletData);
    }
    public void RemoveFromSaveData(Bullet bullet)
    {
        foreach (var bulletData in toSaveBulletData)
        {
            if (bullet == bulletData.Bullet)
            {
                toSaveBulletData.Remove(bulletData);
                
                break;
            }
        }
    }

    public void AddToSaveData(PlayerMainService player)
    {
        playerData = new SavePlayerData(
            player.playerMovement.Body,
            player.playerMovement.PlayerRb,
            player, player.playerMovement,
            player.weaponsManager,
            player.weaponsBulletsManager,
            player.hookService,
            player.dashsService,
            player.immediatelyProtectionService);
    }
    
    public void AddToSaveData(LevelTriggerBase trigger)
    {
        var newTriggerData = new SaveTriggerData(
            trigger.transform,
            trigger);
        
        toSaveLevelTriggers.Add(newTriggerData);
    }
    
    public void RemoveFromSaveData(LevelTriggerBase trigger)
    {
        foreach (var triggerData in toSaveLevelTriggers)
        {
            if (trigger == triggerData.LevelTrigger)
            {
                toSaveLevelTriggers.Remove(triggerData);
                
                break;
            }
        }
    }
    
    public void AddToSaveData(LevelSpawnScenario spawnScenario)
    {
        levelSpawnScenarios.Add(spawnScenario);
    }
    
    public void RemoveFromSaveData(LevelSpawnScenario spawnScenario)
    {
        foreach (var spawnScenarioItem in levelSpawnScenarios)
        {
            if (spawnScenario == spawnScenarioItem)
            {
                levelSpawnScenarios.Remove(spawnScenarioItem);
                
                break;
            }
        }
    }

    public void AddToSaveData(LevelTransformAnimationSystem translateSystem)
    {
        levelTransformTranslateSystems.Add(translateSystem);
    }
    
    public void AddToSaveData(LevelPassageService levelPassageService)
    {
        this.levelPassageService = levelPassageService;
    }
    
    [Serializable]
    public class SaveEnemyData
    {
        [NonSerialized] private Transform enemyPos;
        [NonSerialized] private Rigidbody enemyRb;
        
        [NonSerialized] private DefaultBot enemyType;
        [NonSerialized] private DefaultBotAttack enemyAttack;

        public JsonTransform jsonEnemyPos;
        public JsonRigidbody jsonEnemyRb;

        public string enemyTypeName;
        public string jsonEnemyType;
        public string jsonEnemyAttack;
        
        public SaveEnemyData(Transform enemyPos, Rigidbody enemyRb, DefaultBot enemyType, DefaultBotAttack enemyAttack)
        {
            this.enemyPos = enemyPos;
            this.enemyRb = enemyRb;
            this.enemyType = enemyType;
            this.enemyAttack = enemyAttack;
            enemyTypeName = enemyType.GetType().Name;
        }

        public Transform EnemyPos => enemyPos;

        public Rigidbody EnemyRb => enemyRb;

        public DefaultBot EnemyType => enemyType;

        public DefaultBotAttack EnemyAttack => enemyAttack;
    }
    
    [Serializable]
    public class SaveWeaponItemData
    {
        [NonSerialized] private Transform weaponItemPos;
        [NonSerialized] private WeaponGetItem weaponGetItem;
        
        public JsonTransform jsonWeaponItemPos;
        public string jsonWeaponGetItem;
        
        public SaveWeaponItemData(Transform weaponItemPos, WeaponGetItem weaponGetItem)
        {
            this.weaponItemPos = weaponItemPos;
            this.weaponGetItem = weaponGetItem;
        }

        public Transform WeaponItemPos => weaponItemPos;

        public WeaponGetItem WeaponGetItem => weaponGetItem;
    }
    
    [Serializable]
    public class SaveHealthArmorItemData
    {
        [NonSerialized] private Transform healthArmorItemPos;
        [NonSerialized] private GetHealthArmorItem healthArmorItem;

        public JsonTransform jsonHealthArmorItemPos;
        public string jsonHealthArmorItem;
        
        public SaveHealthArmorItemData(Transform healthArmorItemPos, GetHealthArmorItem healthArmorItem)
        {
            this.healthArmorItemPos = (healthArmorItemPos);
            this.healthArmorItem = healthArmorItem;
        }

        public Transform HealthArmorItemPos => healthArmorItemPos;

        public GetHealthArmorItem HealthArmorItem => healthArmorItem;
    }
    
    [Serializable]
    public class SavePlasmaItemData
    {
        [NonSerialized] private Transform plasmaItemPos;
        [NonSerialized] private PlasmaGetItem plasmaItem;

        public JsonTransform jsonPlasmaItemPos;
        public string jsonPlasmaItem;
        
        public SavePlasmaItemData(Transform plasmaItemPos, PlasmaGetItem plasmaItem)
        {
            this.plasmaItemPos = plasmaItemPos;
            this.plasmaItem = plasmaItem;
        }

        public Transform PlasmaItemPos => plasmaItemPos;

        public PlasmaGetItem PlasmaItem => plasmaItem;
    }
    
    [Serializable]
    public class SaveBulletData
    {
        [NonSerialized] private Transform bulletT;
        [NonSerialized] private Rigidbody bulletRb;


        [NonSerialized] private Bullet bullet;

        public JsonTransform jsonBulletT;
        public JsonRigidbody jsonBulletRb;

        public string jsonBullet;
        
        public SaveBulletData(Transform bulletT, Rigidbody bulletRb, Bullet bullet)
        {
            this.bulletT = (bulletT);
            if(bulletRb != null)
                this.bulletRb = (bulletRb);
            this.bullet = bullet;
        }

        public Transform BulletT => bulletT;

        public Rigidbody BulletRb => bulletRb;

        public Bullet Bullet => bullet;
    }
    
    [Serializable]
    public class SavePlayerData
    {
        [NonSerialized] private Transform playerT;
        [NonSerialized] private Rigidbody playerRb;


        [NonSerialized] private PlayerMainService playerMainService;
        [NonSerialized] private PlayerMovement playerMovement;
        [NonSerialized] private PlayerWeaponsManager playerWeaponsManager;
        [NonSerialized] private PlayerWeaponsBulletsManager playerWeaponsBulletsManager;
        [NonSerialized] private PlayerHookService playerHookService;
        [NonSerialized] private PlayerDashsService playerDashsService;
        [NonSerialized] private PlayerImmediatelyProtectionService playerImmediatelyProtectionService;

        public JsonTransform jsonPlayerT;
        public JsonRigidbody jsonPlayerRb;

        public string jsonPlayerMainService;
        public string jsonPlayerMovement;
        public string jsonPlayerWeaponsManager;
        public string jsonPlayerWeaponsBulletsManager;
        public JsonDictionary<int, int> bulletsCount;
        public JsonDictionary<int, int> bulletsMax;
        public JsonDictionary<string, float> plasmaReserves;
        public JsonDictionary<string, float> plasmaMaxReserves;

        public string jsonPlayerHookService;
        public string jsonPlayerDashsService;
        public string jsonPlayerImmediatelyProtectionService;
        
        public SavePlayerData(Transform playerT, Rigidbody playerRb, PlayerMainService playerMainService, PlayerMovement playerMovement, PlayerWeaponsManager playerWeaponsManager, PlayerWeaponsBulletsManager playerWeaponsBulletsManager, PlayerHookService playerHookService, PlayerDashsService playerDashsService, PlayerImmediatelyProtectionService playerImmediatelyProtectionService)
        {
            this.playerT = (playerT);
            this.playerRb = (playerRb);
            this.playerMainService = playerMainService;
            this.playerMovement = playerMovement;
            this.playerWeaponsManager = playerWeaponsManager;
            this.playerWeaponsBulletsManager = playerWeaponsBulletsManager;
            this.playerHookService = playerHookService;
            this.playerDashsService = playerDashsService;
            this.playerImmediatelyProtectionService = playerImmediatelyProtectionService;
        }

        public Transform PlayerT => playerT;

        public Rigidbody PlayerRb => playerRb;

        public PlayerMainService PlayerMainService => playerMainService;

        public PlayerMovement PlayerMovement => playerMovement;

        public PlayerWeaponsManager PlayerWeaponsManager => playerWeaponsManager;

        public PlayerWeaponsBulletsManager PlayerWeaponsBulletsManager => playerWeaponsBulletsManager;

        public PlayerHookService PlayerHookService => playerHookService;

        public PlayerDashsService PlayerDashsService => playerDashsService;

        public PlayerImmediatelyProtectionService PlayerImmediatelyProtectionService => playerImmediatelyProtectionService;
    }
    
    [Serializable]
    public class SaveTriggerData
    {
        [NonSerialized] private Transform triggerT;
        [NonSerialized] private LevelTrigger levelTrigger;

        public JsonTransform jsonTriggerT;
        public JsonBoxCollider jsonTriggerBoxCollider;
        public string jsonLevelTrigger;

        public SaveTriggerData(Transform triggerT, LevelTrigger levelTrigger)
        {
            this.triggerT = triggerT;
            this.levelTrigger = levelTrigger;
        }

        public Transform TriggerT => triggerT;
        
        public LevelTrigger LevelTrigger => levelTrigger;
    }
}

