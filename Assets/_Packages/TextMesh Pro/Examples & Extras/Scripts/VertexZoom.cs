using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace TMPro.Examples
{

    public class VertexZoom : MonoBehaviour
    {
        public float AngleMultiplier = 1.0f;
        public float SpeedMultiplier = 1.0f;
        public float CurveScale = 1.0f;

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
        private TMP_Text m_TextComponent;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
        private bool hasTextChanged;


        void Awake()
        {
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextComponent = GetComponent<TMP_Text>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
        }

        void OnEnable()
        {
            // Subscribe to event fired when text object has been regenerated.
#pragma warning disable CS0103 // Имя "TMPro_EventManager" не существует в текущем контексте.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
#pragma warning restore CS0103 // Имя "TMPro_EventManager" не существует в текущем контексте.
        }

        void OnDisable()
        {
            // UnSubscribe to event fired when text object has been regenerated.
#pragma warning disable CS0103 // Имя "TMPro_EventManager" не существует в текущем контексте.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
#pragma warning restore CS0103 // Имя "TMPro_EventManager" не существует в текущем контексте.
        }


        void Start()
        {
            StartCoroutine(AnimateVertexColors());
        }


        void ON_TEXT_CHANGED(Object obj)
        {
            if (obj == m_TextComponent)
                hasTextChanged = true;
        }

        /// <summary>
        /// Method to animate vertex colors of a TMP Text object.
        /// </summary>
        /// <returns></returns>
        IEnumerator AnimateVertexColors()
        {

            // We force an update of the text object since it would only be updated at the end of the frame. Ie. before this code is executed on the first frame.
            // Alternatively, we could yield and wait until the end of the frame when the text object will be generated.
            m_TextComponent.ForceMeshUpdate();

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_TextInfo" (возможно, отсутствует директива using или ссылка на сборку).
            TMP_TextInfo textInfo = m_TextComponent.textInfo;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_TextInfo" (возможно, отсутствует директива using или ссылка на сборку).

            Matrix4x4 matrix;
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_MeshInfo" (возможно, отсутствует директива using или ссылка на сборку).
            TMP_MeshInfo[] cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_MeshInfo" (возможно, отсутствует директива using или ссылка на сборку).

            // Allocations for sorting of the modified scales
            List<float> modifiedCharScale = new List<float>();
            List<int> scaleSortingOrder = new List<int>();

            hasTextChanged = true;

            while (true)
            {
                // Allocate new vertices 
                if (hasTextChanged)
                {
                    // Get updated vertex data
                    cachedMeshInfoVertexData = textInfo.CopyMeshInfoVertexData();

                    hasTextChanged = false;
                }

                int characterCount = textInfo.characterCount;

                // If No Characters then just yield and wait for some text to be added
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                // Clear list of character scales
                modifiedCharScale.Clear();
                scaleSortingOrder.Clear();

                for (int i = 0; i < characterCount; i++)
                {
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_CharacterInfo" (возможно, отсутствует директива using или ссылка на сборку).
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_CharacterInfo" (возможно, отсутствует директива using или ссылка на сборку).

                    // Skip characters that are not visible and thus have no geometry to manipulate.
                    if (!charInfo.isVisible)
                        continue;

                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    // Get the cached vertices of the mesh used by this text element (character or sprite).
                    Vector3[] sourceVertices = cachedMeshInfoVertexData[materialIndex].vertices;

                    // Determine the center point of each character at the baseline.
                    //Vector2 charMidBasline = new Vector2((sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2, charInfo.baseLine);
                    // Determine the center point of each character.
                    Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

                    // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
                    // This is needed so the matrix TRS is applied at the origin for each character.
                    Vector3 offset = charMidBasline;

                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                    //Vector3 jitterOffset = new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0);

                    // Determine the random scale change for each character.
                    float randomScale = Random.Range(1f, 1.5f);
                    
                    // Add modified scale and index
                    modifiedCharScale.Add(randomScale);
                    scaleSortingOrder.Add(modifiedCharScale.Count - 1);

                    // Setup the matrix for the scale change.
                    //matrix = Matrix4x4.TRS(jitterOffset, Quaternion.Euler(0, 0, Random.Range(-5f, 5f)), Vector3.one * randomScale);
                    matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, Vector3.one * randomScale);

                    destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                    destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                    destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                    destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

                    destinationVertices[vertexIndex + 0] += offset;
                    destinationVertices[vertexIndex + 1] += offset;
                    destinationVertices[vertexIndex + 2] += offset;
                    destinationVertices[vertexIndex + 3] += offset;

                    // Restore Source UVS which have been modified by the sorting
                    Vector2[] sourceUVs0 = cachedMeshInfoVertexData[materialIndex].uvs0;
                    Vector2[] destinationUVs0 = textInfo.meshInfo[materialIndex].uvs0;

                    destinationUVs0[vertexIndex + 0] = sourceUVs0[vertexIndex + 0];
                    destinationUVs0[vertexIndex + 1] = sourceUVs0[vertexIndex + 1];
                    destinationUVs0[vertexIndex + 2] = sourceUVs0[vertexIndex + 2];
                    destinationUVs0[vertexIndex + 3] = sourceUVs0[vertexIndex + 3];

                    // Restore Source Vertex Colors
                    Color32[] sourceColors32 = cachedMeshInfoVertexData[materialIndex].colors32;
                    Color32[] destinationColors32 = textInfo.meshInfo[materialIndex].colors32;

                    destinationColors32[vertexIndex + 0] = sourceColors32[vertexIndex + 0];
                    destinationColors32[vertexIndex + 1] = sourceColors32[vertexIndex + 1];
                    destinationColors32[vertexIndex + 2] = sourceColors32[vertexIndex + 2];
                    destinationColors32[vertexIndex + 3] = sourceColors32[vertexIndex + 3];
                }

                // Push changes into meshes
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    //// Sort Quads based modified scale
                    scaleSortingOrder.Sort((a, b) => modifiedCharScale[a].CompareTo(modifiedCharScale[b]));

                    textInfo.meshInfo[i].SortGeometry(scaleSortingOrder);

                    // Updated modified vertex attributes
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    textInfo.meshInfo[i].mesh.uv = textInfo.meshInfo[i].uvs0;
                    textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;

                    m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

    }
}