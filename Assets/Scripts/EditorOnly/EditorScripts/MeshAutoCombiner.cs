using System;
using UnityEditor;
using UnityEngine;

public class MeshAutoCombiner : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] private Vector3 meshCombineArea = Vector3.one;
    [SerializeField] private Transform staticDecoratesPoint;
    
    [Space] [SerializeField] private int meshCombineZonesX = 1;
    [SerializeField] private int meshCombineZonesZ = 1;

    [Space] 
    [SerializeField] private string combineMeshesFolderPath;
    
    private bool isMeshsAutoCombineStarted = false;
    public bool IsMeshsAutoCombineStarted => isMeshsAutoCombineStarted;

    private Transform meshCombinerT;

    public void StartMeshsAutoCombine()
    {
        if (isMeshsAutoCombineStarted)
            return;

        isMeshsAutoCombineStarted = true;
        
        var toCombineDecorations = GameObject.FindGameObjectsWithTag("StaticDecoration");
        var toCombineDecorationsNames = 
            GetGameObjectsNames(toCombineDecorations);
        
        var LODnames = new string[] { "LOD0", "LOD1", "LOD2" };
        var toCombineDecorationsNotLOD = 
            GetGameObjectsNamesNotEndWith(toCombineDecorationsNames,LODnames);
        var toCombineDecorationsLOD0 = GetGameObjectsNamesEndWith(toCombineDecorationsNames, LODnames[0]);
        var toCombineDecorationsLOD1 = GetGameObjectsNamesEndWith(toCombineDecorationsNames, LODnames[1]);
        var toCombineDecorationsLOD2 = GetGameObjectsNamesEndWith(toCombineDecorationsNames, LODnames[2]);
        
        
        (Vector3[] combineAreasPoss, Vector3[] combineAreasScaless) = GetCombinerAreasData();
        var combineAreasCount = combineAreasPoss.Length;
        
        var globalCombineArea = new GameObject($"Main Combine Area");
        globalCombineArea.transform.parent = staticDecoratesPoint;
        
        for (int i = 0; i < combineAreasCount; i++)
        {
            var newCombineArea = new GameObject($"CombineArea {i+1}");
            newCombineArea.transform.position = combineAreasPoss[i];
            
            var meshNotLOD = new GameObject("meshNotLOD");
            var meshLOD0 = new GameObject("meshLOD0");
            var meshLOD1 = new GameObject("meshLOD1");
            var meshLOD2 = new GameObject("meshLOD2");

            newCombineArea.transform.parent = globalCombineArea.transform;

            meshNotLOD.transform.parent = newCombineArea.transform;
            meshLOD0.transform.parent = newCombineArea.transform;
            meshLOD1.transform.parent = newCombineArea.transform;
            meshLOD2.transform.parent = newCombineArea.transform;

            RecordCombineObjectsForUndoOperation();
            
            RecordItemsInAreaToMeshsObjects();

            MeshCombiner meshNotLODcombiner;
            MeshCombiner meshLOD0combiner;
            MeshCombiner meshLOD1combiner;
            MeshCombiner meshLOD2combiner;
            
            AddMeshCombinersToMeshObjects();

            SetSettingsToMeshCombiners();

            AreaMeshsCombine();
            
            //AddAndSetLODgroup();
            
            //DestroyCombiners();
            
            isMeshsAutoCombineStarted = false;
            
            void RecordItemsInAreaToMeshsObjects()
            {
                foreach (var item in toCombineDecorationsNotLOD)
                {
                    if(item == null)
                        continue;
                    
                    var isItemInArea = IsPositionInArea
                        (item.transform.position, combineAreasPoss[i], combineAreasScaless[i]);

                    if (isItemInArea)
                        item.transform.parent = meshNotLOD.transform;
                }
            
                foreach (var item in toCombineDecorationsLOD0)
                {
                    if(item == null)
                        continue;
                    
                    var isItemInArea = IsPositionInArea
                        (item.transform.position, combineAreasPoss[i], combineAreasScaless[i]);

                    if (isItemInArea)
                        item.transform.parent = meshLOD0.transform;
                }
            
                foreach (var item in toCombineDecorationsLOD1)
                {
                    if(item == null)
                        continue;
                    
                    var isItemInArea = IsPositionInArea
                        (item.transform.position, combineAreasPoss[i], combineAreasScaless[i]);

                    if (isItemInArea)
                        item.transform.parent = meshLOD1.transform;
                }
            
                foreach (var item in toCombineDecorationsLOD2)
                {
                    if(item == null)
                        continue;
                    
                    var isItemInArea = IsPositionInArea
                        (item.transform.position, combineAreasPoss[i], combineAreasScaless[i]);

                    if (isItemInArea)
                        item.transform.parent = meshLOD2.transform;
                }
                
            }
        
            void RecordCombineObjectsForUndoOperation()
            {
                Undo.RecordObjects(toCombineDecorations,"Mesh Auto Combine");
            }

            void AddMeshCombinersToMeshObjects()
            {
                meshNotLODcombiner = meshNotLOD.AddComponent<MeshCombiner>();
                meshLOD0combiner = meshLOD0.AddComponent<MeshCombiner>();
                meshLOD1combiner = meshLOD1.AddComponent<MeshCombiner>();
                meshLOD2combiner = meshLOD2.AddComponent<MeshCombiner>();
            }

            void SetSettingsToMeshCombiners()
            {
                meshNotLODcombiner.FolderPath = $"{combineMeshesFolderPath}/NotLOD";
                meshLOD0combiner.FolderPath = $"{combineMeshesFolderPath}/LOD0";
                meshLOD1combiner.FolderPath = $"{combineMeshesFolderPath}/LOD1";
                meshLOD2combiner.FolderPath = $"{combineMeshesFolderPath}/LOD2";

                meshNotLODcombiner.CreateMultiMaterialMesh = true;
                meshLOD0combiner.CreateMultiMaterialMesh = true;
                meshLOD1combiner.CreateMultiMaterialMesh = true;
                meshLOD2combiner.CreateMultiMaterialMesh = true;
            }

            void AreaMeshsCombine()
            {
                meshNotLODcombiner.CombineMeshes(false);
                meshLOD0combiner.CombineMeshes(false);
                meshLOD1combiner.CombineMeshes(false);
                meshLOD2combiner.CombineMeshes(false);
            }

            void AddLODgroup()
            {
                var LODgroup = newCombineArea.AddComponent<LODGroup>();

                /*var notLODrenderer = meshNotLOD.GetComponent<MeshRenderer>();

                var LOD0renderers = new Renderer[2] {meshLOD0.GetComponent<MeshRenderer>(),notLODrenderer};
                var LOD1renderers = new Renderer[2] {meshLOD1.GetComponent<MeshRenderer>(),notLODrenderer};
                var LOD2renderers = new Renderer[2] {meshLOD2.GetComponent<MeshRenderer>(),notLODrenderer};

                var LOD0 = new LOD(30, LOD0renderers);
                var LOD1 = new LOD(8, LOD1renderers);
                var LOD2 = new LOD(0.1f, LOD2renderers);

                var lodArray = new LOD[] {LOD0,LOD1,LOD2};
                
                LODgroup.SetLODs(lodArray);*/
            }
            
            void DestroyCombiners()
            {
                Destroy(meshNotLODcombiner);
                Destroy(meshLOD0combiner);
                Destroy(meshLOD1combiner);
                Destroy(meshLOD2combiner);
            }

        }
        
        string[] GetGameObjectsNames(GameObject[] gameObjects)
        {
            var resultNames = new string[gameObjects.Length];
            
            for (int i = 0; i < gameObjects.Length; i++)
            {
                resultNames[i] = gameObjects[i].name;
            }

            return resultNames;
        }
        
        GameObject[] GetGameObjectsNamesEndWith(string[] names,string sortEnd)
        {
            var resultSortNamesCount = 0;
            var preResultSortGameObjects = new GameObject[names.Length];
            
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].EndsWith(sortEnd))
                {
                    preResultSortGameObjects[resultSortNamesCount] = toCombineDecorations[i];
                    resultSortNamesCount++;
                }
            }

            var resultSortGameObjects = new GameObject[resultSortNamesCount + 1];

            for (int i = 0; i < resultSortNamesCount; i++)
            {
                resultSortGameObjects[i] = preResultSortGameObjects[i];
            }

            return resultSortGameObjects;
        }
        
        GameObject[] GetGameObjectsNamesNotEndWith(string[] names,string[] sortEnds)
        {
            var resultSortNamesCount = 0;
            var preResultSortGameObjects = new GameObject[names.Length];
            
            for (int i = 0; i < names.Length; i++)
            {
                var isNameNotEndWith = false;
                
                for (int j = 0; j < sortEnds.Length; j++)
                {
                    isNameNotEndWith = !names[i].EndsWith(sortEnds[j]);
                    
                    if(!isNameNotEndWith)
                        break;
                }
                
                if (isNameNotEndWith)
                {
                    preResultSortGameObjects[resultSortNamesCount] = toCombineDecorations[i];
                    resultSortNamesCount++;
                }
            }

            var resultSortGameObjects = new GameObject[resultSortNamesCount + 1];

            for (int i = 0; i < resultSortNamesCount; i++)
            {
                resultSortGameObjects[i] = preResultSortGameObjects[i];
            }

            return resultSortGameObjects;
        }

        bool IsPositionInArea(Vector3 pos, Vector3 areaPos, Vector3 areaScale)
        {
            var inXpos = pos.x <= areaPos.x + areaScale.x && pos.x >= areaPos.x + -areaScale.x;
            var inYpos = pos.y <= areaPos.y + areaScale.y && pos.y >= areaPos.y + -areaScale.y;
            var inZpos = pos.z <= areaPos.z + areaScale.z && pos.z >= areaPos.z + -areaScale.z;

            return inXpos && inYpos && inZpos;
        }
    }

    private void OnValidate()
    {
        meshCombinerT = transform;

        isMeshsAutoCombineStarted = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (meshCombinerT == null)
            return;

        Gizmos.DrawWireCube(meshCombinerT.position, meshCombineArea);

        DrawCombinerAreas();

        void DrawCombinerAreas()
        {
            var boxsCount = meshCombineZonesX * meshCombineZonesZ;
            
            var boxPoss = new Vector3[boxsCount];
            var boxScaless = new Vector3[boxsCount];

            (boxPoss, boxScaless) = GetCombinerAreasData();

            for (int i = 0; i < boxsCount; i++)
            {
                Gizmos.DrawWireCube(boxPoss[i],boxScaless[i]);
            }
            
        }

    }

    (Vector3[] poss, Vector3[] scales) GetCombinerAreasData()
    {
        var boxsCount = meshCombineZonesX * meshCombineZonesZ;
        Vector3[] resultPoss = new Vector3[boxsCount];
        Vector3[] resultScales = new Vector3[boxsCount];

        ComputeCombineAreaBoxsData();

        return (resultPoss, resultScales);
        
        void ComputeCombineAreaBoxsData()
        {
            var xPos = 0f;
            var zPos = 0f;

            var startPosition = meshCombinerT.position;
            
            var oneXshift = meshCombineArea.x * 2 / meshCombineZonesX;
            var oneZshift = meshCombineArea.z * 2 / meshCombineZonesZ;

            var simpleTimer = 0;

            for (int i = 0; i < meshCombineZonesX; i++)
            {
                xPos = startPosition.x - meshCombineArea.x;
                xPos += oneXshift / 2;

                xPos += oneXshift * i;

                for (int j = 0; j < meshCombineZonesZ; j++)
                {
                    zPos = startPosition.z - meshCombineArea.z;
                    zPos += oneZshift / 2;

                    zPos += oneZshift * j;

                    var resultPos = new Vector3(xPos / 2, startPosition.y, zPos / 2) + startPosition / 2;
                    var resultScale = new Vector3(oneXshift / 2, meshCombineArea.y, oneZshift / 2);

                    resultPoss[simpleTimer] = resultPos;
                    resultScales[simpleTimer] = resultScale;

                    simpleTimer++;
                }
            }
        }
    }

#endif
}
