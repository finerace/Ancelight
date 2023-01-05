using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitysCircle : MonoBehaviour
{
    [SerializeField] private PlayerWeaponsManager weaponsManager;
    [SerializeField] private Image mainImage;
    [SerializeField] private Sprite nullIcon;
    [SerializeField] private Image mainCircle;
    [SerializeField] private Image dynamicCircle;
    [SerializeField] private Image effect;
    [SerializeField] private Image icon;
    private Material mainCircleMaterial;
    private Material dynamicCircleMaterial;
    private Material effectMaterial;
    private Material iconMaterial;
    [SerializeField] private Dictionary<int, float> abilitysTimers = new Dictionary<int, float>();
    [SerializeField] private Color inDelayColor;
    private Color startColor;
    private Color nowColor;


    private void Start()
    {
        weaponsManager = FindObjectOfType<PlayerWeaponsManager>();
        
        mainCircleMaterial = new Material(mainCircle.material);
        mainCircle.material = mainCircleMaterial;

        dynamicCircleMaterial = new Material(dynamicCircle.material);
        dynamicCircle.material = dynamicCircleMaterial;

        effectMaterial = new Material(effect.material);
        effect.material = effectMaterial;

        iconMaterial = new Material(icon.material);
        icon.material = iconMaterial;

        startColor = dynamicCircleMaterial.GetColor("_MainColor");

        foreach (var item in weaponsManager.AbilityDatas)
        {
            abilitysTimers.Add(item.Id, 0);
        }

        weaponsManager.extraAbilityUseEvent.AddListener(
            () => { StartTimer(weaponsManager.SelectedAbilityData.DelayTime); } );

    }

    private void Update()
    {
        ExtraAbilityData selectedAbilityData = weaponsManager.SelectedAbilityData;
        ExtraAbility selectedAbility = weaponsManager.SelectedAbility;

        float timeStep = Time.deltaTime * 15f;

        float incompleteÑircleSmoothness = 0.9f;
        float fillAmount = (1f - abilitysTimers[selectedAbilityData.Id] / selectedAbilityData.DelayTime)
            * incompleteÑircleSmoothness;

        dynamicCircle.fillAmount = fillAmount;

        foreach (var item in weaponsManager.AbilityDatas)
        {
            if(abilitysTimers[item.Id] > 0)
                abilitysTimers[item.Id] -= Time.deltaTime;
        }

        if(selectedAbility.isAttack)
            nowColor = Color.Lerp(nowColor, inDelayColor, timeStep);
        else
            nowColor = Color.Lerp(nowColor, startColor, timeStep);

        SetCircleColors(nowColor);

        if (!weaponsManager.AbilityDelayTest(selectedAbilityData.Id))
            icon.color = Color.Lerp(icon.color, new Color(1, 1, 1, 0.1f),timeStep*2f);
        else
            icon.color = Color.Lerp(icon.color, new Color(1, 1, 1, 1f), timeStep*2f);

        if (mainImage != weaponsManager.SelectedAbilityData.Icon &&
            weaponsManager.SelectedAbilityData.Icon != null)
            mainImage.sprite = weaponsManager.SelectedAbilityData.Icon;

        else if (weaponsManager.SelectedAbilityData.Icon == null 
            && mainImage.sprite != nullIcon)
            mainImage.sprite = nullIcon;

    }

    void StartTimer(float time)
    {
        ExtraAbilityData selectedAbilityData = weaponsManager.SelectedAbilityData;

        abilitysTimers[selectedAbilityData.Id] = time;
    }

    private void SetCircleColors(Color color)
    {
        string name = "_MainColor";
        mainCircleMaterial.SetColor(name, color);
        dynamicCircleMaterial.SetColor(name,color);
        effectMaterial.SetColor(name,color);
        effectMaterial.SetFloat("_Brightness", 0.2f);
        iconMaterial.SetColor(name, color);
    }

}
