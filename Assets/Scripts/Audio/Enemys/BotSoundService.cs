using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotSoundService : MonoBehaviour
{
    private AudioPoolService audioPoolService;
    [SerializeField] private DefaultBot enemy;
    [SerializeField] private DefaultBotAttack enemyAttack;
    private Transform botT;
    
    [Space] 
    
    [SerializeField] private AudioCastData enemyDamageSoundData;

    [Space]

    [SerializeField] private AudioCastData enemyHoverSoundData;
    private AudioSource enemyHoverSoundSource;

    [Space]

    [SerializeField] private AudioCastData enemyIdleSoundData;
    [SerializeField] private AudioCastData enemyIdleDangerSoundData;
    [SerializeField] private float minNextCastTime = 3; 
    [SerializeField] private float maxNextCastTime = 16;
    private float nextTimeToIdleSound = 0;
    
    [Space]

    [SerializeField] private AudioCastData enemyWarningSoundData;
    
    [Space]

    [SerializeField] private AudioCastData enemyPreShotSoundData;
    [SerializeField] private AudioCastData enemyShotSoundData;
    private AudioSource loopedShotSound;
    
    [SerializeField] private AudioCastData enemyMeleeAttackSoundData;
    
    [Space]
    
    [SerializeField] private AudioCastData enemyDieSoundData;

    private void Start()
    {
        SetEmptyFields();
        void SetEmptyFields()
        {
            audioPoolService = AudioPoolService.currentAudioPoolService;

            if (enemy == null)
                enemy = GetComponent<DefaultBot>();

            if (enemyAttack == null)
                enemyAttack = GetComponent<DefaultBotAttack>();

            botT = enemy.transform;
        }
        
        SetSoundCastAlgorithmsToEvents();
        void SetSoundCastAlgorithmsToEvents()
        {
            enemy.OnDie += HoverSoundStop;
            enemy.OnDie += DieSoundCast;
            enemy.OnDamageEvent += DamageSoundCast;
            enemy.SubPlayerLookEvent(WarningSoundCast);
            
            enemyAttack.SubPreShotEvent(PreShotSoundCast);
            enemyAttack.SubShotEvent(ShotSoundCast);
            enemyAttack.SubLoopShotStopEvent(StopLoopShooting);
            enemyAttack.SubMeleeAttackEvent(MeleeAttackSoundCast);
        }
        
        HoverSoundCast();
        StartCoroutine(IdleSoundsCastUpdater());
    }

    private void DamageSoundCast(float damage)
    {
        var damageSoundData = enemyDamageSoundData;

        damageSoundData.castParent = botT;
        damageSoundData.castPos = botT.position;

        audioPoolService.CastAudio(damageSoundData);
    }

    private void HoverSoundCast()
    {
        var hoverSoundData = enemyHoverSoundData;

        hoverSoundData.castParent = botT;
        hoverSoundData.castPos = botT.position;

        enemyHoverSoundSource = audioPoolService.CastAudio(hoverSoundData);
    }

    private void HoverSoundStop()
    {
        if(enemyHoverSoundSource == null)
            return;
        
        enemyHoverSoundSource.Stop();
    }

    private IEnumerator IdleSoundsCastUpdater()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetNextCastDelay());
            
            IdleSoundsCast();
        }

        float GetNextCastDelay()
        {
            var nextTime = Random.Range(minNextCastTime, maxNextCastTime);

            return nextTime;
        }
    }
    
    private void IdleSoundsCast()
    {
        var audioCastData = new AudioCastData();
        var isEnemyAnnoyed = enemy.isAnnoyed;

        if (isEnemyAnnoyed)
            audioCastData = enemyIdleDangerSoundData;
        else
            audioCastData = enemyIdleSoundData;

        audioCastData.castParent = botT;
        audioCastData.castPos = botT.position;

        audioPoolService.CastAudio(audioCastData);
    }

    private void WarningSoundCast()
    {
        var warningSoundCastData = enemyWarningSoundData;

        warningSoundCastData.castParent = botT;
        warningSoundCastData.castPos = botT.position;

        audioPoolService.CastAudio(warningSoundCastData);
    }
    
    private void PreShotSoundCast()
    {
        var preShotSoundData = enemyPreShotSoundData;

        preShotSoundData.castParent = botT;
        preShotSoundData.castPos = botT.position;

        audioPoolService.CastAudio(preShotSoundData);
    }

    private void ShotSoundCast()
    {
        var shotSoundCast = enemyShotSoundData;

        shotSoundCast.castParent = botT;
        shotSoundCast.castPos = botT.position;

        var audioSource = audioPoolService.CastAudio(shotSoundCast);

        if (shotSoundCast.IsLoop)
            loopedShotSound = audioSource;
    }

    private void MeleeAttackSoundCast()
    {
        var meleeAttackSound = enemyMeleeAttackSoundData;

        meleeAttackSound.castParent = botT;
        meleeAttackSound.castPos = botT.position;

        audioPoolService.CastAudio(meleeAttackSound);
    }

    private void DieSoundCast()
    {
        var dieSoundData = enemyDieSoundData;

        dieSoundData.castPos = botT.position;

        audioPoolService.CastAudio(dieSoundData);
    }

    private void StopLoopShooting()
    {
        loopedShotSound.Stop();
    }
    
    
}
