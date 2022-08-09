using UnityEngine;
#pragma warning disable CS0234 // Тип или имя пространства имен "EventSystems" не существует в пространстве имен "UnityEngine" (возможно, отсутствует ссылка на сборку).
using UnityEngine.EventSystems;
#pragma warning restore CS0234 // Тип или имя пространства имен "EventSystems" не существует в пространстве имен "UnityEngine" (возможно, отсутствует ссылка на сборку).
using System.Collections;


namespace TMPro.Examples
{

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "IPointerEnterHandler" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "IPointerExitHandler" (возможно, отсутствует директива using или ссылка на сборку).
    public class TMP_TextSelector_A : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "IPointerExitHandler" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "IPointerEnterHandler" (возможно, отсутствует директива using или ссылка на сборку).
    {
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
        private TextMeshPro m_TextMeshPro;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).

        private Camera m_Camera;

        private bool m_isHoveringObject;
        private int m_selectedLink = -1;
        private int m_lastCharIndex = -1;
        private int m_lastWordIndex = -1;

        void Awake()
        {
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextMeshPro = gameObject.GetComponent<TextMeshPro>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
            m_Camera = Camera.main;

            // Force generation of the text object so we have valid data to work with. This is needed since LateUpdate() will be called before the text object has a chance to generated when entering play mode.
            m_TextMeshPro.ForceMeshUpdate();
        }


        void LateUpdate()
        {
            m_isHoveringObject = false;

#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
            if (TMP_TextUtilities.IsIntersectingRectTransform(m_TextMeshPro.rectTransform, Input.mousePosition, Camera.main))
            {
                m_isHoveringObject = true;
            }
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.

            if (m_isHoveringObject)
            {
                #region Example of Character Selection
#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                int charIndex = TMP_TextUtilities.FindIntersectingCharacter(m_TextMeshPro, Input.mousePosition, Camera.main, true);
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                if (charIndex != -1 && charIndex != m_lastCharIndex && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    //Debug.Log("[" + m_TextMeshPro.textInfo.characterInfo[charIndex].character + "] has been selected.");

                    m_lastCharIndex = charIndex;

                    int meshIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].materialReferenceIndex;

                    int vertexIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].vertexIndex;

                    Color32 c = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);

                    Color32[] vertexColors = m_TextMeshPro.textInfo.meshInfo[meshIndex].colors32;

                    vertexColors[vertexIndex + 0] = c;
                    vertexColors[vertexIndex + 1] = c;
                    vertexColors[vertexIndex + 2] = c;
                    vertexColors[vertexIndex + 3] = c;

                    //m_TextMeshPro.mesh.colors32 = vertexColors;
                    m_TextMeshPro.textInfo.meshInfo[meshIndex].mesh.colors32 = vertexColors;
                }
                #endregion

                #region Example of Link Handling
                // Check if mouse intersects with any links.
#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, Input.mousePosition, m_Camera);
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.

                // Clear previous link selection if one existed.
                if ((linkIndex == -1 && m_selectedLink != -1) || linkIndex != m_selectedLink)
                {
                    //m_TextPopup_RectTransform.gameObject.SetActive(false);
                    m_selectedLink = -1;
                }

                // Handle new Link selection.
                if (linkIndex != -1 && linkIndex != m_selectedLink)
                {
                    m_selectedLink = linkIndex;

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_LinkInfo" (возможно, отсутствует директива using или ссылка на сборку).
                    TMP_LinkInfo linkInfo = m_TextMeshPro.textInfo.linkInfo[linkIndex];
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_LinkInfo" (возможно, отсутствует директива using или ссылка на сборку).

                    // The following provides an example of how to access the link properties.
                    //Debug.Log("Link ID: \"" + linkInfo.GetLinkID() + "\"   Link Text: \"" + linkInfo.GetLinkText() + "\""); // Example of how to retrieve the Link ID and Link Text.

                    Vector3 worldPointInRectangle;

                    RectTransformUtility.ScreenPointToWorldPointInRectangle(m_TextMeshPro.rectTransform, Input.mousePosition, m_Camera, out worldPointInRectangle);

                    switch (linkInfo.GetLinkID())
                    {
                        case "id_01": // 100041637: // id_01
                                      //m_TextPopup_RectTransform.position = worldPointInRectangle;
                                      //m_TextPopup_RectTransform.gameObject.SetActive(true);
                                      //m_TextPopup_TMPComponent.text = k_LinkText + " ID 01";
                            break;
                        case "id_02": // 100041638: // id_02
                                      //m_TextPopup_RectTransform.position = worldPointInRectangle;
                                      //m_TextPopup_RectTransform.gameObject.SetActive(true);
                                      //m_TextPopup_TMPComponent.text = k_LinkText + " ID 02";
                            break;
                    }
                }
                #endregion


                #region Example of Word Selection
                // Check if Mouse intersects any words and if so assign a random color to that word.
#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextMeshPro, Input.mousePosition, Camera.main);
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                if (wordIndex != -1 && wordIndex != m_lastWordIndex)
                {
                    m_lastWordIndex = wordIndex;

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_WordInfo" (возможно, отсутствует директива using или ссылка на сборку).
                    TMP_WordInfo wInfo = m_TextMeshPro.textInfo.wordInfo[wordIndex];
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_WordInfo" (возможно, отсутствует директива using или ссылка на сборку).

                    Vector3 wordPOS = m_TextMeshPro.transform.TransformPoint(m_TextMeshPro.textInfo.characterInfo[wInfo.firstCharacterIndex].bottomLeft);
                    wordPOS = Camera.main.WorldToScreenPoint(wordPOS);

                    //Debug.Log("Mouse Position: " + Input.mousePosition.ToString("f3") + "  Word Position: " + wordPOS.ToString("f3"));

                    Color32[] vertexColors = m_TextMeshPro.textInfo.meshInfo[0].colors32;

                    Color32 c = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                    for (int i = 0; i < wInfo.characterCount; i++)
                    {
                        int vertexIndex = m_TextMeshPro.textInfo.characterInfo[wInfo.firstCharacterIndex + i].vertexIndex;

                        vertexColors[vertexIndex + 0] = c;
                        vertexColors[vertexIndex + 1] = c;
                        vertexColors[vertexIndex + 2] = c;
                        vertexColors[vertexIndex + 3] = c;
                    }

                    m_TextMeshPro.mesh.colors32 = vertexColors;
                }
                #endregion
            }
        }


#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "PointerEventData" (возможно, отсутствует директива using или ссылка на сборку).
        public void OnPointerEnter(PointerEventData eventData)
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "PointerEventData" (возможно, отсутствует директива using или ссылка на сборку).
        {
            Debug.Log("OnPointerEnter()");
            m_isHoveringObject = true;
        }


#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "PointerEventData" (возможно, отсутствует директива using или ссылка на сборку).
        public void OnPointerExit(PointerEventData eventData)
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "PointerEventData" (возможно, отсутствует директива using или ссылка на сборку).
        {
            Debug.Log("OnPointerExit()");
            m_isHoveringObject = false;
        }

    }
}
