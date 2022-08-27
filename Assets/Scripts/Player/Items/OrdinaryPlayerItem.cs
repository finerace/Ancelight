using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class OrdinaryPlayerItem : MonoBehaviour, IPlayerItem
{
    [SerializeField] protected ParticleSystem[] itemParticlesEffects;
    [SerializeField] protected GameObject itemMeshObject;
    [SerializeField] protected float itemDestroyTime = 5;
    [SerializeField] protected bool isOneUsesItem = true;
    private bool itemIsDestroyed = false;

    public void PickUp(PlayerMainService player)
    {
        if(itemIsDestroyed)
            return;

        PickUpItemAlgorithm(player);
    }

    protected abstract void PickUpItemAlgorithm(PlayerMainService player);

    protected virtual void DestroyItem()
    {
        if(itemIsDestroyed)
            return;
            
        itemIsDestroyed = true;
        
        DestroyMainItemObject();
        DestroyItemMeshObject();
        
        DestroyParticles();

        void DestroyMainItemObject()
        {
            var mainItemObject = gameObject;
            
            Destroy(mainItemObject,itemDestroyTime);
        }

        void DestroyItemMeshObject()
        {
            Destroy(itemMeshObject);
        }

        void DestroyParticles()
        {
            foreach (var localParticle in itemParticlesEffects)
            {
                var particlesDestroyTime = localParticle.main.startLifetime.constantMax;
                
                localParticle.Stop();
                
                Destroy(localParticle.gameObject,particlesDestroyTime);
            }
            
        }
    }
    
}

public interface IPlayerItem
{
    public void PickUp(PlayerMainService player);
}
