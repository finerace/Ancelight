using UnityEngine;
using UnityEngine.Events;
#pragma warning disable CS0234 // Тип или имя пространства имен "EventSystems" не существует в пространстве имен "UnityEngine" (возможно, отсутствует ссылка на сборку).
using UnityEngine.EventSystems;
#pragma warning restore CS0234 // Тип или имя пространства имен "EventSystems" не существует в пространстве имен "UnityEngine" (возможно, отсутствует ссылка на сборку).
using System;


namespace TMPro
{

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "IPointerEnterHandler" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "IPointerExitHandler" (возможно, отсутствует директива using или ссылка на сборку).
    public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "IPointerExitHandler" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "IPointerEnterHandler" (возможно, отсутствует директива using или ссылка на сборку).
    {
        [Serializable]
        public class CharacterSelectionEvent : UnityEvent<char, int> { }

        [Serializable]
        public class SpriteSelectionEvent : UnityEvent<char, int> { }

        [Serializable]
        public class WordSelectionEvent : UnityEvent<string, int, int> { }

        [Serializable]
        public class LineSelectionEvent : UnityEvent<string, int, int> { }

        [Serializable]
        public class LinkSelectionEvent : UnityEvent<string, string, int> { }


        /// <summary>
        /// Event delegate triggered when pointer is over a character.
        /// </summary>
        public CharacterSelectionEvent onCharacterSelection
        {
            get { return m_OnCharacterSelection; }
            set { m_OnCharacterSelection = value; }
        }
        [SerializeField]
        private CharacterSelectionEvent m_OnCharacterSelection = new CharacterSelectionEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a sprite.
        /// </summary>
        public SpriteSelectionEvent onSpriteSelection
        {
            get { return m_OnSpriteSelection; }
            set { m_OnSpriteSelection = value; }
        }
        [SerializeField]
        private SpriteSelectionEvent m_OnSpriteSelection = new SpriteSelectionEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a word.
        /// </summary>
        public WordSelectionEvent onWordSelection
        {
            get { return m_OnWordSelection; }
            set { m_OnWordSelection = value; }
        }
        [SerializeField]
        private WordSelectionEvent m_OnWordSelection = new WordSelectionEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a line.
        /// </summary>
        public LineSelectionEvent onLineSelection
        {
            get { return m_OnLineSelection; }
            set { m_OnLineSelection = value; }
        }
        [SerializeField]
        private LineSelectionEvent m_OnLineSelection = new LineSelectionEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a link.
        /// </summary>
        public LinkSelectionEvent onLinkSelection
        {
            get { return m_OnLinkSelection; }
            set { m_OnLinkSelection = value; }
        }
        [SerializeField]
        private LinkSelectionEvent m_OnLinkSelection = new LinkSelectionEvent();



#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
        private TMP_Text m_TextComponent;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).

        private Camera m_Camera;
        private Canvas m_Canvas;

        private int m_selectedLink = -1;
        private int m_lastCharIndex = -1;
        private int m_lastWordIndex = -1;
        private int m_lastLineIndex = -1;

        void Awake()
        {
            // Get a reference to the text component.
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).

