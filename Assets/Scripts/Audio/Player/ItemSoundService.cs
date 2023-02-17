using UnityEngine;

public class ItemSoundService : MonoBehaviour
{
    [SerializeField] private OrdinaryPlayerItem playerItem;
    [SerializeField] private AudioCastData collectItemSoundData;

    private void Start()
    {
        if (playerItem == null)
            playerItem = GetComponent<OrdinaryPlayerItem>();
        
        playerItem.SubItemCollectEvent(CollectItemSoundCast);
    }

    private void CollectItemSoundCast()
    {
        var collectItemSoundData = this.collectItemSoundData;
        collectItemSoundData.castPos = playerItem.transform.position;
        
        AudioPoolService.currentAudioPoolService.CastAudio(collectItemSoundData);
    }

}
