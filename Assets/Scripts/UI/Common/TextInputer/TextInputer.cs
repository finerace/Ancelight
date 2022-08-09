using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public abstract class TextInputer : MonoBehaviour
{

    [SerializeField] protected TextMeshProUGUI textForInput;
    [SerializeField] protected bool onStartUpdateOnly = false;

    private void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        if (!onStartUpdateOnly)
            UpdateText();
    }

    public abstract void UpdateText();

}