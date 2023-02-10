using System;
using System.Collections;
using UnityEngine;

public abstract class DefaultBotAttack : MonoBehaviour
{
    [SerializeField] internal GameObject bullet;
    [SerializeField] internal Transform[] shotPoints = new Transform[0];
    [SerializeField] internal bool isAttack = false;
    [SerializeField] internal bool isShoot = false;
    [SerializeField] internal bool isMeleeAttack = false;
    [SerializeField] internal DefaultBotEffects botEffects;
    [SerializeField] internal DefaultBot bot;

    [SerializeField] internal float onAttackTime;
    [SerializeField] internal float currentWaitTime;
    [SerializeField] internal int attackPhase;
    protected bool isRecentlyLoad;

    protected event Action shotEvent;
    protected event Action loopShotStopEvent;
    
    protected event Action meleeAttackEvent;
    protected event Action preShotEvent;

    public void Load(DefaultBotAttack attack)
    {
        onAttackTime = attack.onAttackTime;
        currentWaitTime = attack.currentWaitTime;
        attackPhase = attack.attackPhase;
    }

    private void Awake()
    {
        if (onAttackTime > 0)
            isRecentlyLoad = true;
    }
    
    public abstract void StartAttack();

    public abstract void StartMeleeAttack(Transform target);

    public virtual GameObject Shot(Transform point,GameObject bullet = null)
    {
        if (bullet == null)
            bullet = this.bullet;

        ActivateShotEvent();
        
        return Instantiate(bullet, point.position, point.rotation);
    }

    public void Update()
    {
        if (bot.isAnnoyed)
            onAttackTime += Time.deltaTime;
    }

    protected void SimpleMeleeAttack(Transform target,float damage)
    {
        Health health;

        if (target.gameObject.TryGetComponent<Health>(out health))
            StartCoroutine(MeleeAttack());
        
        IEnumerator MeleeAttack()
        {
            isMeleeAttack = true;

            if (attackPhase == 0)
            {
                yield return WaitTime(0.5f);

                if (bot.isTargetVeryClosely)
                {
                    health.GetDamage(damage);
                }
                
                ActivateMeleeAttackEvent();
                
                attackPhase = 1;
                botEffects.PlayMeleeAttackParticls();

                if(meleeAttackEvent != null)
                    meleeAttackEvent.Invoke();
            }

            if (attackPhase == 1)
            {
                yield return WaitTime(0.5f);
                attackPhase = 0;
            }
            
            isMeleeAttack = false;
            isRecentlyLoad = false;
        }
    }

    protected IEnumerator WaitTime(float time)
    {
        if(!isRecentlyLoad)
            currentWaitTime = onAttackTime + time;

        while (true)
        {
            if(onAttackTime >= currentWaitTime)
                break;

            yield return null;
        }
    }

    public void SubShotEvent(Action action)
    {
        if (action != null)
            shotEvent += action;
    }
    
    public void UnSubShotEvent(Action action)
    {
        if (action != null)
            shotEvent -= action;
    }
    
    public void SubPreShotEvent(Action action)
    {
        if (action != null)
            preShotEvent += action;
    }
    
    public void UnSubPreShotEvent(Action action)
    {
        if (action != null)
            preShotEvent -= action;
    }
    
    public void SubLoopShotStopEvent(Action action)
    {
        if (action != null)
            loopShotStopEvent += action;
    }
    
    public void UnSubLoopShotStopEvent(Action action)
    {
        if (action != null)
            loopShotStopEvent -= action;
    }
    
    public void SubMeleeAttackEvent(Action action)
    {
        if (action != null)
            meleeAttackEvent += action;
    }
    
    public void UnSubMeleeAttackEvent(Action action)
    {
        if (action != null)
            meleeAttackEvent -= action;
    }

    protected void ActivatePreShotEvent()
    {
        if(preShotEvent != null)
            preShotEvent.Invoke();
    }

    protected void ActivateShotEvent()
    {
        if(shotEvent != null)
            shotEvent.Invoke();
    }

    protected void ActivateLoopShotStopEvent()
    {
        if(loopShotStopEvent != null)
            loopShotStopEvent.Invoke();
    }
    
    protected void ActivateMeleeAttackEvent()
    {
        if(meleeAttackEvent != null)
            meleeAttackEvent.Invoke();
    }

}
