using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Animator animator;
    bool collected;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<PlayerController>().Coin();
                animator.SetTrigger("Collect");
                Destroy(gameObject, 1);
                collected = true;
                //Play particle 
            }
        }
    }
}
