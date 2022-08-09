using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class TeleType : MonoBehaviour
    {


        //[Range(0, 100)]
        //public int RevealSpeed = 50;

        private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";
        private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";


#pragma warning disable CS0246 // Ќе удалось найти тип или им€ пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
        private TMP_Text m_textMeshPro;
#pragma warning restore CS0246 // Ќе удалось найти тип или им€ пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).


        void Awake()
        {
            // Get Reference to TextMeshPro Component
#pragma warning disable CS0246 // Ќе удалось найти тип или им€ пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
            m_textMeshPro = GetComponent<TMP_Text>();
#pragma warning restore CS0246 // Ќе удалось найти тип или им€ пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
            m_textMeshPro.text = label01;
            m_textMeshPro.enableWordWrapping = true;
#pragma warning disable CS0103 // »м€ "TextAlignmentOptions" не существует в текущем контексте.
            m_textMeshPro.alignment = TextAlignmentOptions.Top;
#pragma warning restore CS0103 // »м€ "TextAlignmentOptions" не существует в текущем контексте.



            //if (GetComponentInParent(typeof(Canvas)) as Canvas == null)
            //{
            //    GameObject canvas = new GameObject("Canvas", typeof(Canvas));
            //    gameObject.transform.SetParent(canvas.transform);
            //    canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            //    // Set RectTransform Size
            //    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 300);
            //    m_textMeshPro.fontSize = 48;
            //}


        }


        IEnumerator Start()
        {

            // Force and update of the mesh to get valid information.
            m_textMeshPro.ForceMeshUpdate();


            int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
            int counter = 0;
            int visibleCount = 0;

            while (true)
            {
                visibleCount = counter % (totalVisibleCharacters + 1);

                m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                // Once the last character has been revealed, wait 1.0 second and start over.
                if (visibleCount >= totalVisibleCharacters)
                {
                    yield return new WaitForSeconds(1.0f);
                    m_textMeshPro.text = label02;
                    yield return new WaitForSeconds(1.0f);
                    m_textMeshPro.text = label01;
                    yield return new WaitForSeconds(1.0f);
                }

                counter += 1;

                yield return new WaitForSeconds(0.05f);
            }

            //Debug.Log("Done revealing the text.");
        }

    }
}