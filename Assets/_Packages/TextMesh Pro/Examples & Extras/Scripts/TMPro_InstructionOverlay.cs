using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class TMPro_InstructionOverlay : MonoBehaviour
    {

        public enum FpsCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };

        public FpsCounterAnchorPositions AnchorPosition = FpsCounterAnchorPositions.BottomLeft;

        private const string instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
        private TextMeshPro m_TextMeshPro;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextContainer" (возможно, отсутствует директива using или ссылка на сборку).
        private TextContainer m_textContainer;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextContainer" (возможно, отсутствует директива using или ссылка на сборку).
        private Transform m_frameCounter_transform;
        private Camera m_camera;

        //private FpsCounterAnchorPositions last_AnchorPosition;

        void Awake()
        {
            if (!enabled)
                return;

            m_camera = Camera.main;

            GameObject frameCounter = new GameObject("Frame Counter");
            m_frameCounter_transform = frameCounter.transform;
            m_frameCounter_transform.parent = m_camera.transform;
            m_frameCounter_transform.localRotation = Quaternion.identity;


#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextMeshPro = frameCounter.AddComponent<TextMeshPro>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshPro" (возможно, отсутствует директива using или ссылка на сборку).
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_FontAsset" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_FontAsset" (возможно, отсутствует директива using или ссылка на сборку).
            m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");

            m_TextMeshPro.fontSize = 30;

            m_TextMeshPro.isOverlay = true;
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextContainer" (возможно, отсутствует директива using или ссылка на сборку).
            m_textContainer = frameCounter.GetComponent<TextContainer>();
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextContainer" (возможно, отсутствует директива using или ссылка на сборку).

            Set_FrameCounter_Position(AnchorPosition);
            //last_AnchorPosition = AnchorPosition;

            m_TextMeshPro.text = instructions;

        }




        void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
        {

            switch (anchor_position)
            {
                case FpsCounterAnchorPositions.TopLeft:
                    //m_TextMeshPro.anchor = AnchorPositions.TopLeft;
#pragma warning disable CS0103 // Имя "TextContainerAnchors" не существует в текущем контексте.
                    m_textContainer.anchorPosition = TextContainerAnchors.TopLeft;
#pragma warning restore CS0103 // Имя "TextContainerAnchors" не существует в текущем контексте.
                    m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(0, 1, 100.0f));
                    break;
                case FpsCounterAnchorPositions.BottomLeft:
                    //m_TextMeshPro.anchor = AnchorPositions.BottomLeft;
#pragma warning disable CS0103 // Имя "TextContainerAnchors" не существует в текущем контексте.
                    m_textContainer.anchorPosition = TextContainerAnchors.BottomLeft;
#pragma warning restore CS0103 // Имя "TextContainerAnchors" не существует в текущем контексте.
                    m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(0, 0, 100.0f));
                    break;
                case FpsCounterAnchorPositions.TopRight:
                    //m_TextMeshPro.anchor = AnchorPositions.TopRight;
#pragma warning disable CS0103 // Имя "TextContainerAnchors" не существует в текущем контексте.
                    m_textContainer.anchorPosition = TextContainerAnchors.TopRight;
#pragma warning restore CS0103 // Имя "TextContainerAnchors" не существует в текущем контексте.
                    m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(1, 1, 100.0f));
                    break;
                case FpsCounterAnchorPositions.BottomRight:
                    //m_TextMeshPro.anchor = AnchorPositions.BottomRight;
#pragma warning disable CS0103 // Имя "TextContainerAnchors" не существует в текущем контексте.
                    m_textContainer.anchorPosition = TextContainerAnchors.BottomRight;
#pragma warning restore CS0103 // Имя "TextContainerAnchors" не существует в текущем контексте.
                    m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 100.0f));
                    break;
            }
        }
    }
}
