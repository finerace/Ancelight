using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{

    public class Benchmark02 : MonoBehaviour
    {

        public int SpawnType = 0;
        public int NumberOfNPC = 12;

        private TextMeshProFloatingText floatingText_Script;


        void Start()
        {

            for (int i = 0; i < NumberOfNPC; i++)
            {


                if (SpawnType == 0)
                {
                    // TextMesh Pro Implementation
                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));

#pragma warning disable CS0246 // Ќе удалось найти тип или им€ пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Ќе удалось найти тип или им€ пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
                    TextMeshPro textMeshPro = go.AddComponent<TextMeshPro>();
#pragma warning restore CS0246 // Ќе удалось найти тип или им€ пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning restore CS0246 // Ќе удалось найти тип или им€ пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).

                    textMeshPro.autoSizeTextContainer = true;
                    textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0);

#pragma warning disable CS0103 // »м€ "TextAlignmentOptions" не существует в текущем контексте.
                    textMeshPro.alignment = TextAlignmentOptions.Bottom;
#pragma warning restore CS0103 // »м€ "TextAlignmentOptions" не существует в текущем контексте.
                    textMeshPro.fontSize = 96;
                    textMeshPro.enableKerning = false;

                    textMeshPro.color = new Color32(255, 255, 0, 255);
                    textMeshPro.text = "!";
                    textMeshPro.isTextObjectScaleStatic = true;

                    // Spawn Floating Text
                    floatingText_Script = go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 0;
                }
                else if (SpawnType == 1)
                {
                    // TextMesh Implementation
                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));

                    TextMesh textMesh = go.AddComponent<TextMesh>();
                    textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
                    textMesh.GetComponent<Renderer>().sharedMaterial = textMesh.font.material;

                    textMesh.anchor = TextAnchor.LowerCenter;
                    textMesh.fontSize = 96;

                    textMesh.color = new Color32(255, 255, 0, 255);
                    textMesh.text = "!";

                    // Spawn Floating Text
                    floatingText_Script = go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 1;
                }
                else if (SpawnType == 2)
                {
                    // Canvas WorldSpace Camera
                    GameObject go = new GameObject();
                    Canvas canvas = go.AddComponent<Canvas>();
                    canvas.worldCamera = Camera.main;

                    go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    go.transform.position = new Vector3(Random.Range(-95f, 95f), 5f, Random.Range(-95f, 95f));

#pragma warning disable CS0246 // Ќе удалось найти тип или им€ пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Ќе удалось найти тип или им€ пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
                    TextMeshProUGUI textObject = new GameObject().AddComponent<TextMeshProUGUI>();
#pragma warning restore CS0246 // Ќе удалось найти тип или им€ пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning restore CS0246 // Ќе удалось найти тип или им€ пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
                    textObject.rectTransform.SetParent(go.transform, false);

                    textObject.color = new Color32(255, 255, 0, 255);
#pragma warning disable CS0103 // »м€ "TextAlignmentOptions" не существует в текущем контексте.
                    textObject.alignment = TextAlignmentOptions.Bottom;
#pragma warning restore CS0103 // »м€ "TextAlignmentOptions" не существует в текущем контексте.
                    textObject.fontSize = 96;
                    textObject.text = "!";

                    // Spawn Floating Text
                    floatingText_Script = go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 0;
                }



            }
        }
    }
}
