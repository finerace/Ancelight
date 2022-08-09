using UnityEngine;
using System.Collections;
#pragma warning disable CS0234 // ��� ��� ��� ������������ ���� "UI" �� ���������� � ������������ ���� "UnityEngine" (��������, ����������� ������ �� ������).
using UnityEngine.UI;
#pragma warning restore CS0234 // ��� ��� ��� ������������ ���� "UI" �� ���������� � ������������ ���� "UnityEngine" (��������, ����������� ������ �� ������).


namespace TMPro.Examples
{
    
    public class Benchmark01_UGUI : MonoBehaviour
    {

        public int BenchmarkType = 0;

        public Canvas canvas;
#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_FontAsset" (��������, ����������� ��������� using ��� ������ �� ������).
        public TMP_FontAsset TMProFont;
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_FontAsset" (��������, ����������� ��������� using ��� ������ �� ������).
        public Font TextMeshFont;

#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TextMeshProUGUI" (��������, ����������� ��������� using ��� ������ �� ������).
        private TextMeshProUGUI m_textMeshPro;
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TextMeshProUGUI" (��������, ����������� ��������� using ��� ������ �� ������).
        //private TextContainer m_textContainer;
#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "Text" (��������, ����������� ��������� using ��� ������ �� ������).
        private Text m_textMesh;
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "Text" (��������, ����������� ��������� using ��� ������ �� ������).

        private const string label01 = "The <#0050FF>count is: </color>";
        private const string label02 = "The <color=#0050FF>count is: </color>";

        //private const string label01 = "TextMesh <#0050FF>Pro!</color>  The count is: {0}";
        //private const string label02 = "Text Mesh<color=#0050FF>        The count is: </color>";

        //private string m_string;
        //private int m_frame;

        private Material m_material01;
        private Material m_material02;



        IEnumerator Start()
        {



            if (BenchmarkType == 0) // TextMesh Pro Component
            {
#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TextMeshProUGUI" (��������, ����������� ��������� using ��� ������ �� ������).
                m_textMeshPro = gameObject.AddComponent<TextMeshProUGUI>();
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TextMeshProUGUI" (��������, ����������� ��������� using ��� ������ �� ������).
                //m_textContainer = GetComponent<TextContainer>();


                //m_textMeshPro.anchorDampening = true;

                if (TMProFont != null)
                    m_textMeshPro.font = TMProFont;

                //m_textMeshPro.font = Resources.Load("Fonts & Materials/Anton SDF", typeof(TextMeshProFont)) as TextMeshProFont; // Make sure the Anton SDF exists before calling this...           
                //m_textMeshPro.fontSharedMaterial = Resources.Load("Fonts & Materials/Anton SDF", typeof(Material)) as Material; // Same as above make sure this material exists.

                m_textMeshPro.fontSize = 48;
#pragma warning disable CS0103 // ��� "TextAlignmentOptions" �� ���������� � ������� ���������.
                m_textMeshPro.alignment = TextAlignmentOptions.Center;
#pragma warning restore CS0103 // ��� "TextAlignmentOptions" �� ���������� � ������� ���������.
                //m_textMeshPro.anchor = AnchorPositions.Center;
                m_textMeshPro.extraPadding = true;
                //m_textMeshPro.outlineWidth = 0.25f;
                //m_textMeshPro.fontSharedMaterial.SetFloat("_OutlineWidth", 0.2f);
                //m_textMeshPro.fontSharedMaterial.EnableKeyword("UNDERLAY_ON");
                //m_textMeshPro.lineJustification = LineJustificationTypes.Center;
                //m_textMeshPro.enableWordWrapping = true;    
                //m_textMeshPro.lineLength = 60;          
                //m_textMeshPro.characterSpacing = 0.2f;
                //m_textMeshPro.fontColor = new Color32(255, 255, 255, 255);

                m_material01 = m_textMeshPro.font.material;
                m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - BEVEL"); // Make sure the LiberationSans SDF exists before calling this...  


            }
            else if (BenchmarkType == 1) // TextMesh
            {
#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "Text" (��������, ����������� ��������� using ��� ������ �� ������).
                m_textMesh = gameObject.AddComponent<Text>();
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "Text" (��������, ����������� ��������� using ��� ������ �� ������).

                if (TextMeshFont != null)
                {
                    m_textMesh.font = TextMeshFont;
                    //m_textMesh.renderer.sharedMaterial = m_textMesh.font.material;
                }
                else
                {
                    //m_textMesh.font = Resources.Load("Fonts/ARIAL", typeof(Font)) as Font;
                    //m_textMesh.renderer.sharedMaterial = m_textMesh.font.material;
                }

                m_textMesh.fontSize = 48;
                m_textMesh.alignment = TextAnchor.MiddleCenter;

                //m_textMesh.color = new Color32(255, 255, 0, 255);    
            }



            for (int i = 0; i <= 1000000; i++)
            {
                if (BenchmarkType == 0)
                {
                    m_textMeshPro.text = label01 + (i % 1000);
                    if (i % 1000 == 999)
                        m_textMeshPro.fontSharedMaterial = m_textMeshPro.fontSharedMaterial == m_material01 ? m_textMeshPro.fontSharedMaterial = m_material02 : m_textMeshPro.fontSharedMaterial = m_material01;



                }
                else if (BenchmarkType == 1)
                    m_textMesh.text = label02 + (i % 1000).ToString();

                yield return null;
            }


            yield return null;
        }


        /*
        void Update()
        {
            if (BenchmarkType == 0)
            {
                m_textMeshPro.text = (m_frame % 1000).ToString();            
            }
            else if (BenchmarkType == 1)
            {
                m_textMesh.text = (m_frame % 1000).ToString();
            }

            m_frame += 1;
        }
        */
    }

}
