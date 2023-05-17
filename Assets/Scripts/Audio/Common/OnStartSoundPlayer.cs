using System;
using UnityEngine;

public class OnStartSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioCastData soundData;
    [Space]
    [SerializeField] private Transform soundParent;

    [Space] 
    
    [SerializeField] private bool onEnableMod;
    
    private void Start()
    {
        if(onEnableMod)
            return;
        
        PlaySound();
    }

    private void OnEnable()
    {
        if(onEnableMod)
            PlaySound();
    }

    private void PlaySound()
    {
        var audioPool = AudioPoolService.audioPoolServiceInstance;
        var soundData = this.soundData;
        
        if(soundParent != null)
            soundData.castParent = soundParent.parent;
        
        soundData.castPos = transform.position;
        
        audioPool.CastAudio(soundData);
    }
    
}
