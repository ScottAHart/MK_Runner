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
    float moveSpeed = 1;
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
        //Move to target
        if (target != null && Mathf.Abs(transform.position.x - target.position.x) > 0.5f)
            transform.position += (new Vector3((target.position.x - rigidbody.position.x), 0, 0).normalized * moveSpeed * Time.fixedDeltaTime);
        //Jump check
        if (jumpInput)
        {
            if (Mathf.Abs(rigidbody.velocity.y) < 0.1 && jump == JumpState.Ground)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jump = JumpState.Jump;
                animator.SetBool("Jumping", true);

            }
            else if (jump == JumpState.Jump)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0); // reset velocity  
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jump = JumpState.Double;
            }
            jumpInput = false;
        }
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


    }
    public void Coin()
    {
        Debug.Log("COIN");
    }
}
