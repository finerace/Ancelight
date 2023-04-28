using UnityEngine;

public class KeyTipUseButton : KeyboardKeyTip
{
    private PlayerUseService playerUseService;
    [SerializeField] private SmoothVanishUI smoothVanishUI;
    
    private new void Start()
    {
        playerUseService = FindObjectOfType<PlayerUseService>();
        base.Start();
    }
    
    protected override IUsePlayerDevicesButtons GetDevicesButton()
    {
        return playerUseService;
    }

    private void Update()
    {
        smoothVanishUI.SetVanish(!playerUseService.IsButtonClose);
    }
}
