using System.Collections;
using System.Collections.Generic;
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

    public abstract void StartAttack();

    public abstract void StartMeleeAttack(Transform target);

    public virtual GameObject Shot(Transform point,GameObject bullet = null)
    {
        if (bullet == null)
            bullet = this.bullet;

        return Instantiate(bullet, point.position, point.rotation);
    }

   

    protected void SimpleMeleeAttack(Transform target,float damage)
    {
        Health health;

        if (target.gameObject.TryGetComponent<Health>(out health))
            StartCoroutine(meleeAttack());

        IEnumerator meleeAttack()
        {
            isMeleeAttack = true;

            yield return new WaitForSeconds(0.5f);

            if (bot.isTargetVeryClosely)
            {
                health.GetDamage(damage);
            }

            botEffects.PlayMeleeAttackParticls();

            yield return new WaitForSeconds(0.5f);

            isMeleeAttack = false;
        }
    }
}
