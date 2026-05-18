using UnityEngine;

public class Animation : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;       
    public float runSpeed = 10f;      
    public float gravity = -9.8f;      
    
    private CharacterController characterController;
    private Vector3 velocity;

    [Header("Animation Settings")]
    private Animator animator;
    private int isWalkingHash;
    private int isRunningHash;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");   
        
        bool movementPressed = (horizontal != 0 || vertical != 0);
        
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = (runPressed && movementPressed) ? runSpeed : walkSpeed;

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
        
        moveDirection = transform.TransformDirection(moveDirection); 
        
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        HandleAnimations(movementPressed, runPressed);
    }

    void HandleAnimations(bool movementPressed, bool runPressed)
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (!isWalking && movementPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !movementPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isRunning && (movementPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        if (isRunning && (!movementPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}