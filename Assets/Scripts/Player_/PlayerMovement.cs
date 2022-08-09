using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 1f;

    [HideInInspector] public float Speed
    {
        get { return speed; }
    }

    [HideInInspector]
    public bool isFlies
    {
        get { return IsFlies; }
    }

    [HideInInspector]
    public bool IsSprint
    {
        get { return isSprint; }
    }

    [SerializeField] private float jumpForce = 1f;
    private Rigidbody playerRb;
    public Rigidbody PlayerRb { get { return playerRb; } }

    private Transform playerT;

    public Transform Body { get { return playerT ; } }

    [Space]
    [SerializeField] private float FallingSpeed = 1f;
    [SerializeField] private float SprintSpeedMultiply = 0.5f;
    [SerializeField] private float OnFlySpeedMultiply = 0.15f;
    [SerializeField] private float OnGroundMaxSpeed = 15f;
    [SerializeField] private float OnFlyMaxSpeed = 15f;
    [Space]

    [SerializeField] private float OnGroundRbDrag = 8f;
    [SerializeField] private float OnFlyRbDrag = 0.75f;

    [SerializeField] private bool IsFlies = false;
    [SerializeField] private bool isSprint = false;
    [Space]

    private Collider PlayerCollider;
    [SerializeField] private Vector3 GroundNormal;
    private bool IsJumpingButtonPressed = false;

    private bool isManageActive = true;

    private bool isWalked = false;
    public bool IsWalked { get { return isWalked; } }

    private Vector3 currentMovementDirection;
    public Vector3 CurrentMovementDirection
        { get { return currentMovementDirection; } }
    

    private void Start()
    {
        //Назначение необходимых полей
        if (PlayerCollider == null) PlayerCollider = GetComponent<Collider>();
        playerRb = GetComponent<Rigidbody>();
        playerT = transform;

        float flyTestTime = 0.1f;
        IsFlyTest(flyTestTime);
    }

    private void FixedUpdate()
    {
        if (!isManageActive)
            return;

        MovementAlgorithm();
    }

    void OnGUI()
    {
        float fps = 1.0f / Time.unscaledDeltaTime;
        GUILayout.Label("FPS: " + (int)fps);
    }

    private void MovementAlgorithm()
    {
        //Инициализация вспомогательных переменных/полей
        float horizontal = Axes.Horizontal;
        float vertical = Axes.Vertical;
        float jump = Axes.Jump;
        float maxSpeedTemp;
        isSprint = false; //Input.GetKey(KeyCode.LeftShift);

        Vector3 resultDirection = Vector3.zero;

        //is Walked
        if (horizontal != 0 || vertical != 0)
            isWalked = true;
        else
            isWalked = false;

        //Даёт финальному вектору движения нужное направление
        resultDirection += playerT.forward * vertical;
        resultDirection += playerT.right * horizontal;

        currentMovementDirection = resultDirection;

        //Придача нужной скорости вектору
        float additionalSpeedBoost = 80f;
        resultDirection *= speed * additionalSpeedBoost;

        //Деление вектора в случае нажатия двух кнопок сразу
        float speedMultiply = 0.75f;
        if (horizontal != 0 && vertical != 0)
            resultDirection *= speedMultiply;

        //Умножение финального вектора на множитель спринта
        if (isSprint)
            resultDirection *= SprintSpeedMultiply;

        //Реализация падения и расчёт правильного вектора движения вдоль поверхности
        if (IsFlies)
        {
            maxSpeedTemp = OnFlyMaxSpeed;
            resultDirection *= OnFlySpeedMultiply;

            float defaultFallingSpeed = -0.1f;
            playerRb.velocity += new Vector3(0, defaultFallingSpeed, 0) * FallingSpeed;
            playerRb.useGravity = true;
        }
        else
        {
            maxSpeedTemp = OnGroundMaxSpeed;

            float Dot;
            Dot = Vector3.Dot(resultDirection, GroundNormal);
            Vector3 dotDirection = Dot * GroundNormal;

            dotDirection = resultDirection - dotDirection;
            resultDirection = dotDirection;
            playerRb.useGravity = false;
        }

        //Реализация прыжка
        if (!IsFlies && jump > 0 && IsJumpingButtonPressed == false)
        {
            float additionalJumpBoost = 750f;
            resultDirection += playerT.up * jumpForce * additionalJumpBoost;

            Vector3 rbVelocity = playerRb.velocity;
            playerRb.velocity =
                new Vector3(rbVelocity.x, 0, rbVelocity.z);

            playerT.position += new Vector3(0, 0.1f, 0);

            IsFlies = true;
            IsJumpingButtonPressed = true;
            
        }
        else if (jump == 0 && !IsFlies)
            IsJumpingButtonPressed = false;
        
        //Проверка ограничения скорости и применение силы
        if ((playerRb.velocity + (resultDirection / 80f)).LengthXZ() <= maxSpeedTemp)
            playerRb.AddForce(resultDirection, ForceMode.Acceleration);
        /*else if (isWalked)
        {
            Vector3 walkDirection = resultDirection / 4f;
            walkDirection.y = 0f;

            rigidbody_.AddForce(walkDirection, ForceMode.Acceleration);
        }*/

        

    }

    //Проверка падения и назначение нормали поверхности на которой стоит игрок

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        IsFlies = false;
        playerRb.drag = OnGroundRbDrag;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        IsFlies = true;
        playerRb.drag = OnFlyRbDrag;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.contacts[0].normal.y > 0.25f && collision.gameObject.layer == 0)
            GroundNormal = collision.contacts[0].normal;
    }

    private void OnCollisionExit(Collision collision)
    {
        //Vector3 defaultNormal = new Vector3(0f,1f,0f);
        //GroundNormal = defaultNormal;

        IsFlies = true;
        playerRb.drag = OnFlyRbDrag;

    }

    /*private Vector3 MultiplyXZ(Vector3 vec,float mult)
    {
        vec = new Vector3(vec.x * mult, vec.y, vec.z * mult);

        return vec;
    }
    */

    private void IsFlyTest(float time)
    {
        StartCoroutine(FlyTest_(time));
        
        IEnumerator FlyTest_(float time)
        {
            while(true)
            {
                IsFlies = true;
                playerRb.drag = OnFlyRbDrag;

                yield return new WaitForSeconds(time);
            }
        }
    }
    
    public void SetManageActive(bool state)
    {
        isManageActive = state;
    }

}

