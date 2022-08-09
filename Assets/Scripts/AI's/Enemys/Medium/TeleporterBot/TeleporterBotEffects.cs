using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBotEffects : DefaultBotEffects
{

    [SerializeField] private ParticleSystem teleportParticle;
    private Transform teleportParticleT;

    private new void Start()
    {
        base.Start();

        teleportParticleT = teleportParticle.transform;
    }

    public void PlayTeleportEffect(Vector3 point)
    {
        teleportParticleT.position = point;

        teleportParticle.Play();
    }

}
