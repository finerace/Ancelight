using UnityEngine;
#pragma warning disable CS0234 // Тип или имя пространства имен "UI" не существует в пространстве имен "UnityEngine" (возможно, отсутствует ссылка на сборку).
using UnityEngine.UI;
#pragma warning restore CS0234 // Тип или имя пространства имен "UI" не существует в пространстве имен "UnityEngine" (возможно, отсутствует ссылка на сборку).
using System.Collections;
using TMPro;

public class ChatController : MonoBehaviour {


#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_InputField" (возможно, отсутствует директива using или ссылка на сборку).
    public TMP_InputField TMP_ChatInput;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_InputField" (возможно, отсутствует директива using или ссылка на сборку).

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).
    public TMP_Text TMP_ChatOutput;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Text" (возможно, отсутствует директива using или ссылка на сборку).

#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "Scrollbar" (возможно, отсутствует директива using или ссылка на сборку).
    public Scrollbar ChatScrollbar;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "Scrollbar" (возможно, отсутствует директива using или ссылка на сборку).

    void OnEnable()
    {
        TMP_ChatInput.onSubmit.AddListener(AddToChatOutput);

    }

    void OnDisable()
    {
        TMP_ChatInput.onSubmit.RemoveListener(AddToChatOutput);

    }


    void AddToChatOutput(string newText)
    {
        // Clear Input Field
        TMP_ChatInput.text = string.Empty;

        var timeNow = System.DateTime.Now;

        TMP_ChatOutput.text += "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText + "\n";

        TMP_ChatInput.ActivateInputField();

        // Set the scrollbar to the bottom when next text is submitted.
        ChatScrollbar.value = 0;

    }

}
