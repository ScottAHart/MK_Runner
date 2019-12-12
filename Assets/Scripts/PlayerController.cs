using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public JumpState jump = JumpState.Double;
    bool jumpInput = false;
    float jumpForce = 5;
    float moveSpeed = 10;
    LayerMask groundMask;
    [SerializeField]
    Transform target;
    // Start is called before the first frame update
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        animator.SetBool("Running", true);
        groundMask = LayerMask.GetMask("Ground");
        if (target == null) throw new System.Exception("target null");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
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
        if (grounded && Mathf.Abs(transform.position.x - target.position.x) > 0.5f)
            velocity.x += Mathf.Sign(target.position.x - rigidbody.position.x) * moveSpeed * Time.fixedDeltaTime;
        //transform.position += (new Vector3((target.position.x - rigidbody.position.x), 0, 0).normalized * moveSpeed * Time.fixedDeltaTime);
        Mathf.Clamp(velocity.x, -2, 2);
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
        Debug.Log("COIN");
    }
}
