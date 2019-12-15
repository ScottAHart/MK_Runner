using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Animator animator;
    bool collected;

    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<PlayerController>().Coin();
                animator.SetTrigger("Collect");
                Destroy(gameObject, 1); //destroy object after a second to clean up
                collected = true;
            }
        }
    }
}
