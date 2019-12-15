using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGame : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    Text scoreUI;
    [SerializeField]
    Text timeUI;
    [SerializeField]
    GameObject bonusCanvasUI;

    [Header("Objects")]
    [SerializeField]
    GameObject bonusUIprefab;
    [SerializeField]
    MovingLevel movingLevel;
    [SerializeField]
    float levelSpeedChangeTime = 10.0f;
    [SerializeField]
    GameObject playerTargetPos;
    public GameObject PlayerTarget { get { return playerTargetPos; } }
    float leftWorldTargetPos, rightWorldTargetPos;
    int stepsBetween = 5;
    PlayerController player;

    [SerializeField]
    EndGame endGame; //Next gamemode state

    //Scoring info
    float score;
    float timer;
    int coinsCollected;
    float speed = 1.0f;
    //Set up in game state
    public void Begin()
    {
        timer = 0;
        score = 0;
        speed = 1.0f;

        gameObject.SetActive(true); //UI

        if (player == null) player = GameObject.FindObjectOfType<PlayerController>();
        player.enabled = true;
        //Calculate left and right position for player target positions
        leftWorldTargetPos = Camera.main.ScreenToWorldPoint(new Vector3(2, 0, 0)).x; //close to edge of screen
        rightWorldTargetPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 3, 0, 0)).x;//
        playerTargetPos.transform.position = new Vector3((leftWorldTargetPos - rightWorldTargetPos) / 2, 0, 0); //start in the middle of the two

        movingLevel.enabled = true;
        player.SetUp(this);
    }

    void Update()
    {
        //Timer
        timer += Time.deltaTime;
        speed = movingLevel.ChangeSpeed((int)((timer+levelSpeedChangeTime) / levelSpeedChangeTime));//Adding levelSpeedChangeTime so it starts at 1 
        timeUI.text = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);
        //Score update
        score += speed * Time.deltaTime;
        scoreUI.text = score.ToString("F0");
    }
    
    public void CoinCollected()
    {
        coinsCollected++;
        if (coinsCollected % 10 == 0) MoveTarget(1); //If collected 10 coins move the player forward

        float bonusScore = 100;
        //Bonus text 
        GameObject bonusUI = Instantiate<GameObject>(bonusUIprefab, bonusCanvasUI.transform);
        Text bonusText = bonusUI.GetComponent<Text>();
        bonusText.text = bonusScore.ToString();
        bonusText.color = Color.black;
        Destroy(bonusUI, 1.0f);
        //Score
        score += bonusScore;
        scoreUI.text = score.ToString("F0");
    }
    //Occurs when player dies, disables player script and sets up end state  
    public void GameOver()
    {
        player.enabled = false;
        gameObject.SetActive(false);
        endGame.Load(score, timer, coinsCollected);
    }
    //Called when player takes damage, set up on the damage event on the player 
    public void DamageTaken(int amount)
    {
        //Show negative score
        float scorePenalty = -50;
        GameObject bonusUI = Instantiate<GameObject>(bonusUIprefab, bonusCanvasUI.transform);
        Text bonusText = bonusUI.GetComponent<Text>();
        bonusText.text = scorePenalty.ToString();
        bonusText.color = Color.red;
        Destroy(bonusUI, 1.0f);
        score -= scorePenalty;
        scoreUI.text = score.ToString("F0");
        //Move player
        if (playerTargetPos.transform.position.x == leftWorldTargetPos) player.Die(); //if player is already at left most position and has been hit have them die
        else MoveTarget(-amount);
    }
    //Moves the player target position
    private void MoveTarget(int incrementAmount)
    {
        float newX = playerTargetPos.transform.position.x - ((leftWorldTargetPos - rightWorldTargetPos) / stepsBetween) * incrementAmount; //Left and right a world positions for off screen left and about 1/3 from the left 
        newX = Mathf.Clamp(newX, leftWorldTargetPos, rightWorldTargetPos);
        playerTargetPos.transform.position = new Vector3(newX, 0, 0);
    }
}
