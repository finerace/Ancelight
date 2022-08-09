using TMPro;
using UnityEngine;

public class DropdownSample: MonoBehaviour
{
	[SerializeField]
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).
	private TextMeshProUGUI text = null;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TextMeshProUGUI" (возможно, отсутствует директива using или ссылка на сборку).

	[SerializeField]
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Dropdown" (возможно, отсутствует директива using или ссылка на сборку).
	private TMP_Dropdown dropdownWithoutPlaceholder = null;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Dropdown" (возможно, отсутствует директива using или ссылка на сборку).

	[SerializeField]
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_Dropdown" (возможно, отсутствует директива using или ссылка на сборку).
	private TMP_Dropdown dropdownWithPlaceholder = null;
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_Dropdown" (возможно, отсутствует директива using или ссылка на сборку).

	public void OnButtonClick()
	{
		text.text = dropdownWithPlaceholder.value > -1 ? "Selected values:\n" + dropdownWithoutPlaceholder.value + " - " + dropdownWithPlaceholder.value : "Error: Please make a selection";
	}
}
