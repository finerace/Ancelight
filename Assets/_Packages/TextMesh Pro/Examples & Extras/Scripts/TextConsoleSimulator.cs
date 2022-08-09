using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    public class TextConsoleSimulator : MonoBehaviour
    {
#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_Text" (��������, ����������� ��������� using ��� ������ �� ������).
        private TMP_Text m_TextComponent;
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_Text" (��������, ����������� ��������� using ��� ������ �� ������).
        private bool hasTextChanged;

        void Awake()
        {
#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_Text" (��������, ����������� ��������� using ��� ������ �� ������).
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_Text" (��������, ����������� ��������� using ��� ������ �� ������).
        }


        void Start()
        {
            StartCoroutine(RevealCharacters(m_TextComponent));
            //StartCoroutine(RevealWords(m_TextComponent));
        }


        void OnEnable()
        {
            // Subscribe to event fired when text object has been regenerated.
#pragma warning disable CS0103 // ��� "TMPro_EventManager" �� ���������� � ������� ���������.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
#pragma warning restore CS0103 // ��� "TMPro_EventManager" �� ���������� � ������� ���������.
        }

        void OnDisable()
        {
#pragma warning disable CS0103 // ��� "TMPro_EventManager" �� ���������� � ������� ���������.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
#pragma warning restore CS0103 // ��� "TMPro_EventManager" �� ���������� � ������� ���������.
        }


        // Event received when the text object has changed.
        void ON_TEXT_CHANGED(Object obj)
        {
            hasTextChanged = true;
        }


        /// <summary>
        /// Method revealing the text one character at a time.
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_Text" (��������, ����������� ��������� using ��� ������ �� ������).
        IEnumerator RevealCharacters(TMP_Text textComponent)
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_Text" (��������, ����������� ��������� using ��� ������ �� ������).
        {
            textComponent.ForceMeshUpdate();

#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_TextInfo" (��������, ����������� ��������� using ��� ������ �� ������).
            TMP_TextInfo textInfo = textComponent.textInfo;
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_TextInfo" (��������, ����������� ��������� using ��� ������ �� ������).

            int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
            int visibleCount = 0;

            while (true)
            {
                if (hasTextChanged)
                {
                    totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                    hasTextChanged = false; 
                }

                if (visibleCount > totalVisibleCharacters)
                {
                    yield return new WaitForSeconds(1.0f);
                    visibleCount = 0;
                }

                textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                visibleCount += 1;

                yield return null;
            }
        }


        /// <summary>
        /// Method revealing the text one word at a time.
        /// </summary>
        /// <returns></returns>
#pragma warning disable CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_Text" (��������, ����������� ��������� using ��� ������ �� ������).
        IEnumerator RevealWords(TMP_Text textComponent)
#pragma warning restore CS0246 // �� ������� ����� ��� ��� ��� ������������ ���� "TMP_Text" (��������, ����������� ��������� using ��� ������ �� ������).
        {
            textComponent.ForceMeshUpdate();

            int totalWordCount = textComponent.textInfo.wordCount;
            int totalVisibleCharacters = textComponent.textInfo.characterCount; // Get # of Visible Character in text object
            int counter = 0;
            int currentWord = 0;
            int visibleCount = 0;

            while (true)
            {
                currentWord = counter % (totalWordCount + 1);

                // Get last character index for the current word.
                if (currentWord == 0) // Display no words.
                    visibleCount = 0;
                else if (currentWord < totalWordCount) // Display all other words with the exception of the last one.
                    visibleCount = textComponent.textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
                else if (currentWord == totalWordCount) // Display last word and all remaining characters.
                    visibleCount = totalVisibleCharacters;

                textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                // Once the last character has been revealed, wait 1.0 second and start over.
                if (visibleCount >= totalVisibleCharacters)
                {
                    yield return new WaitForSeconds(1.0f);
                }

                counter += 1;

                yield return new WaitForSeconds(0.1f);
            }
        }

    }
}