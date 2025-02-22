using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using Exception = System.Exception;
using Random = UnityEngine.Random;

public class LevelSaveLoadSystem : MonoBehaviour,IUsePlayerDevicesButtons
{
    private static bool _isLoadedScene;
    private static string _loadSaveName;

    private static bool _isLoadedNextLevelScene;
    private static string _toNextLevelSavedPlayerData;


    private string savesPath;

    [SerializeField] private bool staticMode; 
    public LevelSaveData levelSaveData;
    
    [SerializeField] private GameObject[] bulletsPrefabs;
    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private GameObject[] itemsPrefabs;

    private Camera mainCamera;
    [SerializeField] private MenuSystem menuSystem;
    
    private DeviceButton saveButton = new DeviceButton();
    
    public event Action OnSaveEvent;

    private void Awake()
    {
        savesPath = $"{Application.persistentDataPath}/Saves";
        mainCamera = Camera.main;
        menuSystem = FindObjectOfType<MenuSystem>();
        
        if (_isLoadedScene && !staticMode)
        {
            _isLoadedScene = false;
            
            StartCoroutine(LoadLevel(_loadSaveName));
            
            _loadSaveName = String.Empty;
        }

        if (_isLoadedNextLevelScene)
        {
            _isLoadedNextLevelScene = false;
            
            var playerMainService = FindObjectOfType<PlayerMainService>();

            var savedPlayerData = JsonUtility.FromJson<LevelSaveData.SavePlayerData>(_toNextLevelSavedPlayerData);
            
            var savePlayerMainService = new PlayerMainService();
            JsonUtility.FromJsonOverwrite(savedPlayerData.jsonPlayerMainService,savePlayerMainService);
            
            playerMainService.Load(savePlayerMainService);
            
            JsonUtility.FromJsonOverwrite
                (savedPlayerData.jsonPlayerWeaponsManager,playerMainService.weaponsManager);
            JsonUtility.FromJsonOverwrite
                (savedPlayerData.jsonPlayerWeaponsBulletsManager,playerMainService.weaponsBulletsManager);

            playerMainService.weaponsManager.bulletsManager = playerMainService.weaponsBulletsManager;
            playerMainService.weaponsManager.Load(playerMainService);
            playerMainService.weaponsBulletsManager.Load(savedPlayerData);

            var savedDashService = new PlayerDashsService(); 
            JsonUtility.FromJsonOverwrite(savedPlayerData.jsonPlayerDashsService,savedDashService);
            playerMainService.dashsService.Load(savedDashService);

            var savedProtectionService = new PlayerImmediatelyProtectionService();
            JsonUtility.FromJsonOverwrite(savedPlayerData.jsonPlayerImmediatelyProtectionService,
                savedProtectionService);
            playerMainService.immediatelyProtectionService.Load
                (savedProtectionService);

            var oldDataBase = new SuitInformationDataBase();
            JsonUtility.FromJsonOverwrite(savedPlayerData.jsonSuitInformationDataBase,oldDataBase);
            FindObjectOfType<SuitInformationDataBase>().Load(oldDataBase);
        }
    }

    private void Update()
    {
        if(staticMode)
            return;
            
        if (saveButton.IsGetButtonDown())
        {
            var levelName = CurrentLanguageData.GetText(levelSaveData.LevelNameTextId);

            var saveDate = DateTime.Now;
            var saveDateTxt = $"{saveDate.Second}{saveDate.Millisecond+Random.Range(0,100)}";
            var saveName = $"{levelName}_{saveDateTxt}";

            SaveLevel(saveName);
            
            OnSaveEvent?.Invoke();
        }

        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     StartLoadLevel("testSave");
        // }
    }

    private void SaveLevel(string saveName)
    {
        var jsonSave = 
            LevelSaveData.mainLevelSaveData.SaveToJson();
        
        var toFilePath = $"{savesPath}/{saveName}.bob";

        if (!Directory.Exists(savesPath))
            Directory.CreateDirectory(savesPath);

        var binaryFormatter = 
            new BinaryFormatter();
        var fileStream = 
            File.Create(toFilePath);
            
        binaryFormatter.Serialize(fileStream,jsonSave);
        
        print($"Game save {saveName} saved in {savesPath} ");
        
        fileStream.Close();
        
        CreateScreenshot(Screen.width,Screen.height,savesPath,saveName);
    }

