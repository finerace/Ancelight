using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnAimExtraIcons : MonoBehaviour
{
    private PlayerWeaponsManager weaponsManager;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image abilityIcon;
    private Material weaponIconMaterial;
    private Material abilityIconMaterial;
    [SerializeField] private bool isOnweaponIcon;
    [Space]
    [SerializeField] private float weaponIconDisappearSpeed;
    [SerializeField] private float abilityIconDisappearSpeed;

    private void Start()
    {
        weaponsManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();

        weaponIconMaterial = new Material(weaponIcon.material);
        weaponIcon.material = weaponIconMaterial;

        abilityIconMaterial = new Material(abilityIcon.material);
        abilityIcon.material = abilityIconMaterial;

        weaponsManager.SubWeaponChangeUseButtonEvent(UpdateWeaponIcon);

    }

    private void Update()
    {
        float disappearSpeed = Time.deltaTime * weaponIconDisappearSpeed;

        //WeaponIcon+
        if (weaponIcon.color.a > 0)
        {
            float weaponAlpha = weaponIcon.color.a;
            weaponAlpha -= disappearSpeed;

            
            weaponIcon.color =
               new Color(weaponIcon.color.r, weaponIcon.color.g, weaponIcon.color.b, weaponAlpha);
        }

        //AbilityIcon
        if (abilityIcon.color.a > 0)
        {
            disappearSpeed = Time.deltaTime * abilityIconDisappearSpeed;

            float abilityAlpha = abilityIcon.color.a;
            abilityAlpha -= disappearSpeed;

            abilityIcon.color =
                    new Color(abilityIcon.color.r, abilityIcon.color.g, abilityIcon.color.b, abilityAlpha);
        }

    }

    private void UpdateWeaponIcon()
    {
        if (isOnweaponIcon)
        {
            WeaponData nowWeaponData = weaponsManager.FindWeaponData(weaponsManager.SelectedWeaponID);

            weaponIcon.sprite = nowWeaponData.Icon;

            weaponIcon.color =
                new Color(weaponIcon.color.r, weaponIcon.color.g, weaponIcon.color.b, 1f);

            Color setColor = new Color(0.15f, 0.15f, 0.15f);

            if (weaponsManager.bulletsManager.CheckBullets(nowWeaponData.BulletsID))
                setColor = nowWeaponData.MainColor;

            weaponIconMaterial.SetColor("_MainColor", setColor);
        }
    }

}
