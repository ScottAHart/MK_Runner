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
    GameObject playerTargetPos;
    public GameObject PlayerTarget { get { return playerTargetPos; } }
    float leftWorldTargetPos, rightWorldTargetPos;
    int stepsBetween = 5;
    PlayerController player;


    [Header("Knwon GameModes")]
    [SerializeField]
    EndGame endGame;


    float score;
    float timer;
    int coinsCollected;
    float speed = 1.0f;
    int playerPos; //10x Pixels from left of screen 

    public void Begin()
    {
        timer = 0;
        score = 0;
        speed = 1.0f;
        playerPos = 4;

        gameObject.SetActive(true); //UI

        if (player == null) player = GameObject.FindObjectOfType<PlayerController>();
        player.enabled = true;

        leftWorldTargetPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x; //off screen
        rightWorldTargetPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 3, 0, 0)).x;
        playerTargetPos.transform.position = new Vector3((leftWorldTargetPos - rightWorldTargetPos) / 2, 0, 0);

        movingLevel.enabled = true;
        player.SetUp(this);
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        timeUI.text = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);

        score += speed * Time.deltaTime;
        scoreUI.text = score.ToString("F0");
    }
    
    public void CoinCollected()
    {
        coinsCollected++;
        if (coinsCollected % 10 == 0) MoveTarget(1);

        float bonusScore = 100;
        GameObject bonusUI = Instantiate<GameObject>(bonusUIprefab, bonusCanvasUI.transform);
        Text bonusText = bonusUI.GetComponent<Text>();
        bonusText.text = bonusScore.ToString();
        bonusText.color = Color.black;
        Destroy(bonusUI, 1.0f);
        score += bonusScore;
        scoreUI.text = score.ToString("F0");
    }
    public void GameOver()
    {
        player.enabled = false;
        gameObject.SetActive(false);
        endGame.Load(score, timer, coinsCollected);
    }

    public void DamageTaken(int amount)
    {
        float scorePenalty = -50;
        GameObject bonusUI = Instantiate<GameObject>(bonusUIprefab, bonusCanvasUI.transform);
        Text bonusText = bonusUI.GetComponent<Text>();
        bonusText.text = scorePenalty.ToString();
        bonusText.color = Color.red;
        Destroy(bonusUI, 1.0f);
        score -= scorePenalty;
        scoreUI.text = score.ToString("F0");
        if (playerTargetPos.transform.position.x == leftWorldTargetPos) player.Die(); //player is already at left most position and has been hit have them die
        else MoveTarget(-amount);
    }
    //Moves the player target position
    private void MoveTarget(int steps)
    {
        float newX = playerTargetPos.transform.position.x - ((leftWorldTargetPos - rightWorldTargetPos) / stepsBetween) * steps; //Left and right a world positions for off screen left and about 1/3 from the left 

        newX = Mathf.Clamp(newX, leftWorldTargetPos, rightWorldTargetPos);

        playerTargetPos.transform.position = new Vector3(newX, 0, 0);
    }
}
