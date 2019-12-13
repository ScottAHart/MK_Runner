using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLevel : MonoBehaviour
{

    List<GameObject> platforms;
    float moveSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        platforms = new List<GameObject>();
        foreach (Transform t in transform)
            platforms.Add(t.gameObject);
    }

    // Update is called once per frame
    void Update()
    {


        foreach(GameObject p in platforms)
        {
            if (p != null)
            {
                //Move left
                p.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                
                //Deal if off screen
                Bounds bounds = new Bounds(p.transform.position, Vector3.zero);
                foreach (var renderer in p.GetComponentsInChildren<SpriteRenderer>())
                    bounds.Encapsulate(renderer.bounds);
                if (Camera.main.WorldToScreenPoint(bounds.center + Vector3.right * bounds.extents.x).x < 0)
                {
                    //Destroy(p.gameObject);
                    p.transform.position = 
                        new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0)).x, p.transform.position.y, p.transform.position.z) //Move center to right side
                        + Vector3.right * bounds.extents.x; //Offset to off screen
                }
            }
        }
    }
}
