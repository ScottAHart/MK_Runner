using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageEvent : UnityEvent<int>{}
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public enum JumpState
    {
        Ground,
        Jump,
        Double
    }
    Rigidbody2D rigidbody;
    Animator animator;
    BoxCollider2D collider;
    SpriteRenderer renderer;
    public JumpState jump = JumpState.Double;
    bool jumpInput = false;
    float jumpForce = 5;
    float moveSpeed = 10;
    LayerMask groundMask;
    [SerializeField]
    Transform target;

    UnityEvent dieEvent;
    UnityEvent coinEvent;
    DamageEvent damageEvent;

    //DamageVars
    float immuneTimeAfterHit = 1.0f;
    float flickerTime = 0.25f;
    float lastTimeHit = 0.0f;
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<SpriteRenderer>();

        dieEvent = new UnityEvent();
        coinEvent = new UnityEvent();
        damageEvent = new DamageEvent();
    }

    private void Start()
    {
        animator.SetBool("Running", true);
        groundMask = LayerMask.GetMask("Ground");
    }

    public void SetUp(InGame gameMode)
    {
        coinEvent.AddListener(gameMode.CoinCollected);
        dieEvent.AddListener(gameMode.GameOver);
        damageEvent.AddListener(gameMode.DamageTaken);
        target = gameMode.PlayerTarget.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
        Vector2 playerPos = Camera.main.WorldToScreenPoint(transform.position + collider.bounds.extents * 1.2f); //Bound extents scaled and added to get top right of player position on the screen so theyre completely off the screen
        if (playerPos.x < 0 || playerPos.y < 0) Die(); 
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rigidbody.velocity;
        //Grounded check
        bool grounded = false;
        Vector3[] pos = new Vector3[]{
        this.transform.position + Vector3.left * collider.bounds.extents.x + Vector3.down * collider.bounds.extents.y + Vector3.down * collider.edgeRadius, //Position at bottom of collider
        this.transform.position + Vector3.right * collider.bounds.extents.x + Vector3.down * collider.bounds.extents.y + Vector3.down * collider.edgeRadius, //Position at bottom of collider
        };
        foreach (var p in pos)
            if (Physics2D.OverlapCircle(p, 0.05f, groundMask)) grounded = true;
        if (Mathf.Abs(rigidbody.velocity.y) < 0.01 && grounded)
        {
            jump = JumpState.Ground;
            animator.SetBool("Jumping", false);
        }
        else if (jump == JumpState.Ground)
        {
            jump = JumpState.Jump;
            animator.SetBool("Jumping", true);
        }
        //Move to target
        if (Mathf.Abs(transform.position.x - target.position.x) > 0.5f)
            velocity.x += Mathf.Sign(target.position.x - rigidbody.position.x) * moveSpeed * Time.fixedDeltaTime;
        //transform.position += (new Vector3((target.position.x - rigidbody.position.x), 0, 0).normalized * moveSpeed * Time.fixedDeltaTime);
        velocity.x = Mathf.Clamp(velocity.x, -2, 2);
        rigidbody.velocity = velocity;
        //Jump check
        if (jumpInput)
        {
            if (grounded && jump == JumpState.Ground)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jump = JumpState.Jump;
                animator.SetBool("Jumping", true);

            }
            else if (jump == JumpState.Jump)
            {
                rigidbody.velocity  = new Vector2(rigidbody.velocity.x, 0); //Stop downward momentum to give nicer feeling jump
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jump = JumpState.Double;
            }
            jumpInput = false;
        }

    }
    public void Coin()
    {
        coinEvent.Invoke();
    }
    public void Die()
    {
        dieEvent.Invoke();
        this.enabled = false;
    }
    /// Moves the player closer to the left of the screen 
    public void TakeDamage(int amount)
    {
        if (Time.time - lastTimeHit > immuneTimeAfterHit) {
            lastTimeHit = Time.time;
            damageEvent.Invoke(amount);
            //animator.SetTrigger("Damage");
            StartCoroutine(TakeDamage(immuneTimeAfterHit, flickerTime));
        }
    }

    IEnumerator TakeDamage(float duration, float cycleTime)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.PingPong(timer / cycleTime, 1));

            yield return null;
        }
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
    }
}
