using UnityEngine;

public class BulletSoundService : MonoBehaviour
{
    private AudioPoolService audioPoolService;
    [SerializeField] private Bullet bulletMain;
    
    [Space]
    
    [SerializeField] private AudioCastData bulletDestroySoundData;

    private void Start()
    {
        audioPoolService = AudioPoolService.audioPoolServiceInstance;

        if (bulletMain == null)
            bulletMain = GetComponent<Bullet>();
        
        bulletMain.SubDestroyEvent(DestroySoundCastAlgorithm);
    }

    private void DestroySoundCastAlgorithm()
    {
        bulletDestroySoundData.castPos = bulletMain.body_.position;

        audioPoolService.CastAudio(bulletDestroySoundData);
    }
}
