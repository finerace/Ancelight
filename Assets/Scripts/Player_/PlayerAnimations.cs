using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator handsAnimator;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerWeaponsManager weaponsManager;

    [SerializeField] private Transform head;
    [SerializeField] private Transform camera_;

    private float startHandsSpeed;
    private float handsSpeedColdownTimer = 0;
    private bool isHandsColdown = false;

    private const string walkID = "Walk";
    private const string flieID = "Flie";
    private const string weaponID = "WeaponID";
    private const string isFireID = "IsFire";
    private const string fistsStateID = "FistsState";

    private void Start()
    {
        bodyAnimator.speed = playerMovement.Speed;
        startHandsSpeed = handsAnimator.speed;

        weaponsManager.SubscribeShotEvent(HandsSpeedColdown);
        weaponsManager.SubWeaponChangeEvent(HandsSpeedColdown);

    }

    private void FixedUpdate()
    {
        //Инициализация вспомогательных переменных
        float vertical = Axes.Vertical;
        float horizontal = Axes.Horizontal;
        float mousex = Axes.MouseX;

        bool IsWalked;
        bool IsFlies = playerMovement.isFlies;

        bool IsAttacking = weaponsManager.IsAttacking;
        int WeaponID = weaponsManager.SelectedWeaponID;
        bool fistsState = weaponsManager.WeaponSpecialState;

        //Проверка на ходьбу и движение мышью
        IsWalked = playerMovement.IsWalked;

        //Назначение нужной скорости взависимости от состояния игрока
        if (!IsAttacking && !isHandsColdown)
        {
            if (IsWalked)
                handsAnimator.speed = startHandsSpeed * 3f;
            else 
                handsAnimator.speed = startHandsSpeed;
        }
        else handsAnimator.speed = startHandsSpeed;

        //Финальное назначения в аниматорах. Движение головы
        head.localEulerAngles = camera_.localEulerAngles;

        bodyAnimator.SetBool(walkID, IsWalked);
        bodyAnimator.SetBool(flieID, IsFlies);

        handsAnimator.SetInteger(weaponID,WeaponID);
        handsAnimator.SetBool(isFireID,IsAttacking);
        handsAnimator.SetBool(fistsStateID, fistsState);

        int handsCurrentAnimTagHash = handsAnimator.GetCurrentAnimatorStateInfo(0).tagHash;
        int weaponCurrentSelectedIdHash = Animator.StringToHash(weaponsManager.SelectedWeaponID.ToString());

        if(handsCurrentAnimTagHash == weaponCurrentSelectedIdHash && !handsAnimator.IsInTransition(0))
        {
            weaponsManager.animatorWeaponСhangeAllowed = true;
            weaponsManager.animatorAttackAllowed = true;
        }
        else
        {
            //weaponsManager.animatorWeaponСhangeAllowed = false;
            weaponsManager.animatorAttackAllowed= false;
        }

    }

    private void Update()
    {
        if (handsSpeedColdownTimer > 0)
        {
            isHandsColdown = true;
            handsSpeedColdownTimer -= Time.deltaTime;
        }
        else
            isHandsColdown = false;


    }

    private void HandsSpeedColdown()
    {
        handsSpeedColdownTimer = 0.55f;
    }


}
