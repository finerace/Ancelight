using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class OrdinaryPlayerItem : MonoBehaviour, IPlayerItem
{
    [SerializeField] protected ParticleSystem[] itemParticlesEffects;
    [SerializeField] protected GameObject itemMeshObject;
    [SerializeField] protected float itemDestroyTime = 5;
    [SerializeField] protected bool isOneUsesItem = true;
    [SerializeField] protected int itemId = 0;
    private bool itemIsDestroyed = false;
    private event Action itemCollectEvent;
    
    public int ItemId => itemId;

    protected void Start()
    {
        LevelSaveData.mainLevelSaveData.AddToSaveData(this);
    }

    public void PickUp(PlayerMainService player)
    {
        if(itemIsDestroyed)
            return;

        PickUpItemAlgorithm(player);
        
        
        if(itemCollectEvent != null && itemIsDestroyed)
            itemCollectEvent.Invoke();
    }

    protected abstract void PickUpItemAlgorithm(PlayerMainService player);

    protected virtual void DestroyItem()
    {
        if(itemIsDestroyed)
            return;
            
        LevelSaveData.mainLevelSaveData.RemoveFromSaveData(this);
        
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

    public void SubItemCollectEvent(Action action)
    {
        if (action != null)
            itemCollectEvent += action;
    }
    public void UnSubItemCollectEvent(Action action)
    {
        if (action != null)
            itemCollectEvent -= action;
    }
}

public class PlayerItemContainer : OrdinaryPlayerItem
{
    protected override void PickUpItemAlgorithm(PlayerMainService player)
    {
        throw new NotImplementedException();
    }
}

public interface IPlayerItem
{
    public void PickUp(PlayerMainService player);
}
