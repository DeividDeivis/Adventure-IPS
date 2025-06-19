using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator animator;

    [Header("Move System")]
    [SerializeField] private float moveSpeed = 3;

    [Header("Attack System")]
    [SerializeField] private float attackTime = 1;   
    [SerializeField] private GameObject hitCollider;
    private bool canAttack = true;

    private InputSystem_Actions _Inputs;
    
    void Awake()
    {
        _Inputs = new InputSystem_Actions();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 moveVector = _Inputs.Player.Move.ReadValue<Vector2>();
        //transform.position += new Vector3(moveVector.x, moveVector.y, 0) * moveSpeed * Time.deltaTime;

        // Move Input
        Vector2 moveVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position += new Vector3(moveVector.x, moveVector.y, 0) * moveSpeed * Time.deltaTime;

        // Sprite flip
        if(moveVector.x < 0)
            _sprite.flipX = true;
        else if (moveVector.x > 0)
            _sprite.flipX = false;

        // Animator parameter update
        bool moving = moveVector.x != 0 || moveVector.y != 0 ? true : false;
        animator.SetBool("InMove", moving);

        // Attack Input
        if (Input.GetMouseButtonDown(0) && canAttack) 
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
}