public static class AuxiliaryFunc
{
    
    public static float LengthXZ(this Vector3 b)
    {
        float returnFloat = Mathf.Abs(b.x)+ Mathf.Abs(b.z);
        return returnFloat;
    }

    public static float LengthY(this Vector3 b)
    {
        float returnFloat = Mathf.Abs(b.y);
        return returnFloat;
    }

    public static bool IsLayerInMask(this LayerMask mask, int layer)
    {
        int maskValue = mask.value;
        int layerValue = 1 << layer;

        if (maskValue < layerValue)
            return false;

        if (maskValue == layerValue)
            return maskValue == layerValue;

        int dynamicMaskValue = maskValue;

        for (int i = 30; i >= 0; i--)
        {
            int localMaskNum = 1 << i;

            if (localMaskNum > maskValue)
                continue;

            if (dynamicMaskValue == layerValue)
                return true;

            if (((layerValue * 2) - 1) >= dynamicMaskValue && layerValue <= dynamicMaskValue)
                return true;

            if ((dynamicMaskValue - localMaskNum) < 0)
                continue;

            
            dynamicMaskValue -= localMaskNum;

            if (((layerValue * 2) - 1) >= dynamicMaskValue && layerValue <= dynamicMaskValue)
                return true;
        }

        return false;

    }

    public static float PointDirection_TargetLocalPosDOT(Vector3 targetLocalPos, Vector3 pointDirection)
    {
        return Vector3.Dot(pointDirection.normalized, targetLocalPos.normalized);
    }

}
