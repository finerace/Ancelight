using UnityEngine;
using System;


namespace TMPro
{
    /// <summary>
    /// EXample of a Custom Character Input Validator to only allow digits from 0 to 9.
    /// </summary>
    [Serializable]
    //[CreateAssetMenu(fileName = "InputValidator - Digits.asset", menuName = "TextMeshPro/Input Validators/Digits", order = 100)]
#pragma warning disable CS0246 // Не удалось найти тип или имя пространства имен "TMP_InputValidator" (возможно, отсутствует директива using или ссылка на сборку).
    public class TMP_DigitValidator : TMP_InputValidator
#pragma warning restore CS0246 // Не удалось найти тип или имя пространства имен "TMP_InputValidator" (возможно, отсутствует директива using или ссылка на сборку).
    {
        // Custom text input validation function
#pragma warning disable CS0115 // "TMP_DigitValidator.Validate(ref string, ref int, char)": не найден метод, пригодный для переопределения.
        public override char Validate(ref string text, ref int pos, char ch)
#pragma warning restore CS0115 // "TMP_DigitValidator.Validate(ref string, ref int, char)": не найден метод, пригодный для переопределения.
        {
            if (ch >= '0' && ch <= '9')
            {
                text += ch;
                pos += 1;
                return ch;
            }

            return (char)0;
        }
    }
}