    public void StartLoadLevel(string saveName)
    {
        _isLoadedScene = true;
        _loadSaveName = saveName;

        var levelData = new LevelSaveData();
        var levelDataJson = GetJsonLevelSaveData(saveName);
        
        JsonUtility.FromJsonOverwrite(levelDataJson,levelData);
        
        SceneManager.LoadScene(levelData.LevelSceneId);
    }

    public void DeleteSave(string saveName)
    {
        var savePath = $"{savesPath}/{saveName}.bob";
        
        File.Delete(savePath);
    }
    
    private IEnumerator LoadLevel(string saveName)
    {
        var jsonSavedLevelData = GetJsonLevelSaveData(saveName);
        
        var playerMainService = levelSaveData.playerMainService;
        
        JsonUtility.FromJsonOverwrite(jsonSavedLevelData,levelSaveData);
        
        LoadPlayer();
        
        ClearAllDataObjects(levelSaveData);

        yield return new WaitForEndOfFrame();

        DestroyAllDataObjects(levelSaveData);
        
        JsonUtility.FromJsonOverwrite(jsonSavedLevelData,levelSaveData);
        
        LoadSavedLevelData(levelSaveData);
        
        ClearAllDataObjects(levelSaveData,false);
        
        void LoadPlayer()
        {
            var playerData = levelSaveData.PlayerData;
            
            LoadPlayerMainService();
            
            LoadPlayerMovement();
                
            LoadWeaponSystem();
            
            LoadDashSystem();
            
            LoadImmediatelyProtection();
            
            LoadHookService();
            
            LoadSuitDatabase();
            
            LoadTaskService();
            
            void LoadPlayerMainService()
            {
                var savePlayerMainService = new PlayerMainService();
                JsonUtility.FromJsonOverwrite(playerData.jsonPlayerMainService,savePlayerMainService);
                
                playerMainService.Load(savePlayerMainService);
            }
            
            void LoadPlayerMovement()
            {
                var playerMovement = playerMainService.playerMovement;

                var playerT = playerMovement.Body;
                var playerRb = playerMovement.PlayerRb;
                    
                    
                FixedJsonUtilityFunc.JsonToNormal(playerData.jsonPlayerT,playerT);
                FixedJsonUtilityFunc.JsonToNormal(playerData.jsonPlayerRb,playerRb);
            }

            void LoadWeaponSystem()
            {
                JsonUtility.FromJsonOverwrite(playerData.jsonPlayerWeaponsManager,playerMainService.weaponsManager);
                JsonUtility.FromJsonOverwrite(playerData.jsonPlayerWeaponsBulletsManager,playerMainService.weaponsBulletsManager);

                playerMainService.weaponsManager.bulletsManager = playerMainService.weaponsBulletsManager;
                playerMainService.weaponsManager.Load(playerMainService);
                playerMainService.weaponsBulletsManager.Load(playerData);
            }

            void LoadDashSystem()
            {
                var savedDashService = new PlayerDashsService();
                JsonUtility.FromJsonOverwrite(playerData.jsonPlayerDashsService,savedDashService);
                
                playerMainService.dashsService.Load(savedDashService);
            }

            void LoadImmediatelyProtection()
            {
                var jsonSavedProtection = playerData.jsonPlayerImmediatelyProtectionService;
                var savedProtection = new PlayerImmediatelyProtectionService();
                
                JsonUtility.FromJsonOverwrite(jsonSavedProtection,savedProtection);
                
                playerMainService.immediatelyProtectionService.Load(savedProtection);
            }

            void LoadHookService()
            {
                var jsonSavedHook = playerData.jsonPlayerHookService;
                var savedHook = new PlayerHookService();
                
                JsonUtility.FromJsonOverwrite(jsonSavedHook,savedHook);
                
                playerMainService.hookService.Load(savedHook);
            }

            void LoadSuitDatabase()
            {
                var savedDataBase = new SuitInformationDataBase();
                JsonUtility.FromJsonOverwrite(playerData.jsonSuitInformationDataBase,savedDataBase);
                
                FindObjectOfType<SuitInformationDataBase>().Load(savedDataBase);
            }
            
            void LoadTaskService()
            {
                var saveLevelTaskService = new LevelTaskService();
                JsonUtility.FromJsonOverwrite(playerData.jsonLevelTaskService,saveLevelTaskService);

                FindObjectOfType<LevelTaskService>().Load(saveLevelTaskService);
            }
        }

        void DestroyAllDataObjects(LevelSaveData data)
        {
            // foreach (var triggerData in data.LevelTriggers)
            // {
            //     if(triggerData.LevelTrigger == null)
            //         return;
            //     
            //     DestroyImmediate(triggerData.LevelTrigger.gameObject);
            // }
            
            // foreach (var scenariosData in data.LevelSpawnScenarios)
            // {
            //     if(scenariosData == null)
            //         return;
            //     
            //     DestroyImmediate(scenariosData.gameObject);
            // }
            
            foreach (var bulletData in data.ToSaveBulletData)
            {
                if(bulletData.Bullet == null)
                    return;
                
                DestroyImmediate(bulletData.Bullet.gameObject);
            }
            
            foreach (var enemyData in data.ToSaveEnemyData)
            {
                if(enemyData.EnemyType == null)
                    return;
                
                DestroyImmediate(enemyData.EnemyType.gameObject);
            }
            
            foreach (var itemData in data.ToSaveItemData)
            {
                if(itemData.ItemService == null)
                    return;
                
                DestroyImmediate(itemData.ItemService.gameObject);
            }
        }

        void ClearAllDataObjects(LevelSaveData data,bool clearStaticDataObjects = true)
        {
            if (clearStaticDataObjects)
            {
                data.ToSaveLevelTriggers.Clear();
                data.LevelTransformTranslateSavedSystems.Clear();
                data.LevelTransformTranslateSystems.Clear();
            }

            data.ToSaveBulletData.Clear();
            data.SavedLevelSpawnScenarios.Clear();
            data.LevelTransformTranslateSavedSystems.Clear();
            data.ToSaveEnemyData.Clear();
            data.ToSaveItemData.Clear();
        }
        
        void LoadSavedLevelData(LevelSaveData levelData)
        {
            LoadBullets();
            
            LoadEnemies();
            
            LoadItems();
            
            LoadTriggers();
            
            LoadSpawnScenarios();
            
            LoadTransformAnimationSystems();
            
            LoadLevelPassageService();
            
            LoadLevelPassagePointsService();
            
            void LoadBullets()
            {
                foreach (var bulletData in levelData.ToSaveBulletData)
                {
                    var jsonSaveBullet = bulletData.jsonBullet;
                    var saveBullet = new Bullet();
                    
                    JsonUtility.FromJsonOverwrite(jsonSaveBullet,saveBullet);

                    var saveBulletID = saveBullet.BulletId;

                    var restoredBullet = Instantiate(bulletsPrefabs[saveBulletID - 1]).GetComponent<Bullet>();
                    
                    var restoredBulletT = restoredBullet.body_;
                    var restoredBulletRb = new Rigidbody();

                    if (restoredBullet.rigidbody_ != null)
                        restoredBulletRb = restoredBullet.rigidbody_;
                    
                    restoredBullet.Load(saveBullet.CurrentTime);
                    
                    FixedJsonUtilityFunc.JsonToNormal(bulletData.jsonBulletT,restoredBulletT);
                    
                    if(restoredBullet.rigidbody_ != null) 
                        FixedJsonUtilityFunc.JsonToNormal(bulletData.jsonBulletRb,restoredBulletRb);
                }
            }

            void LoadEnemies()
            {
                foreach (var enemyData in levelData.ToSaveEnemyData)
                {
                    var jsonBotTypeData = enemyData.jsonEnemyType;
                    var jsonBotAttackData = enemyData.jsonEnemyAttack;

                    DefaultBot savedBot = new ParentElementBot();
                    DefaultBotAttack savedBotAttack = new ParentElementAttack();
                    
                    JsonUtility.FromJsonOverwrite(jsonBotTypeData,savedBot);
                    JsonUtility.FromJsonOverwrite(jsonBotAttackData,savedBotAttack);
                    
                    var toSpawnBotPrefab = FindBotInPrefabs(enemyData.enemyTypeName);
                    var restoredBotObject = Instantiate(toSpawnBotPrefab);
                    
                    var restoredBot = restoredBotObject.GetComponent<DefaultBot>();
                    var restoredBotAttack = restoredBot.botAttack;
                    var restoredBotT = restoredBot.body;
                    var restoredBotRb = restoredBot.bodyRB;

                    FixedJsonUtilityFunc.JsonToNormal(enemyData.jsonEnemyPos,restoredBotT);
                    FixedJsonUtilityFunc.JsonToNormal(enemyData.jsonEnemyRb,restoredBotRb);
                    
                    restoredBot.Load(savedBot);
                    restoredBotAttack.Load(savedBotAttack);
                    
                    GameObject FindBotInPrefabs(string defaultBotTypeName)
                    {
                        foreach (var enemyPrefab in enemiesPrefabs)
                        {
                            var enemyBotPrefabTypeName = enemyPrefab.GetComponent<DefaultBot>().GetType().Name;

                            if (enemyBotPrefabTypeName == defaultBotTypeName)
                                return enemyPrefab;
                        }

                        throw new Exception($"{defaultBotTypeName} not founded.");
                    }
                }    
            }

            void LoadItems()
            {
                foreach (var plasmaItemData in levelData.ToSaveItemData)
                {
                    OrdinaryPlayerItem itemMain = new PlayerItemContainer();
                    JsonUtility.FromJsonOverwrite(plasmaItemData.jsonItemService,itemMain);

                    SpawnItem(itemMain.ItemId,plasmaItemData.jsonItemT);
                }
                
                void SpawnItem(int id, JsonTransform transf)
                {
                    foreach (var itemPrefab in itemsPrefabs)
                    {
                        var itemPrefabId = itemPrefab.GetComponent<OrdinaryPlayerItem>().ItemId;
                        
                        if(id != itemPrefabId)
                            continue;

                        var recoveredItem = Instantiate(itemPrefab);
                        FixedJsonUtilityFunc.JsonToNormal(transf, recoveredItem.transform);
                        
                        break;
                    }
                }
            }

            void LoadTriggers()
            {
                var allTriggersInLevel = 
                    new LevelTrigger [levelSaveData.ToSaveLevelTriggers.Count];
                
                for (int i = 0; i < levelSaveData.ToSaveLevelTriggers.Count; i++)
                {
                    allTriggersInLevel[i] = levelSaveData.ToSaveLevelTriggers[i].LevelTrigger;
                }

                
                for (var i = 0; i < allTriggersInLevel.Length; i++)
                {
                    var newTrigger = allTriggersInLevel[i];

                    for (var j = 0; j < levelData.ToSaveLevelTriggers.Count; j++)
                    {
                        var saveTriggerData = levelData.ToSaveLevelTriggers[j];
                        var savedTriggerJson = saveTriggerData.jsonLevelTrigger;
                        var savedTrigger = new LevelTriggerBase();
                        JsonUtility.FromJsonOverwrite(savedTriggerJson, savedTrigger);

                        if (newTrigger.TriggerId == savedTrigger.TriggerId)
                        {
                            newTrigger.isTriggerActive = savedTrigger.isTriggerActive;
                            saveTriggerData = new LevelSaveData.SaveTriggerData(newTrigger.transform, newTrigger);
                            
                            break;
                        }
                    }
                }
            }
            
            void LoadSpawnScenarios()
            {
                var allSpawnersInLevel = 
                    new LevelSpawnScenario [levelSaveData.LevelSpawnScenarios.Count];
                
                for (int i = 0; i < levelSaveData.LevelSpawnScenarios.Count; i++)
                {
                    allSpawnersInLevel[i] = levelSaveData.LevelSpawnScenarios[i];
                }
                
                for (var i = 0; i < allSpawnersInLevel.Length; i++)
                {
                    var newSpawner = allSpawnersInLevel[i];

                    foreach (var saveSpawnerJson in levelData.SavedLevelSpawnScenarios)
                    {
                        var savedSpawnerJson = saveSpawnerJson;
                        var savedSpawner = new LevelSpawnScenario();
                        JsonUtility.FromJsonOverwrite(savedSpawnerJson, savedSpawner);
                        
                        if (newSpawner.SpawnerId == savedSpawner.SpawnerId)
                        {
                            newSpawner.LoadSpawnScenario(savedSpawner);
                            
                            break;
                        }
                    }
                }
            }

            void LoadTransformAnimationSystems()
            {
                var allSavesTranslateSystems = 
                    new LevelTransformAnimationSystem[levelData.LevelTransformTranslateSavedSystems.Count];
                
                for (int i = 0; i < allSavesTranslateSystems.Length; i++)
                {
                    var savedJsonTranslateSystem = levelData.LevelTransformTranslateSavedSystems[i];
                    allSavesTranslateSystems[i] = new LevelTransformAnimationSystem();
                    
                    JsonUtility.FromJsonOverwrite(savedJsonTranslateSystem, allSavesTranslateSystems[i]);
                }
                
                foreach (var savedTransformSystem in allSavesTranslateSystems)
                {
                    foreach (var existTranslateSystem in levelData.LevelTransformTranslateSystems)
                    {
                        if (existTranslateSystem.Id != savedTransformSystem.Id)
                            continue;
                        
                        existTranslateSystem.Load(savedTransformSystem);
                        
                        break;
                    }
                    
                }
            }

            void LoadLevelPassageService()
            {
                var saveLevelPassageService = new LevelPassageService(); 
                JsonUtility.FromJsonOverwrite(levelData.SavedLevelPassageService,saveLevelPassageService);
                
                levelData.LevelPassageService.Load(saveLevelPassageService);
            }

            void LoadLevelPassagePointsService()
            {
                var savedLevelPointsService = new LevelPassagePointsService();
                
                JsonUtility.FromJsonOverwrite(levelSaveData.JsonLevelPassagePointsService,savedLevelPointsService);
                FindObjectOfType<LevelPassagePointsService>().Load(savedLevelPointsService);
                
            }
        }
    }

