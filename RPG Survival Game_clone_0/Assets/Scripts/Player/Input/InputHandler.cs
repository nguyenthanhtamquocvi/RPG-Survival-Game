using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;
    [HideInInspector] public float moveAmount;

    private PlayerControls inputActions;
    private PlayerAttacker playerAttacker;
    private PlayerLocomotion playerLocomotion;
    private PlayerManger playerManager;
    private AnimationHandler animationHandler;

    private Vector2 moveInput;
    public bool d_input;
    public bool q_input;
    public bool i_input;
    public bool a_input;
    public bool b_input;
    public bool sprintFlag;
    public bool comboFlag;
    public bool rollFlag;
    public float rollInputTimer;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManger>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animationHandler = GetComponentInChildren<AnimationHandler>();
        animationHandler.Initialaze();
        instance = this;
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.Player.Movement.performed += i => moveInput = i.ReadValue<Vector2>();
            inputActions.Player.Drop.performed += i => d_input = true;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput()
    {
        HandleMovementInput();
        HandleRollInput();
        HandleAttackInput();
        HandleInteractingInput();
    }

    private void HandleMovementInput()
    {
        horizontal = moveInput.x;
        vertical = moveInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        animationHandler.UpdateAnimatorValues(moveAmount, 0, playerManager.isSprinting);
    }

    private void HandleRollInput()
    {
        b_input = inputActions.Player.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
        if (b_input)
        {
            rollInputTimer += Time.deltaTime;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    public void HandleAttackInput()
    {
        inputActions.Player.Attack.performed += i => a_input = true;

        if (a_input)
        {
            playerManager.stopRotate = true;
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(PlayerInventory.instance.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;
                if (playerManager.canDoCombo)
                    return;

                playerAttacker.HandleAttack(PlayerInventory.instance.rightWeapon);
            }

        }
        else
        {
            playerManager.stopRotate = false;
        }
    }

    private void HandleInteractingInput()
    {
        inputActions.Player.Interacting.performed += i => i_input = true;
    }

}