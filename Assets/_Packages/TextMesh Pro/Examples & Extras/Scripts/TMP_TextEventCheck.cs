using UnityEngine;


namespace TMPro.Examples
{
    public class TMP_TextEventCheck : MonoBehaviour
    {

        public TMP_TextEventHandler TextEventHandler;

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
        private TMP_Text m_TextComponent;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).

        void OnEnable()
        {
            if (TextEventHandler != null)
            {
                // Get a reference to the text component
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
                m_TextComponent = TextEventHandler.GetComponent<TMP_Text>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
                
                TextEventHandler.onCharacterSelection.AddListener(OnCharacterSelection);
                TextEventHandler.onSpriteSelection.AddListener(OnSpriteSelection);
                TextEventHandler.onWordSelection.AddListener(OnWordSelection);
                TextEventHandler.onLineSelection.AddListener(OnLineSelection);
                TextEventHandler.onLinkSelection.AddListener(OnLinkSelection);
            }
        }


        void OnDisable()
        {
            if (TextEventHandler != null)
            {
                TextEventHandler.onCharacterSelection.RemoveListener(OnCharacterSelection);
                TextEventHandler.onSpriteSelection.RemoveListener(OnSpriteSelection);
                TextEventHandler.onWordSelection.RemoveListener(OnWordSelection);
                TextEventHandler.onLineSelection.RemoveListener(OnLineSelection);
                TextEventHandler.onLinkSelection.RemoveListener(OnLinkSelection);
            }
        }


        void OnCharacterSelection(char c, int index)
        {
            Debug.Log("Character [" + c + "] at Index: " + index + " has been selected.");
        }

        void OnSpriteSelection(char c, int index)
        {
            Debug.Log("Sprite [" + c + "] at Index: " + index + " has been selected.");
        }

        void OnWordSelection(string word, int firstCharacterIndex, int length)
        {
            Debug.Log("Word [" + word + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
        }

        void OnLineSelection(string lineText, int firstCharacterIndex, int length)
        {
            Debug.Log("Line [" + lineText + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
        }

        void OnLinkSelection(string linkID, string linkText, int linkIndex)
        {
            if (m_TextComponent != null)
            {
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_LinkInfo" (возможно, отсутствует директива using или ссылка на сборку).
                TMP_LinkInfo linkInfo = m_TextComponent.textInfo.linkInfo[linkIndex];
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_LinkInfo" (возможно, отсутствует директива using или ссылка на сборку).
            }
            
            Debug.Log("Link Index: " + linkIndex + " with ID [" + linkID + "] and Text \"" + linkText + "\" has been selected.");
        }

    }
}