    public void LoadNextLevel(
        int nextLevelId,
        PlayerMainService playerMainService,
        PlayerWeaponsManager playerWeaponsManager,
        PlayerWeaponsBulletsManager playerWeaponsBulletsManager)
    {
        _isLoadedNextLevelScene = true;

        var savedPlayerData = new LevelSaveData.SavePlayerData(
            transform,
            new Rigidbody(),
            playerMainService,
            new PlayerMovement(),
            playerMainService.weaponsManager,
            playerMainService.weaponsBulletsManager,
            new PlayerHookService(),
            playerMainService.dashsService,
            playerMainService.immediatelyProtectionService,
            FindObjectOfType<SuitInformationDataBase>(),
            new LevelTaskService());

        savedPlayerData.jsonPlayerMainService = JsonUtility.ToJson(playerMainService);
        savedPlayerData.jsonPlayerWeaponsManager = JsonUtility.ToJson(playerWeaponsManager);
        savedPlayerData.jsonPlayerWeaponsBulletsManager = JsonUtility.ToJson(playerWeaponsBulletsManager);
        
        savedPlayerData.plasmaReserves = 
            new JsonDictionary<string, float>(playerWeaponsBulletsManager.PlasmaReserves);
        savedPlayerData.plasmaMaxReserves =
            new JsonDictionary<string, float>(playerWeaponsBulletsManager.PlasmaMaxReserves);
        savedPlayerData.bulletsCount =
            new JsonDictionary<int, int>(playerWeaponsBulletsManager.BulletsCount);
        savedPlayerData.bulletsMax = 
            new JsonDictionary<int, int>(playerWeaponsBulletsManager.BulletsMax);

        savedPlayerData.jsonPlayerDashsService = JsonUtility.ToJson(playerMainService.dashsService);
        savedPlayerData.jsonPlayerImmediatelyProtectionService =
            JsonUtility.ToJson(playerMainService.immediatelyProtectionService);
        savedPlayerData.jsonSuitInformationDataBase = JsonUtility.ToJson(savedPlayerData.SuitInformationDataBase);
        
        _toNextLevelSavedPlayerData = JsonUtility.ToJson(savedPlayerData);
        
        SceneManager.LoadScene(nextLevelId);
    }
    
