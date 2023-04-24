using UnityEngine;

public class PlayerExistModulesTips : MonoBehaviour
{
    private PlayerMainService playerMain;
    [SerializeField] private GameObject hookExistTipObj;
    [SerializeField] private GameObject dashExistTipObj;
    [SerializeField] private GameObject protectionExistTipObj;
    [SerializeField] private bool isInvert;
    
    private void Start()
    {
        playerMain = FindObjectOfType<PlayerMainService>();
        
        if (!playerMain.hookService.IsHookExist)
        {
            playerMain.hookService.OnHookUnlock += DisableTipObject;
            void DisableTipObject()
            {
                hookExistTipObj.SetActive(isInvert);
            }
        }
        else
            hookExistTipObj.SetActive(isInvert);
        
        if (!playerMain.dashsService.IsDashServiceExist)
        {
            playerMain.dashsService.OnDashServiceUnlock += DisableTipObject;
            void DisableTipObject()
            {
                dashExistTipObj.SetActive(isInvert);
            }
        }
        else
            dashExistTipObj.SetActive(isInvert);

        if (!playerMain.immediatelyProtectionService.IsProtectionExist)
        {
            playerMain.immediatelyProtectionService.OnUnlock += DisableTipObject;
            void DisableTipObject()
            {
                protectionExistTipObj.SetActive(isInvert);
            }
        }
        else
            protectionExistTipObj.SetActive(isInvert);
    }
}
