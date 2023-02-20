using UnityEngine;

public class GameMenuSoundService : MonoBehaviour
{
    private AudioPoolService audioPoolService;
    private PlayerMainService playerMainService;

    [SerializeField] private DashsIndicatorService dashsIndicatorService;
    
    [Space] 
    
    [SerializeField] private AudioCastData onWeaponChangeSound;
    [SerializeField] private AudioCastData onDashUnitReadySound;
    [SerializeField] private AudioCastData onHookFullRegenerateSound;
    
    private void Start()
    {
        audioPoolService = AudioPoolService.audioPoolServiceInstance;
        playerMainService = FindObjectOfType<PlayerMainService>();
        
        AddCastMethodsToEvents();
    }

    private void AddCastMethodsToEvents()
    {
        playerMainService.weaponsManager.SubWeaponChangeEvent(WeaponChangeSoundCast);
        dashsIndicatorService.DashUnitReadyEvent += DashUnitReadySoundCast;
        playerMainService.hookService.HookRegenerateEvent += HookRegenerationSoundCast;
    }
    
    private void WeaponChangeSoundCast()
    {
        var onWeaponChangeSoundData = onWeaponChangeSound;
        onWeaponChangeSoundData.castPos = playerMainService.weaponsManager.shootingPoint.position;

        audioPoolService.CastAudio(onWeaponChangeSoundData);
    }

    private void DashUnitReadySoundCast()
    {
        var onDashRegenerateSoundData = onDashUnitReadySound;
        onDashRegenerateSoundData.castPos = playerMainService.weaponsManager.shootingPoint.position;

        audioPoolService.CastAudio(onDashRegenerateSoundData);
    }
    
    private void HookRegenerationSoundCast()
    {
        var onHookRegenerationSoundData = onHookFullRegenerateSound;
        onHookRegenerationSoundData.castPos = playerMainService.weaponsManager.shootingPoint.position;

        audioPoolService.CastAudio(onHookRegenerationSoundData);
    }

}
