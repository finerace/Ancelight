using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class TMP_FrameRateCounter : MonoBehaviour
    {
        public float UpdateInterval = 5.0f;
        private float m_LastInterval = 0;
        private int m_Frames = 0;

        public enum FpsCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };

        public FpsCounterAnchorPositions AnchorPosition = FpsCounterAnchorPositions.TopRight;

        private string htmlColorTag;
        private const string fpsLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
        private TextMeshPro m_TextMeshPro;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
        private Transform m_frameCounter_transform;
        private Camera m_camera;

        private FpsCounterAnchorPositions last_AnchorPosition;

        void Awake()
        {
            if (!enabled)
                return;

            m_camera = Camera.main;
            Application.targetFrameRate = 9999;

            GameObject frameCounter = new GameObject("Frame Counter");

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextMeshPro = frameCounter.AddComponent<TextMeshPro>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_FontAsset" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_FontAsset" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");


            m_frameCounter_transform = frameCounter.transform;
            m_frameCounter_transform.SetParent(m_camera.transform);
            m_frameCounter_transform.localRotation = Quaternion.identity;

            m_TextMeshPro.enableWordWrapping = false;
            m_TextMeshPro.fontSize = 24;
            //m_TextMeshPro.FontColor = new Color32(255, 255, 255, 128);
            //m_TextMeshPro.edgeWidth = .15f;
            //m_TextMeshPro.isOverlay = true;

            //m_TextMeshPro.FaceColor = new Color32(255, 128, 0, 0);
            //m_TextMeshPro.EdgeColor = new Color32(0, 255, 0, 255);
            //m_TextMeshPro.FontMaterial.renderQueue = 4000;

            //m_TextMeshPro.CreateSoftShadowClone(new Vector2(1f, -1f));

            Set_FrameCounter_Position(AnchorPosition);
            last_AnchorPosition = AnchorPosition;


        }

        void Start()
        {
            m_LastInterval = Time.realtimeSinceStartup;
            m_Frames = 0;
        }

        void Update()
        {
            if (AnchorPosition != last_AnchorPosition)
                Set_FrameCounter_Position(AnchorPosition);

            last_AnchorPosition = AnchorPosition;

            m_Frames += 1;
            float timeNow = Time.realtimeSinceStartup;

            if (timeNow > m_LastInterval + UpdateInterval)
            {
                // display two fractional digits (f2 format)
                float fps = m_Frames / (timeNow - m_LastInterval);
                float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

                if (fps < 30)
                    htmlColorTag = "<color=yellow>";
                else if (fps < 10)
                    htmlColorTag = "<color=red>";
                else
                    htmlColorTag = "<color=green>";

                //string format = System.String.Format(htmlColorTag + "{0:F2} </color>FPS \n{1:F2} <#8080ff>MS",fps, ms);
                //m_TextMeshPro.text = format;

                m_TextMeshPro.SetText(htmlColorTag + fpsLabel, fps, ms);

                m_Frames = 0;
                m_LastInterval = timeNow;
            }
        }


        void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
        {
            //Debug.Log("Changing frame counter anchor position.");
            m_TextMeshPro.margin = new Vector4(1f, 1f, 1f, 1f);

            switch (anchor_position)
            {
                case FpsCounterAnchorPositions.TopLeft:
#pragma warning disable CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                    m_TextMeshPro.alignment = TextAlignmentOptions.TopLeft;
#pragma warning restore CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                    m_TextMeshPro.rectTransform.pivot = new Vector2(0, 1);
                    m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(0, 1, 100.0f));
                    break;
                case FpsCounterAnchorPositions.BottomLeft:
#pragma warning disable CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                    m_TextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
#pragma warning restore CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                    m_TextMeshPro.rectTransform.pivot = new Vector2(0, 0);
                    m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(0, 0, 100.0f));
                    break;
                case FpsCounterAnchorPositions.TopRight:
#pragma warning disable CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                    m_TextMeshPro.alignment = TextAlignmentOptions.TopRight;
#pragma warning restore CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                    m_TextMeshPro.rectTransform.pivot = new Vector2(1, 1);
                    m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(1, 1, 100.0f));
                    break;
                case FpsCounterAnchorPositions.BottomRight:
#pragma warning disable CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                    m_TextMeshPro.alignment = TextAlignmentOptions.BottomRight;
#pragma warning restore CS0103 // Имя "TextAlignmentOptions" не существует в текущем контексте.
                    m_TextMeshPro.rectTransform.pivot = new Vector2(1, 0);
                    m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 100.0f));
                    break;
            }
        }
    }
}
