using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsMove : MonoBehaviour
{
    [SerializeField] private float handsMoveSpeed = 5;
    [SerializeField] private float handsMovePower = 1;
    [Space]
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform playerHands;
    [SerializeField] private Rigidbody playerRB;

    private Vector3 startPlayerHandsPos;

    private void Start()
    {
        startPlayerHandsPos = playerHands.localPosition;
    }

    private void Update()
    {
        float timeStepHands = Time.deltaTime * handsMoveSpeed;

        Vector3 playerVelocityTrue = RotateRBvelocity();

        Vector3 targetPlayerHandsPos = (-playerVelocityTrue * handsMovePower) + startPlayerHandsPos;

        playerHands.localPosition = Vector3.Lerp(playerHands.localPosition, targetPlayerHandsPos, timeStepHands);

    }

    private Vector3 RotateRBvelocity()
    {
        Vector3 resultVector = new Vector3();

        Vector2 forward = new Vector2(playerCam.forward.x,playerCam.forward.z);

        float forwardDot = Vector2.Dot(new Vector2(0, 1), forward);
        float rightDot = Vector2.Dot(new Vector2(1,0), forward);

        resultVector.z += playerRB.velocity.z * forwardDot;
        resultVector.z += playerRB.velocity.x * rightDot;

        resultVector.x += playerRB.velocity.z * -rightDot;
        resultVector.x += playerRB.velocity.x * forwardDot;

        resultVector.y = playerRB.velocity.y;

        return resultVector;
    }

}
