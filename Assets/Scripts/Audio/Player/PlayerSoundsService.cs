using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsService : MonoBehaviour
{
    private AudioPoolService audioPoolService;
    [SerializeField] private PlayerMainService playerMainService;
    private float mainSoundTime;
    
    [Space] 
    
    [SerializeField] private PlayerWalkZone playerWalkZone;
    private PlayerMovement playerMovement;
    private float playerWalkTime = 0;
    private float nextStepSoundCastTime = 0;
    [SerializeField] private float playerStepsTimeCastSmooth = 2;
    
    [Space]
    
    [SerializeField] private AudioCastData hardMatStepSounds;
    [SerializeField] private AudioCastData grassStepSounds;
    [SerializeField] private AudioCastData waterStepSounds;
    
    [Space]
    
    [SerializeField] private AudioCastData walkClothSound;
    private AudioSource currentWalkClothAudioSource = null;
    [SerializeField] private AudioCastData jumpSound;

    [Space]
    
    private PlayerWeaponsManager playerWeaponsManager;
    [SerializeField] private float weaponChangeCooldownSmooth = 1.5f;
    private float weaponChangeNextCastAllowTime = 0;
    [SerializeField] private AudioCastData weaponChangeSounds;

    [Space] 
    [SerializeField] private AudioCastData fistsAttackSounds;
    
    [SerializeField] private AudioCastData pistolShotSounds;
    
    [SerializeField] private AudioCastData shotGunShotSounds;
    
    [SerializeField] private AudioCastData machineGunShotSounds;
    
    [SerializeField] private AudioCastData grenadeLauncherShotSounds;
    
    [SerializeField] private AudioCastData rocketLauncherShotSounds;
    
    [SerializeField] private AudioCastData superGunShotSounds;
    
    [SerializeField] private AudioCastData laserGunShotStartSounds;
    private AudioSource laserGunShotSoundSource;
    [SerializeField] private AudioCastData laserGunShotSounds;
    [SerializeField] private AudioCastData laserGunEndShotSounds;

    [SerializeField] private AudioCastData bpjCollapserShotSounds;
    [SerializeField] private AudioCastData bpjCollapserShotPreparingSounds;
    
    [Space]
    private AudioSource currentShotPreparingSource;
    
    [Space]
    
    [SerializeField] private AudioCastData dashSoundData;
    
    [Space]
    
    [SerializeField] private AudioCastData hookUseSoundData;
    [SerializeField] private AudioCastData hookHookedSoundData;
    [SerializeField] private float hookHookedSoundDelay = 0.15f;
    
    private AudioSource hookRopeSoundSource;
    [SerializeField] private AudioCastData hookRopeSoundData;
    

    private void Awake()
    {
        InitFields();
        void InitFields()
        {
            audioPoolService = FindObjectOfType<AudioPoolService>();
            
            if (playerMainService == null)
                playerMainService = GetComponent<PlayerMainService>();

            playerMovement = playerMainService.playerMovement;
            playerWeaponsManager = playerMainService.weaponsManager;
        }
    }

    private void Start()
    {
        AddJumpSoundActionToJumpEvent();
        void AddJumpSoundActionToJumpEvent()
        {
            playerMovement.onJumpEvent += JumpSoundCastAlgorithm;
        }

        AddWeaponChangeSoundCastToEvent();
        void AddWeaponChangeSoundCastToEvent()
        {
            playerWeaponsManager.SubWeaponChangeEvent(WeaponChangeSoundCastAlgorithm);
        }

        AddShotSoundCastToEvent();
        void AddShotSoundCastToEvent()
        {
            playerWeaponsManager.SubscribeShotEvent(WeaponShotSoundCastAlgorithm);
        }

        AddShotPreparingSoundCastToEvent();
        void AddShotPreparingSoundCastToEvent()
        {
            playerWeaponsManager.SubscribeShotPreparingEvent(WeaponShotPreparingCastAlgorithm);
        }

        AddDashUseSoundCastToEvent();
        void AddDashUseSoundCastToEvent()
        {
            if(playerMainService.dashsService != null)
                playerMainService.dashsService.SubDashUseEvent(DashUseSoundCastAlgorithm);
        }

        AddHookSoundsToEvents();
        void AddHookSoundsToEvents()
        {
            if (playerMainService.hookService != null)
            {
                playerMainService.hookService.SubHookUseEvent(HookUseSoundCastAlgorithm);
                playerMainService.hookService.SubHookEndUseEvent(HookUseSoundCastAlgorithm);
                
                playerMainService.hookService.SubHookEndUseEvent(HookRopeSoundStop);
                
            }
        }
    }

    private void Update()
    {
        mainSoundTime += Time.deltaTime;
        
        PlayerMovementSoundsWork();
        void PlayerMovementSoundsWork()
        {
            WalkTimeWork();
            void WalkTimeWork()
            {
                if (playerMovement.IsWalked)
                    playerWalkTime += Time.deltaTime;
            }

            WalkSoundsCastWork();
            void WalkSoundsCastWork()
            {
                StepSoundsWork();

                void StepSoundsWork()
                {
                    const float minPlayerSpeedToCastWalkSounds = 0.25f;
                    if (!(playerWalkTime >= nextStepSoundCastTime) ||
                        playerMovement.isFlies ||
                        playerMovement.PlayerRb.velocity.magnitude < minPlayerSpeedToCastWalkSounds)
                        return;

                    var audioCastData = new AudioCastData();

                    var castedClip = CastStepSound();

                    AudioClip CastStepSound()
                    {
                        InitWalkZone();

                        void InitWalkZone()
                        {
                            switch (playerWalkZone)
                            {
                                case PlayerWalkZone.HardMat:
                                    audioCastData = hardMatStepSounds;
                                    break;
                                case PlayerWalkZone.Grass:
                                    audioCastData = grassStepSounds;
                                    break;
                                case PlayerWalkZone.Water:
                                    audioCastData = waterStepSounds;
                                    break;
                            }
                        }

                        audioCastData.castPos = playerMovement.Body.position;
                        audioCastData.castParent = playerMovement.Body;

                        var castedSource = audioPoolService.CastAudio(audioCastData); 
                        
                        if(castedSource != null)
                            return castedSource.clip;

                        return null;
                    }

                    nextStepSoundCastTime = GetNextCastStepSoundTime();

                    float GetNextCastStepSoundTime()
                    {
                        if (castedClip == null)
                        {
                            return 0.5f;
                        }
                        
                        var resultTime = 0f;

                        if (castedClip != null)
                            resultTime = castedClip.length;
                        else
                            resultTime = audioCastData.Clips[0].length;

                        if (!playerMovement.IsPlayerCrouch)
                            resultTime /= playerStepsTimeCastSmooth;

                        resultTime += playerWalkTime;

                        return resultTime;
                    }
                }

                WalkClothSoundCloth();

                void WalkClothSoundCloth()
                {
                    var walkClothSoundData = walkClothSound;
                    walkClothSoundData.castParent = playerMovement.Body;
                    walkClothSoundData.castPos = playerMovement.Body.position;

                    if (playerMovement.IsWalked && !playerMovement.isFlies && currentWalkClothAudioSource == null)
                    {
                        currentWalkClothAudioSource = audioPoolService.CastAudio(walkClothSoundData);
                    }

                    if (currentWalkClothAudioSource != null && !playerMovement.IsWalked)
                    {
                        currentWalkClothAudioSource.Stop();
                        currentWalkClothAudioSource = null;
                    }
                }
            }
        }


        WeaponsSoundsWork();
        void WeaponsSoundsWork()
        {
            LaserSoundsStopAlgorithm();
            void LaserSoundsStopAlgorithm()
            {
                if (playerWeaponsManager.selectedWeaponData.Id == 8 &&
                    !playerWeaponsManager.IsAttacking &&
                    laserGunShotSoundSource != null)
                {
                    laserGunShotSoundSource.Stop();
                }
                
                if (laserGunShotSoundSource != null && !laserGunShotSoundSource.isPlaying)
                {
                    var laserGunEndShotSoundData = laserGunEndShotSounds;
                    laserGunEndShotSoundData.castParent = playerMovement.Body;
                    laserGunEndShotSoundData.castPos = playerMovement.Body.position;

                    audioPoolService.CastAudio(laserGunEndShotSoundData);

                    laserGunShotSoundSource = null;
                }
            }
        }
        
    }

    private void JumpSoundCastAlgorithm()
    {
        var jumpSoundData = jumpSound;
        jumpSoundData.castPos = playerMovement.Body.position;
        jumpSoundData.castParent = playerMovement.Body;
        
        audioPoolService.CastAudio(jumpSoundData);
    }

    private void WeaponChangeSoundCastAlgorithm()
    {
        if (weaponChangeNextCastAllowTime > mainSoundTime)
            return;
            
        var weaponChangeSoundData = weaponChangeSounds;
        
        weaponChangeSoundData.castParent = playerMovement.Body;
        weaponChangeSoundData.castPos = playerMovement.Body.position;
        
        var resultAudioSource = audioPoolService.CastAudio(weaponChangeSoundData);

        if(resultAudioSource == null)
            return;

        weaponChangeNextCastAllowTime =
            (resultAudioSource.clip.length * resultAudioSource.pitch / weaponChangeCooldownSmooth) + mainSoundTime;
    }

    private void WeaponShotSoundCastAlgorithm()
    {
        var targetWeaponShotSound = new AudioCastData();
        var currentWeaponId = playerWeaponsManager.selectedWeaponData.Id;
        
        switch (currentWeaponId)
        {
            case 1:
            {
                targetWeaponShotSound = fistsAttackSounds;
                break;
            }
            
            case 2:
            {
                targetWeaponShotSound = pistolShotSounds;
                break;
            }
            
            case 3:
            {
                targetWeaponShotSound = shotGunShotSounds;
                break;
            }
            
            case 4:
            {
                targetWeaponShotSound = machineGunShotSounds;
                break;
            }
            
            case 5:
            {
                targetWeaponShotSound = grenadeLauncherShotSounds;
                break;
            }
            
            case 6:
            {
                targetWeaponShotSound = rocketLauncherShotSounds;
                break;
            }
            
            case 7:
            {
                targetWeaponShotSound = superGunShotSounds;
                break;
            }
            
            case 8:{
                if (laserGunShotSoundSource == null)
                    targetWeaponShotSound = laserGunShotSounds;
                
                break;
            }
            
            case 9:
            {
                targetWeaponShotSound = bpjCollapserShotSounds;
                break;
            }
        }
        
        if(targetWeaponShotSound.Clips == null)
            return;

        if (currentShotPreparingSource != null)
        {
            currentShotPreparingSource.Stop();
            currentShotPreparingSource = null;
        }

        targetWeaponShotSound.castParent = playerWeaponsManager.shootingPoint;
        targetWeaponShotSound.castPos = playerWeaponsManager.shootingPoint.position;

        var castedAudio = audioPoolService.CastAudio(targetWeaponShotSound);

        SpecialLaserSoundAlgorithm();
        void SpecialLaserSoundAlgorithm()
        {
            if (currentWeaponId == 8)
            {
                if (laserGunShotSoundSource == null)
                {
                    var laserStartSoundData = laserGunShotStartSounds;
                    laserStartSoundData.castPos = playerMovement.Body.position;

                    audioPoolService.CastAudio(laserStartSoundData);
                }

                laserGunShotSoundSource = castedAudio;
            }
        }
    }

    private void WeaponShotPreparingCastAlgorithm()
    {
        var currentWeaponId = playerWeaponsManager.selectedWeaponData.Id;
        var weaponShotPointT = playerWeaponsManager.shootingPoint;
        var preparingShotSoundData = new AudioCastData();

        switch (currentWeaponId)
        {
            case 9:
            {
                preparingShotSoundData = bpjCollapserShotPreparingSounds;
                break;
            }
        }
        
        if(preparingShotSoundData.Clips == null)
            return;
        
        preparingShotSoundData.castParent = weaponShotPointT;
        preparingShotSoundData.castPos = weaponShotPointT.position;

        currentShotPreparingSource = audioPoolService.CastAudio(preparingShotSoundData);
    }

    private void DashUseSoundCastAlgorithm()
    {
        var dashSoundData = this.dashSoundData;

        dashSoundData.castPos = playerMovement.Body.position;
        dashSoundData.castParent = playerMovement.Body;

        audioPoolService.CastAudio(dashSoundData);
    }

    private void HookUseSoundCastAlgorithm()
    {
        var hookUseSoundData = this.hookUseSoundData;
        hookUseSoundData.castParent = playerMovement.Body;
        hookUseSoundData.castPos = playerMovement.Body.position;

        audioPoolService.CastAudio(hookUseSoundData);
        
        if(!playerMainService.hookService.IsHookUsed)
            return;
        
        StartCoroutine(HookHookedSoundCast());

        IEnumerator HookHookedSoundCast()
        {
            yield return new WaitForSeconds(hookHookedSoundDelay);
            var playerHookService = playerMainService.hookService;
            
            if (!playerHookService.IsHookUsed)
                yield break;

            var hookRopeSoundData = this.hookRopeSoundData;
            hookRopeSoundData.castParent = playerHookService.HookMeshT;
            hookRopeSoundData.castPos = playerHookService.HookMeshT.position;
        
            hookRopeSoundSource = audioPoolService.CastAudio(hookRopeSoundData);
            
            var hookHookedSoundData = this.hookHookedSoundData;
            hookHookedSoundData.castParent = playerHookService.HookMeshT;
            hookHookedSoundData.castPos = playerHookService.HookMeshT.position;

            audioPoolService.CastAudio(hookHookedSoundData);
        }
        
    }

    private void HookRopeSoundStop()
    {
        if(hookRopeSoundSource == null)
            return;
            
        hookRopeSoundSource.Stop();
        hookRopeSoundSource = null;
    }
    
    public void SetNewWalkZone(PlayerWalkZone newWalkZone)
    {
        playerWalkZone = newWalkZone;
    }
    public enum PlayerWalkZone
    {
        HardMat,
        Grass,
        Water
    }
}
