using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed = 1;
    private Transform playerTransform;

    void Awake()
    {
        playerTransform = FindFirstObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        Vector2 newPosition = playerTransform.position - transform.position;
        transform.position += new Vector3(newPosition.x, newPosition.y, 0) * moveSpeed * Time.deltaTime;

        // Sprite flip
        if (newPosition.x < 0)
            _sprite.flipX = true;
        else if (newPosition.x > 0)
            _sprite.flipX = false;

        // Animator parameter update
        bool moving = true;
        animator.SetBool("InMove", moving);
    }
}
