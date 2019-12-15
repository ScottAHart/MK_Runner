using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simple damage script for objects with a trigger
public class Spikes : MonoBehaviour
{
    [SerializeField]
    int damageAmount = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().TakeDamage(damageAmount);
        }
    }
}
