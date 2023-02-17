using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class LevelTransformAnimationSystem : MonoBehaviour
{

    [SerializeField] private TransformAnimationData[] transformScenarios = new TransformAnimationData[0];
    [SerializeField] private GlobalStateData[] globalScenarioStates = new GlobalStateData[0];
    [HideInInspector] [SerializeField] private float localStopwatch;
    [SerializeField] private int id;

    public int Id => id;

    public TransformAnimationData[] TransformScenarios => transformScenarios;
    
    public void Load(LevelTransformAnimationSystem savedTransformAnimationSystem)
    {
        for (var i = 0; i < transformScenarios.Length; i++)
        {
            var scenarioData = transformScenarios[i];
            FixedJsonUtilityFunc.JsonToNormal(savedTransformAnimationSystem.transformScenarios[i].savedObjectT, scenarioData.ObjectT);
        }

        globalScenarioStates = savedTransformAnimationSystem.globalScenarioStates;

        localStopwatch = savedTransformAnimationSystem.localStopwatch;

        for (var i = 0; i < transformScenarios.Length; i++)
        {
            transformScenarios[i].CurrentScenarioUnitId = savedTransformAnimationSystem.transformScenarios[i].CurrentScenarioUnitId;

            for (var j = 0; j < transformScenarios[i].TransformScenarioUnits.Length; j++)
            {
                var scenarioUnit = transformScenarios[i].TransformScenarioUnits[j];

                scenarioUnit.currentAnimationEndTime = savedTransformAnimationSystem.transformScenarios[i]
                    .TransformScenarioUnits[j].currentAnimationEndTime;
            }
        }
    }
    
    private void Start()
    {
        SetStartTransforms();
        void SetStartTransforms()
        {
            foreach (var scenario in transformScenarios)
            {
                if (scenario.StartTransformUnitData.IsPositionOn)
                {
                    if (!scenario.StartTransformUnitData.IsPositionLocal)
                        scenario.ObjectT.position = scenario.StartTransformUnitData.Position;
                    else
                        scenario.ObjectT.localPosition = scenario.StartTransformUnitData.Position;
                }

                if(scenario.StartTransformUnitData.IsRotationOn)
                    scenario.ObjectT.rotation = Quaternion.Euler(scenario.StartTransformUnitData.Rotation);

                if (scenario.StartTransformUnitData.IsScaleOn)
                    scenario.ObjectT.localScale = scenario.StartTransformUnitData.Scale;
            }
        }

        SetScenariosAdditionalServices();
        void SetScenariosAdditionalServices()
        {
            foreach (var scenario in transformScenarios)
            {
                if (scenario.ObjectT.GetChild(0).TryGetComponent(out LevelTranslateScenarioStuckDetector stuckDetector))
                    scenario.stuckDetector = stuckDetector;
                
                if (scenario.ObjectT.GetChild(1).TryGetComponent(out LevelTranslateScenarioCorrectPhysicService physicService))
                    scenario.physicService = physicService;
            }
        }
        
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
    }

    private void Update()
    {
        localStopwatch += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        AnimationsWork();
    }

    private void AnimationsWork()
    {
        foreach (var scenario in transformScenarios)
        {
            if(scenario.ScenarioState == TransformScenarioState.AutoRepeating)
                AutoRepeatScenarioPreparing();
            
            void AutoRepeatScenarioPreparing()
            {
                var isScenarioStartNow =
                    scenario.CurrentScenarioUnitId == 0 && scenario.TransformScenarioUnits.Length > 0;
                
                if (isScenarioStartNow)
                    scenario.CurrentScenarioUnitId = 1;
                
                var currentScenarioUnit = scenario.TransformScenarioUnits[scenario.CurrentScenarioUnitId-1];
                
                if(isScenarioStartNow)
                    currentScenarioUnit.currentAnimationEndTime = localStopwatch + currentScenarioUnit.AnimationTime;
                
                var isScenarioCompleted =
                    localStopwatch > currentScenarioUnit.currentAnimationEndTime;

                if (isScenarioCompleted)
                {
                    ContinueScenario();

                    void ContinueScenario()
                    {
                        var currentStateIsLast
                            = scenario.CurrentScenarioUnitId == scenario.TransformScenarioUnits.Length;
                        
                        if (currentStateIsLast)
                            scenario.CurrentScenarioUnitId = 1;
                        else
                            scenario.CurrentScenarioUnitId++;

                        var newCurrentScenarioUnit = scenario.TransformScenarioUnits[scenario.CurrentScenarioUnitId-1];
                        
                        newCurrentScenarioUnit.currentAnimationEndTime = 
                            localStopwatch + newCurrentScenarioUnit.AnimationTime;
                    }
                }
            }

            ObjectTranslateProcess();
            
            void ObjectTranslateProcess()
            {
                var scenarioObj = scenario.ObjectT;
                var currentScenarioUnit = GetCurrentScenarioUnitData();
                var lerpEnabled = currentScenarioUnit.LerpAnimationEnabled;

                if (IsAnimationObjectStuck())
                {
                    scenarioObj.position -= scenario.physicService.CurrentObjectVelocity/4;
                    
                    return;
                }

                if (currentScenarioUnit.IsPositionOn)
                {
                    var oldObjectPos = scenarioObj.position;
                    CalculateAndSetPosition();
                    var newObjectPos = scenarioObj.position;
                    
                    var objectTranslateDirectionDistance = -(oldObjectPos - newObjectPos);
                    if(scenario.ObjectTranslatePosEvent != null)
                        scenario.ObjectTranslatePosEvent.Invoke(objectTranslateDirectionDistance);
                    
                    void CalculateAndSetPosition()
                    {
                        if (currentScenarioUnit.IsPositionLocal)
                        {
                            scenarioObj.localPosition =
                                CalculateNextValue(scenarioObj.localPosition, currentScenarioUnit.Position);   
                        }
                        else
                            scenarioObj.position =
                                CalculateNextValue(scenarioObj.position, currentScenarioUnit.Position);
                    }
                }
                if (currentScenarioUnit.IsScaleOn)
                {
                    CalculateAndSetScale();
                    
                    void CalculateAndSetScale()
                    {
                        scenarioObj.localScale =
                            CalculateNextValue(scenarioObj.localScale, currentScenarioUnit.Scale);
                    }
                }

                scenario.savedObjectT = FixedJsonUtilityFunc.GetJsonVersion(scenarioObj);
                
                Vector3 CalculateNextValue(Vector3 currentValue,Vector3 endValue)
                {
                    Vector3 resultValue;

                    if (!lerpEnabled)
                    {
                        NotLerpCalculateProcess();
                        
                        void NotLerpCalculateProcess()
                        {
                            var moveDirection = (endValue - currentValue).normalized;
                            const float moveDirectionSmoothness = 10;
                            moveDirection /= moveDirectionSmoothness;
                            moveDirection *= currentScenarioUnit.AnimationSpeed;
                            
                            resultValue = (currentValue + moveDirection);

                            if ((endValue - resultValue).normalized == -moveDirection.normalized)
                                resultValue = endValue;
                        }
                    }
                    else
                    {
                        LerpCalculateProcess();
                        
                        void LerpCalculateProcess()
                        {
                            var lerpSpeed = currentScenarioUnit.AnimationSpeed;
                            const float lerpSpeedSmoothness = 100;
                            lerpSpeed /= lerpSpeedSmoothness;

                            resultValue = Vector3.Lerp(currentValue, endValue, lerpSpeed);
                        }
                    }

                    return resultValue;
                }
                TransformAnimationUnitData GetCurrentScenarioUnitData()
                {
                    TransformAnimationUnitData resultData; 
                    
                    if (scenario.CurrentScenarioUnitId == 0)
                        resultData = scenario.StartTransformUnitData;
                    else
                        resultData = scenario.TransformScenarioUnits[scenario.CurrentScenarioUnitId - 1];

                    return resultData;
                }

                bool IsAnimationObjectStuck()
                {
                    return scenario.stuckDetector.IsStuck;
                }
                
            }
        }
    }

    [Serializable]
    public class TransformAnimationData
    {
        [SerializeField] private Transform objectT;
        [HideInInspector] [SerializeField] public LevelTranslateScenarioStuckDetector stuckDetector;
        [HideInInspector] [SerializeField] public LevelTranslateScenarioCorrectPhysicService physicService;
        [SerializeField] public JsonTransform savedObjectT;
        [SerializeField] private int currentScenarioUnitId;
        [SerializeField] private TransformScenarioState scenarioState;
        
        [Space] 
        
        [SerializeField] private TransformAnimationUnitData startTransformUnitData;
        [SerializeField] private TransformAnimationUnitData[] transformScenarioUnits;
        public UnityEvent<Vector3> ObjectTranslatePosEvent;
        
        public Transform ObjectT
        {
            get => objectT;
            set
            {
                if (value == null)
                    throw new NullReferenceException("??? ?????? ?? ?????? ??????? ?? ?????? ???????????, ????????? ?? ?????????");
                
                objectT = value;
            }
        }
        
        public int CurrentScenarioUnitId
        {
            get => currentScenarioUnitId;
            
            set
            {
                if (value < 0 || value > transformScenarioUnits.Length)
                    throw new InvalidDataException($"State id not may be {value}");

                currentScenarioUnitId = value;
            }
        }

        public TransformScenarioState ScenarioState => scenarioState;

        public TransformAnimationUnitData StartTransformUnitData => startTransformUnitData;

        public TransformAnimationUnitData[] TransformScenarioUnits => transformScenarioUnits;
    }
    
    [Serializable]
    public class TransformAnimationUnitData
    {
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector3 scale;
        [Space]
        [SerializeField] private bool isPositionOn = true;
        [SerializeField] private bool isRotationOn;
        [SerializeField] private bool isScaleOn;
        [Space] 
        [SerializeField] private bool isPositionLocal = true;
        [Space]
        [SerializeField] private float animationSpeed = 1;
        [SerializeField] private float animationTime = 5;
        [SerializeField] public float currentAnimationEndTime;
        [SerializeField] private float animationStartEndSmoothness = 1;
        [SerializeField] private bool lerpAnimationEnabled = true;

        public Vector3 Position => position;

        public Vector3 Rotation => rotation;

        public Vector3 Scale => scale;

        public bool IsPositionOn => isPositionOn;

        public bool IsRotationOn => isRotationOn;

        public bool IsScaleOn => isScaleOn;

        public bool IsPositionLocal => isPositionLocal;

        public float AnimationSpeed => animationSpeed;

        public float AnimationTime => animationTime;

        public float AnimationStartEndSmoothness => animationStartEndSmoothness;

        public bool LerpAnimationEnabled => lerpAnimationEnabled;
    }
    
    [Serializable]
    public class GlobalStateData
    {
        [SerializeField] private int[] scenarioStates;

        public int[] ScenarioStates => scenarioStates;
    }
    
    public enum TransformScenarioState 
    {
        Static,
        AutoRepeating
    }
}