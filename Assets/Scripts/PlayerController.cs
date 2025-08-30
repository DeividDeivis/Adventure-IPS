using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private int playerLifes = 3;
    public int _lifes => playerLifes;

    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator animator;

    [Header("Mobile Settings")]
    [SerializeField] private Joystick mobileJoystick;
    [SerializeField] private Button attackButton;
    private bool virtualAttackButton = false;

    [Header("Move System")]
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float stepsTime = .3f;
    private float currentStep = 0;
    [SerializeField] private AudioSource moveSource;

    [Header("Attack System")]
    [SerializeField] private float attackTime = 1;   
    [SerializeField] private GameObject hitCollider;
    private bool canAttack = true;

    public static Action OnPlayerHit;
    public static Action OnPlayerDead;

    private InputSystem_Actions _Inputs;
    [SerializeField] private Vector2 moveVector = Vector2.zero;

    void Awake()
    {
        _Inputs = new InputSystem_Actions();
        _Inputs.Player.Attack.started += OnAttack;
        //_Inputs.Player.Move.performed += OnMove;

        //attackButton?.onClick.AddListener(() => virtualAttackButton = true);
    }

    private void OnEnable()
    {
        _Inputs.Player.Enable();
    }

    private void OnDisable()
    {
        _Inputs.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        // Move Input
        moveVector = Vector2.zero;

        // #define se usa principalmente para definir símbolos de preprocesador para controlar la compilación condicional. 
        // Directivas de preprocesador de C#.
#if !UNITY_ANDROID
        moveVector = _Inputs.Player.Move.ReadValue<Vector2>();
#else
        moveVector = mobileJoystick?.Direction;
#endif
        transform.position += new Vector3(moveVector.x, moveVector.y, 0) * moveSpeed * Time.deltaTime;

        // Sprite flip
        if(moveVector.x < 0)
            _sprite.flipX = true;
        else if (moveVector.x > 0)
            _sprite.flipX = false;

        // Animator parameter update
        bool moving = moveVector.x != 0 || moveVector.y != 0 ? true : false;
        animator.SetBool("InMove", moving);

        if (moving)
        {
            if (currentStep > 0)
                currentStep -= Time.deltaTime;
            else 
            {
                moveSource.Play();
                currentStep = stepsTime;
            }
        }
        else
            currentStep = stepsTime;
    }

    public void OnMove(InputAction.CallbackContext ctx) 
    {
        moveVector = ctx.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx) 
    {
        if (ctx.started || virtualAttackButton && canAttack)
        {
            canAttack = false;
            StartCoroutine(waitToAttack());
            hitCollider.SetActive(true);
            animator.SetTrigger("Attacking");
        }
    }

    private IEnumerator waitToAttack() 
    {
        yield return new WaitForSeconds(attackTime);
        canAttack = true;
        FinishAttack();
    }

    public void FinishAttack() 
    {
        hitCollider.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            PlayerReceiveDamage();          
    }

    private void PlayerReceiveDamage()
    {
        playerLifes--;
        OnPlayerHit?.Invoke();

        if (playerLifes <= 0)
        {
            isAlive = false;
            OnPlayerDead?.Invoke();
        }
    }

    public void SetData(Vector2 savedPos, int savedLifes) 
    {
        transform.position = savedPos;
        playerLifes = savedLifes;
    }
}
