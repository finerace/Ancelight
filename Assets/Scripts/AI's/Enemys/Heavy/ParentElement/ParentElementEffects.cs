using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentElementEffects : DefaultBotEffects
{
    [SerializeField] private ParentElementBot bot;
    [SerializeField] private Gradient colorsOnAttackChargeTime;
    [SerializeField] private float colorChangeSpeed = 2;
    [SerializeField] private MeshRenderer shieldMesh;

    private Material shieldMat;

    internal float chargeScale = 0;
    private float chargeTimer = 0;
    internal float startAttackColdown;

    private new void Start()
    {
        base.Start();

        shieldMat = shieldMesh.material;
    }

    public override void MaterialManageService()
    {
        base.MaterialManageService();

        ChargeTimerTick();

        SetChargeScale();

        ChangeColorOnChargeTime();

        ChangeShield();

        void ChangeColorOnChargeTime()
        {
            float timeStep = Time.deltaTime * colorChangeSpeed;

            //\\
            Color nowChargeScaleColor = 
                colorsOnAttackChargeTime.Evaluate(chargeScale) * botActiveBright;

            Color nowColor = mainMaterial.GetColor("EmissionColor");

            nowChargeScaleColor =
                Color.Lerp(nowColor,nowChargeScaleColor,timeStep);
            //\\

            mainMaterial.SetColor("EmissionColor", nowChargeScaleColor);
        }

        void ChargeTimerTick()
        {
            if(chargeTimer < startAttackColdown && bot.botAttack.isAttack)
                chargeTimer += Time.deltaTime;
        }
        void SetChargeScale()
        {
            chargeScale = chargeTimer / startAttackColdown;
        }

        void ChangeShield()
        {
            Color nowColor = mainMaterial.GetColor("EmissionColor");
            shieldMat.SetColor("_MainColor", nowColor);

            float alphaAmount = bot.ShieldHP / bot.ShieldMaxHP;
            shieldMat.SetFloat("_Alpha",alphaAmount);

        }
    }

    public void ResetChargeTimer()
    {
        chargeTimer = 0;
    }

}