    public string GetJsonLevelSaveData(string saveName)
    {
        var toFilePath = $"{savesPath}/{saveName}.bob";
        var saveFileIsExists = File.Exists(toFilePath);

        if (!saveFileIsExists)
        {
            throw new DataMisalignedException("?? ???? ?????????? ??? ???????");
        }

        var binaryFormatter = 
            new BinaryFormatter();
        var saveFileStream = 
            File.Open(toFilePath,FileMode.Open);

        var jsonLevelData = 
            (string)binaryFormatter.Deserialize(saveFileStream);
        
        saveFileStream.Close();

        return jsonLevelData;
    }

    public void CreateScreenshot(int width,int height, string savePath,string saveName)
    {
        StartCoroutine(CreateScreenshotCoroutine());
        
        IEnumerator CreateScreenshotCoroutine()
        {
            menuSystem.CurrentMenuData.menu.SetActive(false);
            
            yield return new WaitForEndOfFrame();

            mainCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
            var renderTexture = mainCamera.targetTexture;
            
            var resultScreenshot = new Texture2D(width, height);
            var screenshotRect = new Rect(0, 0, width, height);
            resultScreenshot.ReadPixels(screenshotRect, 0, 0);
            
            var screenshotByte = resultScreenshot.EncodeToPNG();
            File.WriteAllBytes($"{savePath}/{saveName}_screenshot.png", screenshotByte);
            
            RenderTexture.ReleaseTemporary(renderTexture);
            mainCamera.targetTexture = null;
            
            menuSystem.CurrentMenuData.menu.SetActive(true);
        }
    }
    
    public DeviceButton[] GetUsesDevicesButtons()
    {
        return new [] {saveButton};
    }
}