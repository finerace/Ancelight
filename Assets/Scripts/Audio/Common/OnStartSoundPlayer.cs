using UnityEngine;

public class OnStartSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioCastData soundData;
    [Space]
    [SerializeField] private Transform soundParent;
    
    private void Start()
    {
        var audioPool = AudioPoolService.currentAudioPoolService;
        var soundData = this.soundData;
        
        if(soundParent != null)
            soundData.castParent = soundParent.parent;
        
        soundData.castPos = transform.position;
        
        audioPool.CastAudio(soundData);
    }
}
