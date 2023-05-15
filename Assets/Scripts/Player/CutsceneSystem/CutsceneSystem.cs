using UnityEngine;
using UnityEngine.Playables;

public class CutsceneSystem : MonoBehaviour
{
    private CutsceneModeMenuSwitchService cutsceneSwitchService;
    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private Vector3 playerCameraLocalPos;
    
    [Space] 

    [SerializeField] private PlayableDirector[] cutscenesList;
    private PlayableDirector currentCutscene;
    private bool isCutsceneActive;

    private Camera playerCamera;
    private float cameraFOV;

    [SerializeField] private SpriteRenderer cutsceneBlackScreen;
    
    private void Start()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();

        cutsceneSwitchService = FindObjectOfType<CutsceneModeMenuSwitchService>();
        
        playerCamera = Camera.main;
    }

    public void StartCutscene(int cutsceneListId)
    {
        playerMainService.SetManageActive(false);
        playerMainService.SetPlayerHandsActive(false);
        cutsceneSwitchService.SetCutsceneMode(true);

        isCutsceneActive = true;
        
        var cutscenePlayer = cutscenesList[cutsceneListId];
        cutscenePlayer.gameObject.SetActive(true);
        currentCutscene = cutscenePlayer;

        cameraFOV = playerCamera.fieldOfView; 
        
        cutscenePlayer.Play();
        cutscenePlayer.stopped += StopCutscene;
        playerMainService.OnDamageEvent += StopCutsceneOnDamage;
    }

    private void StopCutscene(PlayableDirector cutscenePlayer)
    {
        cutscenePlayer.gameObject.SetActive(false);
        
        playerMainService.SetManageActive(true);
        playerMainService.SetPlayerHandsActive(true);
        cutsceneSwitchService.SetCutsceneMode(false);
        
        isCutsceneActive = false; 
        currentCutscene = null;
        playerCamera.fieldOfView = cameraFOV;

        var playerCameraT = playerMainService.playerLook.MainCameraT;
        playerCameraT.localPosition = playerCameraLocalPos;
        playerCameraT.localRotation = Quaternion.Euler(Vector3.zero);
        cutscenePlayer.stopped -= StopCutscene;
        playerMainService.OnDamageEvent -= StopCutsceneOnDamage;

        var newBlackScreenColor = Color.black;
        newBlackScreenColor.a = 0;

        cutsceneBlackScreen.color = newBlackScreenColor;
    }

    private void Update()
    {
        if(isCutsceneActive && Input.GetKey(KeyCode.H))
            StopCutscene(currentCutscene);

        if (isCutsceneActive)
        {
            playerMainService.SetManageActive(false);    
        }
        
    }
    
    private void StopCutsceneOnDamage(float damage)
    {
        StopCutscene(currentCutscene);
    }

}
