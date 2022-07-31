using UnityEngine;
public class PlayerLocomotion : MonoBehaviour
{
    private InputHandler inputHandler;
    private AnimationHandler animationHandler;
    private PlayerManger playerManager;

    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public Transform myTrans;

    [Header("Ground && Air Detection Stats")]
    [SerializeField] private float groundDetectionRayStarPoint = 0.5f;
    [SerializeField] private float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField] private float groundDirectionRayDistance = 0.2f;
    [HideInInspector] public LayerMask ignoreLayer;
    public float inAirTimer;

    [Header("Player Movement Stats")]
    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float sprintSpeed = 10;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float fallingSpeed = 45;

    [HideInInspector] public new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animationHandler = GetComponentInChildren<AnimationHandler>();
        animationHandler.Initialaze();
        playerManager = GetComponent<PlayerManger>();
        myTrans = transform;

        playerManager.isGrounded = true;
        ignoreLayer = ~(1 << 8 | 1 << 11);
    }

    #region Movement

    private Vector3 normalVector;
    private Vector3 targetPosition;

    public void HandleRotation()
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = new Vector3(inputHandler.horizontal, 0, inputHandler.vertical);
        targetDir.Normalize();

        if (targetDir == Vector3.zero)
            targetDir = myTrans.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTrans.rotation, tr, rs * Time.deltaTime);

        myTrans.rotation = targetRotation;
    }

    public void HandleMovement()
    {
        if (playerManager.isInteracting)
            return;

        if (inputHandler.rollFlag)
            return;

        moveDirection = new Vector3(inputHandler.horizontal, 0, inputHandler.vertical);
        moveDirection.Normalize();

        float speed = movementSpeed;
        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if (inputHandler.moveAmount < 0.5f)
            {
                moveDirection *= walkingSpeed;
                playerManager.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;
    }

    public void HandleRolling()
    {
        if (animationHandler.anim.GetBool("isInteracting"))
            return;

        if (inputHandler.rollFlag)
        {
            moveDirection = new Vector3(inputHandler.horizontal, 0, inputHandler.vertical);

            if (inputHandler.moveAmount > 0)
            {
                animationHandler.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTrans.rotation = rollRotation;
            }
        }

        if (playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
            myTrans.position = Vector3.Lerp(myTrans.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            myTrans.position = targetPosition;
        }
    }

    public void HandleFalling(Vector3 moveDirection)
    {
        playerManager.isGrounded = true;
        RaycastHit hit;
        Vector3 origin = myTrans.position;
        origin.y += groundDetectionRayStarPoint;

        if (Physics.Raycast(origin, myTrans.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = myTrans.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreLayer))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + inAirTimer);
                    animationHandler.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    animationHandler.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (playerManager.isInAir == false)
            {
                if (playerManager.isInteracting == false)
                {
                    animationHandler.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isGrounded)
        {
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTrans.position = Vector3.Lerp(myTrans.position, targetPosition, Time.deltaTime);
            }
            else
            {
                myTrans.position = targetPosition;
            }
        }
    }

    #endregion Movement
}