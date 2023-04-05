using UnityEngine;

public class PlayerExistModulesTips : MonoBehaviour
{
    private PlayerMainService playerMain;
    [SerializeField] private GameObject hookExistTipObj;
    [SerializeField] private GameObject dashExistTipObj;
    [SerializeField] private GameObject protectionExistTipObj;

    private void Awake()
    {
        playerMain = FindObjectOfType<PlayerMainService>();
        
        if (!playerMain.hookService.IsHookExist)
        {
            playerMain.hookService.OnHookUnlock += DisableTipObject;
            void DisableTipObject()
            {
                hookExistTipObj.SetActive(false);
            }
            
        }
        
        if (!playerMain.dashsService.IsDashServiceExist)
        {
            playerMain.dashsService.OnDashServiceUnlock += DisableTipObject;
            void DisableTipObject()
            {
                dashExistTipObj.SetActive(false);
            }
        }

        if (!playerMain.immediatelyProtectionService.IsProtectionExist)
        {
            playerMain.immediatelyProtectionService.OnUnlock += DisableTipObject;
            void DisableTipObject()
            {
                protectionExistTipObj.SetActive(false);
            }
        }
    }
}
