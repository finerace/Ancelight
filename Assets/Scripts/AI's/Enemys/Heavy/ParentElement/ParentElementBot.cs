using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentElementBot : DefaultBot
{
    [SerializeField] private GameObject shieldObj;
    [SerializeField] private ParentElementAttack attackComp;
    [SerializeField] private float shieldHP;
    [SerializeField] private float shieldMaxHP;
    [SerializeField] private float headRotationSpeed = 3;
    [SerializeField] private float shieldRegenerationSpeed = 1;
    [SerializeField] private int childsMax = 10;
    [SerializeField] private int childsCountNow = 0;

    private bool isShieldBroken = false;


    public float ShieldHP => shieldHP;
    public float ShieldMaxHP => shieldMaxHP;

    private new void Start()
    {
        base.Start();

        attackComp.childSpawn += AddChild;

        void AddChild(Health health)
        {
            childsCountNow++;

            health.OnDie += () => { 

                RemoveChild(); 

            };
        }

        void RemoveChild()
        {
            childsCountNow--;
        }
    }

    internal new void Update()
    {
        base.Update();

        HeadRotation();
        ShieldRegeneration();
    }

    public override void GetDamage(float damage, Transform source)
    {
        if(isShieldBroken)
        {
            base.GetDamage(damage, source);
            return;
        }

        if (damage < 0)
            damage = 0;

        if (shieldHP - damage <= 0)
        {
            shieldHP = 0;
            damage -= shieldHP;
            BrokeShield();
        }
        else
        {
            shieldHP -= damage;
            damage = 0;
        }

        if(damage > 0)
            base.GetDamage(damage, source);
    }

    internal override IEnumerator IsAttacksAllowChecker(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            bool isChildsMany = childsCountNow >= childsMax;

            if (isAnnoyed && !botAttack.isAttack && !isChildsMany)
            {
                botAttack.StartAttack();
            }

            if (isTargetVeryClosely && !botAttack.isMeleeAttack)
                botAttack.StartMeleeAttack(target);

        }
    }

    private void HeadRotation()
    {
        float timeStep = Time.deltaTime * headRotationSpeed;
        float staticHeadRotationSpeed = 10f;

        head.localRotation *= Quaternion.Euler(0, staticHeadRotationSpeed * timeStep,0);
    }

    private void ShieldRegeneration()
    {
        if (isShieldBroken)
            return;

        float timeStep = Time.deltaTime * shieldRegenerationSpeed;

        if (shieldHP + timeStep < shieldMaxHP)
            shieldHP += timeStep;
    }    

    private void BrokeShield()
    {
        isShieldBroken = true;
        shieldObj.SetActive(false);

    }
}
