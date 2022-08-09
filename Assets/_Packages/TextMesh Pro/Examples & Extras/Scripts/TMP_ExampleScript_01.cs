using UnityEngine;
#pragma warning disable CS0234 // Тип или имя пространства имен "UI" не существует в пространстве имен "UnityEngine" (возможно, отсутствует ссылка на сборку).
using UnityEngine.UI;
#pragma warning restore CS0234 // Тип или имя пространства имен "UI" не существует в пространстве имен "UnityEngine" (возможно, отсутствует ссылка на сборку).
using System.Collections;
using TMPro;


namespace TMPro.Examples
{

    public class TMP_ExampleScript_01 : MonoBehaviour
    {
        public enum objectType { TextMeshPro = 0, TextMeshProUGUI = 1 };

        public objectType ObjectType;
        public bool isStatic;

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
        private TMP_Text m_text;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).

        //private TMP_InputField m_inputfield;


        private const string k_label = "The count is <#0080ff>{0}</color>";
        private int count;

        void Awake()
        {
            // Get a reference to the TMP text component if one already exists otherwise add one.
            // This example show the convenience of having both TMP components derive from TMP_Text. 
            if (ObjectType == 0)
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
                m_text = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
            else
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
                m_text = GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).

            // Load a new font asset and assign it to the text object.
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_FontAsset" (возможно, отсутствует директива using или ссылка на сборку).
            m_text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_FontAsset" (возможно, отсутствует директива using или ссылка на сборку).

            // Load a new material preset which was created with the context menu duplicate.
            m_text.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");

            // Set the size of the font.
            m_text.fontSize = 120;

            // Set the text
            m_text.text = "A <#0080ff>simple</color> line of text.";

            // Get the preferred width and height based on the supplied width and height as opposed to the actual size of the current text container.
            Vector2 size = m_text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);

            // Set the size of the RectTransform based on the new calculated values.
            m_text.rectTransform.sizeDelta = new Vector2(size.x, size.y);
        }


        void Update()
        {
            if (!isStatic)
            {
                m_text.SetText(k_label, count % 1000);
                count += 1;
            }
        }

    }
}
