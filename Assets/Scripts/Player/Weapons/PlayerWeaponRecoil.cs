using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerWeaponRecoil
{

    private Transform playerCameraT;
    private Transform playerBodyT;

    private float recoilForce = 0.5f;
    private int recoilFrameTime = 20;

    private Vector2 recoilVector = Vector2.up;
    public bool isRecoilOn = true;

    public PlayerWeaponRecoil(Transform playerCameraT,Transform playerBodyT, float recoilForce = 5, int recoilFrameTime = 25)
    {

        if(playerCameraT == null)
            throw new System.ArgumentOutOfRangeException("Camera transform component is null!");

        if(playerBodyT == null)
            throw new System.ArgumentOutOfRangeException("Body transform component is null!");

        this.playerCameraT = playerCameraT;
        this.playerBodyT = playerBodyT;

        SetRecoilSettings(recoilForce,recoilFrameTime);

    }

    public void SetRecoilSettings(float recoilForce, int recoilFrameTime, Vector2? recoilVector = null)
    {
        if (recoilForce < 0)
            throw new System.ArgumentOutOfRangeException("Negative Recoil Force!");

        if (recoilFrameTime <= 0)
            throw new System.ArgumentOutOfRangeException("Recoil frame time is 0 or less!");

        this.recoilForce = recoilForce;
        this.recoilFrameTime = recoilFrameTime;

        if (recoilVector != null)
            this.recoilVector = (Vector2)recoilVector;

    }

    public void SetRecoilVector(Vector2 recoilVector)
    {
        if (recoilVector == null)
            throw new System.ArgumentNullException("Recoil vector is null!");

        this.recoilVector = recoilVector;

    }

    public IEnumerator GetRecoilUpdater(float currentWeaponDamage = 15f)
    {
        Vector2 resultRecoilDirection = recoilVector * recoilForce;

        int localFrameTime = recoilFrameTime;

        YieldInstruction waitFrame = new WaitForEndOfFrame();

        yield return waitFrame;

        for (int i = recoilFrameTime; i > 0; i--)
        {
            float smoothness = 25f;

            Vector2 loopCountScaledDirection = 
                (resultRecoilDirection * i * (currentWeaponDamage)) / localFrameTime / smoothness;

            AddCameraRotation(loopCountScaledDirection);

            yield return waitFrame;
        }


        void AddCameraRotation(Vector2 eulerAngles)
        {
            playerBodyT.localEulerAngles += new Vector3(0,eulerAngles.x);
            playerCameraT.localEulerAngles += new Vector3(-eulerAngles.y, 0);
        }

    }

}
