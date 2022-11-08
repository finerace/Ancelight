using System.Collections;
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
    [SerializeField] private LayerMask groundLayerMask;
    [Space]

    [SerializeField] private float OnGroundRbDrag = 8f;
    [SerializeField] private float OnFlyRbDrag = 0.75f;

    [SerializeField] private bool IsFlies = false;
    [SerializeField] private bool isSprint = false;
    
    [Space] 
    [SerializeField] private Transform cameraT;
    private float defaultCameraYpos;
    [SerializeField] private float cameraHeight;
    [SerializeField] private CapsuleCollider MainCollider;
    [SerializeField] private CapsuleCollider LegsCollider;

    private float defaultMainColliderHeight;
    private float defaultLegsColliderHeight;

    private float defaultMainColliderYcenterValue;
    private float defaultLegsColliderYcenterValue;
    
    [Range(0.05f,1f)] [SerializeField] private float crouchPlayerHeight = 0.65f;
    [SerializeField] private float crouchSpeedMultiplier = 0.4f;
    [SerializeField] private bool isPlayerCrouch = false;
    
    [Space]
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
        //?????????? ??????????? ?????
        SetCollidersParams();
        defaultCameraYpos = cameraT.localPosition.y;
        
        playerRb = GetComponent<Rigidbody>();
        playerT = transform;

        float flyTestTime = 0.1f;
        IsFlyTest(flyTestTime);

        void SetCollidersParams()
        {
            defaultMainColliderHeight = MainCollider.height;
            defaultMainColliderYcenterValue = MainCollider.center.y;

            defaultLegsColliderHeight = LegsCollider.height;
            defaultLegsColliderYcenterValue = LegsCollider.center.y;
        }
    }

    private void FixedUpdate()
    {
        if (!isManageActive)
            return;

        MovementAlgorithm();
    }

    private void Update()
    {
        if (!isManageActive)
            return;

        CrouchAlgorithm();
    }
    
    void OnGUI()
    {
        float fps = 1.0f / Time.unscaledDeltaTime;
        GUILayout.Label("FPS: " + (int)fps);
    }

    private void MovementAlgorithm()
    {
        if(!isFlies)
            GroundNormalUpdate();
        
        float horizontal = Axis.Horizontal;
        float vertical = Axis.Vertical;
        float jump = Axis.Jump;
        float maxSpeedTemp;
        isSprint = false;

        Vector3 resultDirection = Vector3.zero;

        //is Walked
        if (horizontal != 0 || vertical != 0)
            isWalked = true;
        else
            isWalked = false;

        //??? ?????????? ??????? ???????? ?????? ???????????
        resultDirection += playerT.forward * vertical;
        resultDirection += playerT.right * horizontal;

        currentMovementDirection = resultDirection;

        //??????? ?????? ???????? ???????
        float additionalSpeedBoost = 80f;
        resultDirection *= speed * additionalSpeedBoost;

        //??????? ??????? ? ?????? ??????? ???? ?????? ?????
        float speedMultiply = 0.75f;
        if (horizontal != 0 && vertical != 0)
            resultDirection *= speedMultiply;

        //????????? ?????????? ??????? ?? ????????? ???????
        if (isSprint)
            resultDirection *= SprintSpeedMultiply;

        if (isPlayerCrouch)
            resultDirection *= crouchSpeedMultiplier;

        //?????????? ??????? ? ?????? ??????????? ??????? ???????? ????? ???????????
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

        //?????????? ??????
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
        
        //???????? ??????????? ???????? ? ?????????? ????
        if ((playerRb.velocity + (resultDirection / 80f)).LengthXZ() <= maxSpeedTemp)
            playerRb.AddForce(resultDirection, ForceMode.Acceleration);
        /*else if (isWalked)
        {
            Vector3 walkDirection = resultDirection / 4f;
            walkDirection.y = 0f;

            rigidbody_.AddForce(walkDirection, ForceMode.Acceleration);
        }*/

        

    }

    //???????? ??????? ? ?????????? ??????? ??????????? ?? ??????? ????? ?????

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

    // private void OnCollisionEnter(Collision collision)
    // {
    //
    //     if(collision.contacts[0].normal.y > 0.25f && collision.gameObject.layer == 0)
    //         GroundNormal = collision.contacts[0].normal;
    // }

    private void CrouchAlgorithm()
    {
        var crouchActive = Input.GetKey(KeyCode.C);

        if (crouchActive && !isPlayerCrouch)
        {
            StartCrouch();            
        }
        else if (!crouchActive && isPlayerCrouch)
        {
            EndCrouch();
        }

        void StartCrouch()
        {
            isPlayerCrouch = true;

            var newCameraLocalPos = cameraT.localPosition;
            newCameraLocalPos.y = defaultCameraYpos - (cameraHeight * crouchPlayerHeight);
            cameraT.localPosition = newCameraLocalPos;
            
            SetCapsuleColliderToCrouch(ref MainCollider,defaultMainColliderHeight,defaultMainColliderYcenterValue);
            SetCapsuleColliderToCrouch(ref LegsCollider,defaultLegsColliderHeight,defaultLegsColliderYcenterValue);
            
            void SetCapsuleColliderToCrouch(ref CapsuleCollider targetCollider, float defaultCollideHeight
                ,float defaultColliderYcenterValue)
            {
                var resultColliderHeight = defaultCollideHeight * crouchPlayerHeight;
                var resultColliderYcenterValue = 
                    -((defaultCollideHeight - resultColliderHeight) / 2) + defaultColliderYcenterValue;

                targetCollider.height = resultColliderHeight;

                var newCenter = targetCollider.center;
                newCenter.y = resultColliderYcenterValue;
                targetCollider.center = newCenter;
            }
        }

        void EndCrouch()
        {
            if(EndCrouchIsPossible() == false)
                return;
                
            isPlayerCrouch = false;
            
            var newCameraLocalPos = cameraT.localPosition;
            newCameraLocalPos.y = defaultCameraYpos;
            cameraT.localPosition = newCameraLocalPos;
            
            SetCapsuleColliderToDefault(ref MainCollider,defaultMainColliderHeight,defaultMainColliderYcenterValue);
            SetCapsuleColliderToDefault(ref LegsCollider,defaultLegsColliderHeight,defaultLegsColliderYcenterValue);
            
            void SetCapsuleColliderToDefault(ref CapsuleCollider targetCollider, float defaultCollideHeight
                ,float defaultColliderYcenterValue)
            {
                targetCollider.height = defaultCollideHeight;

                var newCenter = targetCollider.center;
                newCenter.y = defaultColliderYcenterValue;
                targetCollider.center = newCenter;
            }

            bool EndCrouchIsPossible()
            {
                const float rayDistanceSmooth = 0.25f;
                
                var checkRayDirection = Vector3.up;
                var checkRayOrigin = cameraT.position;
                var checkRayDistance = Mathf.Abs(cameraT.localPosition.y - defaultCameraYpos) + rayDistanceSmooth;

                var checkRoofRay = new Ray(checkRayOrigin, checkRayDirection);

                return !Physics.Raycast(checkRoofRay, checkRayDistance, groundLayerMask);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Vector3 defaultNormal = new Vector3(0f,1f,0f);
        //GroundNormal = defaultNormal;

        IsFlies = true;
        playerRb.drag = OnFlyRbDrag;

    }

    private void GroundNormalUpdate()
    {
        var resultNormal = Vector3.up;
            
        var rayOriginPoint = playerT.position;
        var ratDirection = Vector3.down;
        
        var rayCheckLength = 1.25f; 
        
        var checkNormalRay = new Ray(rayOriginPoint, ratDirection);

        var isRayHit = Physics.Raycast(checkNormalRay, out RaycastHit hitInfo, rayCheckLength, groundLayerMask);

        if (isRayHit)
            resultNormal = hitInfo.normal;

        GroundNormal = resultNormal;
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
    
    public static float ClampToTwoRemainingCharacters(this float target)
    {
        return (int)(target * 100f) / 100f;
    }
    
}
