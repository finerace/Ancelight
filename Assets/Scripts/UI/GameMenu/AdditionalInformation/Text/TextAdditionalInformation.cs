using System;
using TMPro;
using UnityEngine;

public class TextAdditionalInformation : MonoBehaviour
{
    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private TextMeshProUGUI additionalInformationText;

    [Space] 
    
    [SerializeField] private float textVanishTime;
    private float textVanishTimer = 0;
    private bool timerIsEnd = true;

    private void Start()
    {
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
            VanishText();
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
    }

    private void VanishText()
    {
        if(additionalInformationText.text != String.Empty)
            additionalInformationText.text = String.Empty;
    }
    
    
    
    
}