            // Get a reference to the camera rendering the text taking into consideration the text component type.
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
            if (m_TextComponent.GetType() == typeof(TextMeshProUGUI))
            {
                m_Canvas = gameObject.GetComponentInParent<Canvas>();
                if (m_Canvas != null)
                {
                    if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                        m_Camera = null;
                    else
                        m_Camera = m_Canvas.worldCamera;
                }
            }
            else
            {
                m_Camera = Camera.main;
            }
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
        }


        void LateUpdate()
        {
#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
            if (TMP_TextUtilities.IsIntersectingRectTransform(m_TextComponent.rectTransform, Input.mousePosition, m_Camera))
            {
                #region Example of Character or Sprite Selection
#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                int charIndex = TMP_TextUtilities.FindIntersectingCharacter(m_TextComponent, Input.mousePosition, m_Camera, true);
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                if (charIndex != -1 && charIndex != m_lastCharIndex)
                {
                    m_lastCharIndex = charIndex;

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_TextElementType" (возможно, отсутствует директива using или ссылка на сборку).
                    TMP_TextElementType elementType = m_TextComponent.textInfo.characterInfo[charIndex].elementType;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_TextElementType" (возможно, отсутствует директива using или ссылка на сборку).

                    // Send event to any event listeners depending on whether it is a character or sprite.
#pragma warning disable CS0103 // Имя "TMP_TextElementType" не существует в текущем контексте.
#pragma warning disable CS0103 // Имя "TMP_TextElementType" не существует в текущем контексте.
                    if (elementType == TMP_TextElementType.Character)
                        SendOnCharacterSelection(m_TextComponent.textInfo.characterInfo[charIndex].character, charIndex);
                    else if (elementType == TMP_TextElementType.Sprite)
                        SendOnSpriteSelection(m_TextComponent.textInfo.characterInfo[charIndex].character, charIndex);
#pragma warning restore CS0103 // Имя "TMP_TextElementType" не существует в текущем контексте.
#pragma warning restore CS0103 // Имя "TMP_TextElementType" не существует в текущем контексте.
                }
                #endregion


                #region Example of Word Selection
                // Check if Mouse intersects any words and if so assign a random color to that word.
#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextComponent, Input.mousePosition, m_Camera);
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                if (wordIndex != -1 && wordIndex != m_lastWordIndex)
                {
                    m_lastWordIndex = wordIndex;

                    // Get the information about the selected word.
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_WordInfo" (возможно, отсутствует директива using или ссылка на сборку).
                    TMP_WordInfo wInfo = m_TextComponent.textInfo.wordInfo[wordIndex];
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_WordInfo" (возможно, отсутствует директива using или ссылка на сборку).

                    // Send the event to any listeners.
                    SendOnWordSelection(wInfo.GetWord(), wInfo.firstCharacterIndex, wInfo.characterCount);
                }
                #endregion


                #region Example of Line Selection
                // Check if Mouse intersects any words and if so assign a random color to that word.
#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                int lineIndex = TMP_TextUtilities.FindIntersectingLine(m_TextComponent, Input.mousePosition, m_Camera);
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                if (lineIndex != -1 && lineIndex != m_lastLineIndex)
                {
                    m_lastLineIndex = lineIndex;

                    // Get the information about the selected word.
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_LineInfo" (возможно, отсутствует директива using или ссылка на сборку).
                    TMP_LineInfo lineInfo = m_TextComponent.textInfo.lineInfo[lineIndex];
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_LineInfo" (возможно, отсутствует директива using или ссылка на сборку).

                    // Send the event to any listeners.
                    char[] buffer = new char[lineInfo.characterCount];
                    for (int i = 0; i < lineInfo.characterCount && i < m_TextComponent.textInfo.characterInfo.Length; i++)
                    {
                        buffer[i] = m_TextComponent.textInfo.characterInfo[i + lineInfo.firstCharacterIndex].character;
                    }

                    string lineText = new string(buffer);
                    SendOnLineSelection(lineText, lineInfo.firstCharacterIndex, lineInfo.characterCount);
                }
                #endregion


                #region Example of Link Handling
                // Check if mouse intersects with any links.
#pragma warning disable CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextComponent, Input.mousePosition, m_Camera);
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.

                // Handle new Link selection.
                if (linkIndex != -1 && linkIndex != m_selectedLink)
                {
                    m_selectedLink = linkIndex;

                    // Get information about the link.
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_LinkInfo" (возможно, отсутствует директива using или ссылка на сборку).
                    TMP_LinkInfo linkInfo = m_TextComponent.textInfo.linkInfo[linkIndex];
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_LinkInfo" (возможно, отсутствует директива using или ссылка на сборку).

                    // Send the event to any listeners. 
                    SendOnLinkSelection(linkInfo.GetLinkID(), linkInfo.GetLinkText(), linkIndex);
                }
                #endregion
            }
#pragma warning restore CS0103 // Имя "TMP_TextUtilities" не существует в текущем контексте.
        }


#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "PointerEventData" (возможно, отсутствует директива using или ссылка на сборку).
        public void OnPointerEnter(PointerEventData eventData)
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "PointerEventData" (возможно, отсутствует директива using или ссылка на сборку).
        {
            //Debug.Log("OnPointerEnter()");
        }


#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "PointerEventData" (возможно, отсутствует директива using или ссылка на сборку).
        public void OnPointerExit(PointerEventData eventData)
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "PointerEventData" (возможно, отсутствует директива using или ссылка на сборку).
        {
            //Debug.Log("OnPointerExit()");
        }


        private void SendOnCharacterSelection(char character, int characterIndex)
        {
            if (onCharacterSelection != null)
                onCharacterSelection.Invoke(character, characterIndex);
        }

        private void SendOnSpriteSelection(char character, int characterIndex)
        {
            if (onSpriteSelection != null)
                onSpriteSelection.Invoke(character, characterIndex);
        }

        private void SendOnWordSelection(string word, int charIndex, int length)
        {
            if (onWordSelection != null)
                onWordSelection.Invoke(word, charIndex, length);
        }

        private void SendOnLineSelection(string line, int charIndex, int length)
        {
            if (onLineSelection != null)
                onLineSelection.Invoke(line, charIndex, length);
        }

        private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
        {
            if (onLinkSelection != null)
                onLinkSelection.Invoke(linkID, linkText, linkIndex);
        }

    }
}
