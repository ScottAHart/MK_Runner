using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlock : MonoBehaviour
{

    [SerializeField]
    GameObject block;
    [SerializeField]
    Vector3 blockNewPosOffSet;
    [SerializeField]
    float moveTime = 2;

    Vector3 blockStartPos;
    float timer;
    bool triggered;
    private void Start()
    {
        blockStartPos = block.transform.localPosition;
        if (moveTime == 0) throw new System.Exception("Move time cannot be 0");
    }
    void Update()
    {
        if (triggered)
        {
            timer += Time.deltaTime;
            block.transform.localPosition = Vector3.Lerp(blockStartPos, blockStartPos + blockNewPosOffSet, timer / moveTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            triggered = true;
        }
    }
}
