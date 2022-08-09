using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBotWizardEffects : DefaultBotEffects
{
    [SerializeField] private ParticleSystem preShootParticls;

    internal void PlayPreShootParticls()
    {
        preShootParticls.Play();
    }



}
