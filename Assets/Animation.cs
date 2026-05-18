using UnityEngine;

public class Animation : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;       // سرعة المشي الطبيعية
    public float runSpeed = 10f;       // سرعة الركض عند ضغط Shift
    public float gravity = -9.8f;      // قوة الجاذبية
    
    private CharacterController characterController;
    private Vector3 velocity;

    [Header("Animation Settings")]
    private Animator animator;
    private int isWalkingHash;
    private int isRunningHash;

    void Start()
    {
        // جلب المكونات تلقائياً من اللاعب
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // تحويل النصوص إلى Hash لرفع الأداء وتقليل استهلاك المعالج
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        // 1. استقبال المدخلات من لوحة المفاتيح (WASD / الأسهم)
        float horizontal = Input.GetAxis("Horizontal"); // أزرار A/D أو الأسهم الجانبية
        float vertical = Input.GetAxis("Vertical");     // أزرار W/S أو الأسهم العلوية/السفلية
        
        // فحص هل اللاعب يضغط على أزرار الحركة فعلياً؟
        bool movementPressed = (horizontal != 0 || vertical != 0);
        
        // فحص هل اللاعب يضغط على زر الركض (Shift)؟
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        // 2. تحديد السرعة الحالية (إذا كان يركض تتحول السرعة لـ runSpeed)
        float currentSpeed = (runPressed && movementPressed) ? runSpeed : walkSpeed;

        // 3. حساب اتجاه الحركة وتطبيقه
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
        
        // تحويل الحركة لتكون متناسبة مع الاتجاه الذي ينظر إليه اللاعب (وليس اتجاه العالم الثابت)
        moveDirection = transform.TransformDirection(moveDirection); 
        
        // تحريك اللاعب بناءً على السرعة والوقت
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        // 4. نظام الجاذبية الذكي
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // تصفير الجاذبية المتراكمة عندما يلمس اللاعب الأرض منعاً للمشاكل
        }
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // 5. التحكم في أنميشن المشي والركض
        HandleAnimations(movementPressed, runPressed);
    }

    // دالة مخصصة للتحكم بالأنميشن للحفاظ على نظافة الكود وترتيبه
    void HandleAnimations(bool movementPressed, bool runPressed)
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        // منطق أنميشن المشي (isWalking)
        if (!isWalking && movementPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !movementPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        // منطق أنميشن الركض (isRunning)
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