using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider2D))]
public class Player : MonoBehaviour
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
    LayerMask groundMask;
    // Start is called before the first frame update
    void Awake()
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            jumpInput = true;
        }

        
    }

    private void FixedUpdate()
    {
        if (jumpInput)
        {
            if (Mathf.Abs(rigidbody.velocity.y) < 0.1 && jump == JumpState.Ground)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jump = JumpState.Jump;

            }
            else if (jump == JumpState.Jump)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0); // reset velocity  
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jump = JumpState.Double;
            }
            jumpInput = false;
        }

        Vector3 pos = this.transform.position + Vector3.down * collider.size.y / 2.0f; //Position at bottom of collider
        if (Mathf.Abs(rigidbody.velocity.y) < 0.01 && Physics2D.OverlapCircle(pos, 0.05f, groundMask))
            jump = JumpState.Ground;

        Debug.DrawLine(pos, pos + Vector3.down * 0.05f, Color.green);
    }
}
