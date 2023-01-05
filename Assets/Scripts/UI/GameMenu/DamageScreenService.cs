using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageScreenService : MonoBehaviour
{

    [SerializeField] private PlayerMainService playerMain;
    [SerializeField] private Image damageScreen;
    [SerializeField] private float screenDisappearSpeed;

    private float damageScreenIntensity { get => damageScreen.color.a; }

    private void Awake()
    {
        playerMain = FindObjectOfType<PlayerMainService>();
        
        SetDamageScreenIntensity(0);

        playerMain.GetDamageEvent += DamageScreenIntensityTrackerAndSetter;
    }

    private void Update()
    {
        DamageScreenSmoothnesDisappear();
    }

    private void DamageScreenIntensityTrackerAndSetter(float damage)
    {
        const float smooth = 0.05f;
        float resultScreenIntensity = (damage * smooth) + damageScreenIntensity;

        SetDamageScreenIntensity(resultScreenIntensity);
    }

    private void SetDamageScreenIntensity(float intensity)
    {
        if (intensity > 1)
            intensity = 1;

        Color newDamageScreenColor = damageScreen.color;

        newDamageScreenColor.a = intensity;

        damageScreen.color = newDamageScreenColor;
    }

    private void DamageScreenSmoothnesDisappear()
    {
        float timeStep = Time.deltaTime * screenDisappearSpeed;

        float resultScreenIntensity = 
            Mathf.Lerp(damageScreenIntensity, 0,timeStep);

        SetDamageScreenIntensity(resultScreenIntensity);
    }

}
