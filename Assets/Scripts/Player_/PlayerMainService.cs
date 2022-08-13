using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMainService : Health
{
    [Space]
    [SerializeField] private float maxArmor;
    [SerializeField] private float armor;

    public float MaxArmor { get { return maxArmor; }}
    public float Armor { get { return armor; }}

    [Space]
    [SerializeField] internal PlayerMovement playerMovement;
    [SerializeField] internal PlayerWeaponsManager weaponsManager;
    [SerializeField] internal PlayerWeaponsBulletsManager weaponsBulletsManager;
    [SerializeField] internal PlayerRotation playerRotation;
    [SerializeField] internal PlayerLookService playerLook;
    [SerializeField] internal PlayerHookService hookService;
    [SerializeField] internal PlayerDashsService dashsService;
    [SerializeField] internal PlayerImmediatelyProtectionService immediatelyProtectionService; 
    
    [SerializeField] public event Action<float> GetDamageEvent;

    private void Awake()
    {
        CheckPlayerComponents();
    }

    public void AddWeapon(int id)
    {
        weaponsManager.UnlockWeapon(id);
    }

    public void AddBullets(int id,int count)
    {
        weaponsBulletsManager.AddBullets(id, count);
    }

    public override void GetDamage(float damage,Transform source)
    {
        if(armor >= damage)
        {
            armor -= damage/2f;
            damage /= 2.5f;
        } 
        else if(armor < damage && armor > 0)
        {
            damage -= armor / 2.5f;
            armor = 0;
        }

        if(GetDamageEvent != null)
            GetDamageEvent.Invoke(damage);

        base.GetDamage(damage);
    }

    public void AddArmor(float armor)
    {
        if(this.armor + armor >= maxArmor)
            this.armor = armor;
        else if(this.armor + armor <= 0)
            this.armor = 0;
        else
            this.armor += armor;
    }

    public void SetManageActive(bool state)
    {
        playerMovement.SetManageActive(state);
        weaponsManager.SetManageActive(state);
        playerRotation.SetManageActive(state);
    }

    public override void Died()
    {

    }

    private void CheckPlayerComponents()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
        
        if (weaponsManager == null)
            weaponsManager = GetComponent<PlayerWeaponsManager>();
        
        if (weaponsBulletsManager == null)
            weaponsBulletsManager = GetComponent<PlayerWeaponsBulletsManager>();
        
        if (playerRotation == null)
            playerRotation = GetComponent<PlayerRotation>();
        
        if (playerLook == null)
            playerRotation = GetComponent<PlayerRotation>();
    }

}
