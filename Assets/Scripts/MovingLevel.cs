using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLevel : MonoBehaviour
{
    //Class for holding bounds of a level
    class LevelAndBounds
    {
        public GameObject level;
        public Bounds bounds;
        public int index;
        public LevelAndBounds(GameObject obj, Bounds bounds, int prefabIndex)
        {
            this.level = obj;
            this.bounds = bounds;
            this.index = prefabIndex;
        }
    }
    [SerializeField]
    GameObject[] levelPrefabs; //Populated list of possible levels to load
    
    List<LevelAndBounds> levels; //list of currently moving levels
    float moveSpeed = 2.0f; //Speed of level movement
    void Start()
    {
        levels = new List<LevelAndBounds>();
        //Get the current levels in the scene
        foreach (Transform t in transform)
        {
            LevelAndBounds level = new LevelAndBounds(t.gameObject, CalculateSpriteBounds(t.gameObject), 0);            
            levels.Add(level);
        }
    }
    void Update()
    {
        List<LevelAndBounds> removedLevels = new List<LevelAndBounds>();
        //Move levels to the left
        foreach(var level in levels)
        {
            GameObject levelObject = level.level;
            if (levelObject != null)
            {
                //Move left
                levelObject.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                level.bounds.center +=  Vector3.left * moveSpeed * Time.deltaTime;
                //Destroy level if it is off the screen
                if (Camera.main.WorldToScreenPoint(level.bounds.center + Vector3.right * level.bounds.extents.x).x < 0)
                {
                    Destroy(levelObject);
                    removedLevels.Add(level);
                }
            }
        }
        //Add new level if current level is no longer off screen
        LevelAndBounds lastLevel = levels[levels.Count - 1];
        if (Camera.main.WorldToScreenPoint(lastLevel.bounds.center + Vector3.right * lastLevel.bounds.extents.x).x < Screen.width)
            levels.Add(LoadNextLevel());
        //Removed destroyed levels 
        foreach (var level in removedLevels)
            levels.Remove(level);

    }
    //Instantiate new level from prefab list, move to right side of screen and calculate bounds
    LevelAndBounds LoadNextLevel()
    {
        int index = Random.Range(0, levelPrefabs.Length);
        //Change index if it what was last spawned (stops duplicate spawns)
        while(index == levels[levels.Count -1].index) 
            index = Random.Range(0, levelPrefabs.Length);

        GameObject level = Instantiate<GameObject>(levelPrefabs[index], this.transform);
        
        Bounds bounds = CalculateSpriteBounds(level);
        level.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x, level.transform.position.y, level.transform.position.z) //Move center to right side
                    + Vector3.right * (level.transform.position.x - bounds.center.x)
                    + Vector3.right * bounds.extents.x; //Offset to off screen

        bounds = CalculateSpriteBounds(level);
        return new LevelAndBounds(level, bounds, index);
    }
    //Calculate Bounds of a game objects sprite renderers
    Bounds CalculateSpriteBounds(GameObject level)
    {
        SpriteRenderer[] renderers = level.GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0) throw new System.Exception("Cannot calculate bounds of sprites, no sprite renderers available in game object");
        Bounds bounds = renderers[0].bounds;
        foreach (var renderer in renderers)
            bounds.Encapsulate(renderer.bounds);
        return bounds;
    }
}
