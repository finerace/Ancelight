using UnityEngine;
using System;

public abstract class Health : MonoBehaviour
{

    [SerializeField] private float health_;
    [SerializeField] private float maxHealth_;
    [SerializeField] internal bool isDie;
    public event Action OnDie;
    public event Action<float> OnDamageEvent; 

    public float Health_ => health_;

    public float MaxHealth_ => maxHealth_;

    public virtual void GetDamage(float damage,Transform source = null)
    {
        if(damage < 0)
            damage = 0;

        if(health_-damage <= 0)
        {
            if(!isDie)
            {
                isDie = true;

                if(OnDie != null)
                    OnDie.Invoke();

                Died();
            }

            health_ = 0;
        }
        else if (health_ - damage >= maxHealth_)
            health_ = maxHealth_;
        else
        {
            health_ -= damage;
            
            if(OnDamageEvent != null)
                OnDamageEvent.Invoke(damage);
        }
    }

    public virtual void SetMaxHealth(float maxHealth)
    {
        if (maxHealth > 0)
            maxHealth_ = maxHealth;
        else 
            throw new Exception("?????? ???????? ???????????? ????????!");
    }
    
    public virtual void SetHealth(float health)
    {
        if (health > 0)
            this.health_ = health;
        else 
            throw new Exception("?????? ???????? ???????????? ????????!");
    }

    public virtual void AddHealth(float health)
    {
        if (health_ + health >= maxHealth_)
            health_ = maxHealth_;
        else if (health_ + health <= 0)
        {
            if (!isDie)
            {
                isDie = true;
                OnDie.Invoke();
                Died();
            }
        }
        else
            health_ += health;
    }

    public virtual void Died() { 
        isDie = true; 
    }


}
