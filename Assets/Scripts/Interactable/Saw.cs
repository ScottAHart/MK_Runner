using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{

    [SerializeField]
    float rotationSpeed = 50.0f;
    [SerializeField]
    float moveDuration = 2.0f;
    [SerializeField]
    Vector3 endPoint = new Vector3(0, 10, 0);
    [SerializeField]
    int damage = 2;

    Vector3 startPos;

    float moveTimer;

    private void Start()
    {
        startPos = this.transform.localPosition;
    }

    private void Update()
    {
        moveTimer += Time.deltaTime;
        transform.localPosition = Vector3.Lerp(startPos, startPos + endPoint, Mathf.PingPong(moveTimer, moveDuration)/ moveDuration); //Move saw to offset and back based off duration 

        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime)); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

}
