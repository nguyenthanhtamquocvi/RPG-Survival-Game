using UnityEngine;

public class PlayerManger : MonoBehaviour
{
    #region Singleton

    public static PlayerManger instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion Singleton

    private Animator anim;

    [HideInInspector]
    private PlayerLocomotion playerLocomotion;

    private InputHandler inputHandler;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isGrounded;
    public bool isSprinting;
    public bool isInAir;
    public bool canDoCombo;
    public bool stopRotate;
    public bool isHitItem = false;

    private void Start()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        anim = GetComponentInChildren<Animator>();
        inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        inputHandler.TickInput();
        isSprinting = inputHandler.b_input;
        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        stopRotate = anim.GetBool("stopRotate");
        CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
        if (!stopRotate)
        {
            playerLocomotion.HandleRotation();
        }
        playerLocomotion.HandleMovement();
        playerLocomotion.HandleRolling();
        playerLocomotion.HandleFalling(playerLocomotion.moveDirection);

        inputHandler.d_input = false;
        inputHandler.i_input = false;
        inputHandler.q_input = false;
        inputHandler.a_input = false;
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.comboFlag = false;
    }

    private void LateUpdate()
    {
        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }
    public void CheckForInteractableObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2))
        {
            if (hit.collider.tag == "Interactable")
            {
                isHitItem = true;
                Debug.Log("You in");
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject != null)
                {
       
                    if (inputHandler.i_input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            isHitItem = false;
          
    
        }
    }
}