﻿using UnityEngine;
using System.Collections;
using UnityEngine.TextCore.LowLevel;


namespace TMPro.Examples
{

    public class Benchmark03 : MonoBehaviour
    {
        public enum BenchmarkType { TMP_SDF_MOBILE = 0, TMP_SDF__MOBILE_SSD = 1, TMP_SDF = 2, TMP_BITMAP_MOBILE = 3, TEXTMESH_BITMAP = 4 }

        public int NumberOfSamples = 100;
        public BenchmarkType Benchmark;

        public Font SourceFontFile;


        void Awake()
        {

        }


        void Start()
        {
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_FontAsset" (возможно, отсутствует директива using или ссылка на сборку).
            TMP_FontAsset fontAsset = null;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_FontAsset" (возможно, отсутствует директива using или ссылка на сборку).

            // Create Dynamic Font Asset for the given font file.
            switch (Benchmark)
            {
                case BenchmarkType.TMP_SDF_MOBILE:
#pragma warning disable CS0103 // Имя "TMP_FontAsset" не существует в текущем контексте.
#pragma warning disable CS0103 // Имя "AtlasPopulationMode" не существует в текущем контексте.
                    fontAsset = TMP_FontAsset.CreateFontAsset(SourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
#pragma warning restore CS0103 // Имя "AtlasPopulationMode" не существует в текущем контексте.
#pragma warning restore CS0103 // Имя "TMP_FontAsset" не существует в текущем контексте.
                    break;
                case BenchmarkType.TMP_SDF__MOBILE_SSD:
#pragma warning disable CS0103 // Имя "AtlasPopulationMode" не существует в текущем контексте.
#pragma warning disable CS0103 // Имя "TMP_FontAsset" не существует в текущем контексте.
                    fontAsset = TMP_FontAsset.CreateFontAsset(SourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
#pragma warning restore CS0103 // Имя "TMP_FontAsset" не существует в текущем контексте.
#pragma warning restore CS0103 // Имя "AtlasPopulationMode" не существует в текущем контексте.
                    fontAsset.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field SSD");
                    break;
                case BenchmarkType.TMP_SDF:
#pragma warning disable CS0103 // Имя "AtlasPopulationMode" не существует в текущем контексте.
#pragma warning disable CS0103 // Имя "TMP_FontAsset" не существует в текущем контексте.
                    fontAsset = TMP_FontAsset.CreateFontAsset(SourceFontFile, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
#pragma warning restore CS0103 // Имя "TMP_FontAsset" не существует в текущем контексте.
#pragma warning restore CS0103 // Имя "AtlasPopulationMode" не существует в текущем контексте.
                    fontAsset.material.shader = Shader.Find("TextMeshPro/Distance Field");
                    break;
                case BenchmarkType.TMP_BITMAP_MOBILE:
#pragma warning disable CS0103 // Имя "TMP_FontAsset" не существует в текущем контексте.
#pragma warning disable CS0103 // Имя "AtlasPopulationMode" не существует в текущем контексте.
                    fontAsset = TMP_FontAsset.CreateFontAsset(SourceFontFile, 90, 9, GlyphRenderMode.SMOOTH, 256, 256, AtlasPopulationMode.Dynamic);
#pragma warning restore CS0103 // Имя "AtlasPopulationMode" не существует в текущем контексте.
#pragma warning restore CS0103 // Имя "TMP_FontAsset" не существует в текущем контексте.
                    break;
            }

            for (int i = 0; i < NumberOfSamples; i++)
            {
                switch (Benchmark)
                {
                    case BenchmarkType.TMP_SDF_MOBILE:
                    case BenchmarkType.TMP_SDF__MOBILE_SSD:
                    case BenchmarkType.TMP_SDF:
                    case BenchmarkType.TMP_BITMAP_MOBILE:
                        {
                            GameObject go = new GameObject();
                            go.transform.position = new Vector3(0, 1.2f, 0);

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
                            TextMeshPro textComponent = go.AddComponent<TextMeshPro>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
                            textComponent.font = fontAsset;
                            textComponent.fontSize = 128;
                            textComponent.text = "@";
#pragma warning disable CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                            textComponent.alignment = TextAlignmentOptions.Center;
#pragma warning restore CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                            textComponent.color = new Color32(255, 255, 0, 255);

                            if (Benchmark == BenchmarkType.TMP_BITMAP_MOBILE)
                                textComponent.fontSize = 132;

                        }
                        break;
                    case BenchmarkType.TEXTMESH_BITMAP:
                        {
                            GameObject go = new GameObject();
                            go.transform.position = new Vector3(0, 1.2f, 0);

                            TextMesh textMesh = go.AddComponent<TextMesh>();
                            textMesh.GetComponent<Renderer>().sharedMaterial = SourceFontFile.material;
                            textMesh.font = SourceFontFile;
                            textMesh.anchor = TextAnchor.MiddleCenter;
                            textMesh.fontSize = 130;

                            textMesh.color = new Color32(255, 255, 0, 255);
                            textMesh.text = "@";
                        }
                        break;
                }
            }
        }

    }
}
