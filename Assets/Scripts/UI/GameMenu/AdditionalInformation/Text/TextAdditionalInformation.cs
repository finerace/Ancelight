using System;
using TMPro;
using UnityEngine;

public class TextAdditionalInformation : MonoBehaviour
{
    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private TextMeshProUGUI additionalInformationText;

    [Space] 
    
    [SerializeField] private float textVanishTime;
    [SerializeField] private float textVanishSpeed = 5f;
    
    private float textVanishTimer = 0;
    private bool timerIsEnd = true;
    
    
    private void Start()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
        
        playerMainService.AddHealthEvent += ShowAddHealthText;
        playerMainService.AddArmorEvent += ShowAddArmorText;
        playerMainService.AddPlasmaEvent += ShowAddPlasmaText;
        playerMainService.UnlockWeaponEvent += ShowAddWeaponText;
        
        void ShowAddHealthText(float healthCount)
        {
            var newInformation = $"+{healthCount} health units.";
            
            ShowText(newInformation);
        }
        
        void ShowAddArmorText(float armorCount)
        {
            var newInformation = $"+{armorCount} armor units.";
            
            ShowText(newInformation);
        }

        void ShowAddPlasmaText((string id, float count) plasmaData)
        {
            var newInformation = $"+{plasmaData.count} {plasmaData.id} plasma units.";

            ShowText(newInformation);
        }

        void ShowAddWeaponText(WeaponData weaponData)
        {
            var newInformation = $"Added {weaponData.Name} weapon.";

            ShowText(newInformation);
        }
    }

    private void Update()
    {
        VanishTimerSet();
        
        if(timerIsEnd)
            VanishTextSmooth();
    }

    private void VanishTimerSet()
    {
        if (textVanishTimer <= 0)
        {
            timerIsEnd = true;
            return;
        }
        
        timerIsEnd = false;
        
        textVanishTimer -= Time.deltaTime;
    }
    
    private void ShowText(string text)
    {
        timerIsEnd = false;
        textVanishTimer = textVanishTime;
        
        additionalInformationText.text = text;
        SetMainTextTransparency(1);
    }

    private void VanishTextSmooth()
    {
        var timeStep = Time.deltaTime * textVanishSpeed;
        var newTextColor = additionalInformationText.color;

        var newTextTransparency = Mathf.Lerp(newTextColor.a,0,timeStep);
        
        SetMainTextTransparency(newTextTransparency);
    }

    private void SetMainTextTransparency(float newTransparency)
    {
        var newTextColor = additionalInformationText.color;
        newTextColor.a = newTransparency;

        additionalInformationText.color = newTextColor;
    }
    
    
}
