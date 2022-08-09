using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBotAnimations : MonoBehaviour
{
    [SerializeField] private Animator botAnimator;

    public virtual void SetAnimations(bool isAttacking, bool isMeleeAttacking, bool isPrepareAttack)
    {
        botAnimator.SetBool("isAttacking", isAttacking);
        botAnimator.SetBool("isMeleeAttacking", isMeleeAttacking);
        botAnimator.SetBool("isPrepareAttack",isPrepareAttack);
    }


}
