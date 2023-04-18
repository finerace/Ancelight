using UnityEngine;

public class SuitInformationButtonSelector : MonoBehaviour
{
    [SerializeField] private SuitInformationSetUI uiSetter;
    [SerializeField] private ButtonMainService buttonMainService;
    
    public void SetInformation(int informationId)
    {
        void OpenInformation()
        {
            uiSetter.OpenInformation(informationId);
        }
        
        buttonMainService.onClickAction.AddListener(OpenInformation);
    }
}
