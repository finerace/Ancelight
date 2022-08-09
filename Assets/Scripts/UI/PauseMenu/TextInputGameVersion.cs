using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInputGameVersion : TextInputer
{

    public override void UpdateText()
    {
        textForInput.text = Application.version;
        onStartUpdateOnly = true;
    }

}
