using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneSystem : MonoBehaviour
{
    [SerializeField] private PlayerMainService playerMainService;
    [SerializeField] private Vector3 playerCameraLocalPos;

    [Space] 

    [SerializeField] private PlayableDirector[] cutscenesList;
    private PlayableDirector currentCutscene;

    private void Start()
    {
        playerMainService = FindObjectOfType<PlayerMainService>();
    }

    public void StartCutscene(int cutsceneListId)
    {
        playerMainService.SetManageActive(false);

        var cutscenePlayer = cutscenesList[cutsceneListId];
        
        cutscenePlayer.Play();
        cutscenePlayer.stopped += StopCutscene;
    }

    private void StopCutscene(PlayableDirector playableDirector)
    {
        playableDirector.gameObject.SetActive(false);
        playerMainService.SetManageActive(true);
        
        var playerCameraT = playerMainService.playerLook.MainCameraT;
        playerCameraT.localPosition = playerCameraLocalPos;
        playerCameraT.localRotation = Quaternion.Euler(Vector3.zero);
    }

}
