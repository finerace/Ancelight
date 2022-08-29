using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAim : MonoBehaviour
{

    [SerializeField] private PlayerLookService playerLook;
    [SerializeField] private RectTransform aimRT;
    [SerializeField] private float smoothneess = 2;


    private void FixedUpdate()
    {
        Vector3 point = playerLook.GetShootingRayHit().point;

        if (point == Vector3.zero)
            point = playerLook.ShootingPoint.position + (playerLook.ShootingPoint.forward * 1000f);

        Vector2 postPos = playerLook.mainCamera.WorldToScreenPoint(point);

        postPos = new Vector2(postPos.x - (playerLook.mainCamera.pixelWidth / 2),
            postPos.y - (playerLook.mainCamera.pixelHeight / 2));

        aimRT.localPosition = Vector3.Lerp
                (aimRT.localPosition, postPos, Time.deltaTime * smoothneess);
    }

}
