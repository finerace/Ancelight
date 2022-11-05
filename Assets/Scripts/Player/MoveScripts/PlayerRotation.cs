using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Transform transform_;
    public float mouseSensivity = 1f;
    public bool mouseInvert = false;

    [SerializeField] private PlayerWeaponsManager weaponsManager;
    private PlayerWeaponRecoil weaponRecoil;
    private bool isManageActive = true;

    private void Start()
    {
        //??? ??????? ? ?????????? ??????????????? ?????
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(cameraTransform == null)
            cameraTransform = GameObject.Find("PlayerCam").transform;

        transform_ = transform;

        weaponRecoil = new PlayerWeaponRecoil(cameraTransform,transform_,0.25f,6);

        void GetRecoil()
        {
            float currentWeaponDamage = weaponsManager.selectedWeaponData.Damage;

            SpecialWeaponsScaleDamage();

            StartCoroutine(weaponRecoil.GetRecoilUpdater(currentWeaponDamage));


            void SpecialWeaponsScaleDamage()
            {
                switch (weaponsManager.selectedWeaponData.Id)
                {
                    case 1: //Fists
                        currentWeaponDamage = 0;
                        break;

                    case 3: //Lazer Gun
                        currentWeaponDamage = 80;
                        break;

                    case 8: //Lazer Gun
                        currentWeaponDamage *= 0.2f;
                        break;

                    case 9: //BPJ-Collapser
                        currentWeaponDamage *= 0.25f;
                        break;

                }
            }

        }

        weaponsManager.SubscribeShotEvent(GetRecoil);
    }

    private void Update()
    {
        if (!isManageActive)
            return;

        //????????????? ??????????????? ??????????
        float MouseX = Axis.MouseX;
        float MouseY = Axis.MouseY;

        if (MouseX + MouseY != 0)
        {
            //??????? ????
            transform_.localEulerAngles += new Vector3(0, MouseX, 0);

            float lookX = cameraTransform.eulerAngles.x;

            //???????? ?? ???????????? ??????? ??????
            if (lookX - MouseY >= 90 && lookX - MouseY <= 235)
                cameraTransform.localEulerAngles = new Vector3(90, 0, 0);

            else if (lookX - MouseY >= 235 && lookX - MouseY <= 270)
                cameraTransform.localEulerAngles = new Vector3(270, 0, 0);

            //???? ?? ?????? ?????????? ???????? ?????? ?????? ????????
            else cameraTransform.localEulerAngles = new Vector3(lookX - MouseY, 0, 0);

        }
    }
    
    public void SetManageActive(bool state)
    {
        isManageActive = state;
    }

}
