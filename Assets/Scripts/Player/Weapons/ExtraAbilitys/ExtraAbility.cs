using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ExtraAbility : MonoBehaviour
{
    internal PlayerWeaponsManager weaponsManager;
    [SerializeField] internal bool isDelay;
    [SerializeField] internal bool isAttack;
    private float delayTime;
    private float attackTime;

    internal void Start()
    {
        weaponsManager = FindObjectOfType<PlayerWeaponsManager>();
        delayTime = weaponsManager.SelectedAbilityData.DelayTime;
        attackTime = weaponsManager.SelectedAbilityData.AttackTime;
        SubAbility();
    }

    public virtual void LaunchAbility()
    {
        StartCoroutine( StartDelayTimer());
        StartCoroutine(StartAttackTimer());
    }
    
    private IEnumerator StartDelayTimer()
    {
        isDelay = true;
        yield return new WaitForSeconds(delayTime);
        isDelay = false;
    }

    private IEnumerator StartAttackTimer()
    {
        isAttack = true;
        yield return new WaitForSeconds(attackTime);
        isAttack = false;
    }

    internal virtual void SubAbility()
    {
        weaponsManager.extraAbilityUseEvent.AddListener(LaunchAbility);
    }

    internal virtual void UnsubAbility()
    {
        weaponsManager.extraAbilityUseEvent.RemoveListener(LaunchAbility);
    }

    internal void OnDestroy()
    {
        UnsubAbility();
    }

}
