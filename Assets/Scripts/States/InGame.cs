using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGame : MonoBehaviour, IGameMode
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

        gameObject.SetActive(true);

        if (player == null) player = GameObject.FindObjectOfType<PlayerController>();
        player.enabled = true;

        movingLevel.enabled = true;
        player.SetUp(this);
    }

    public void End()
    {
        player.enabled = false;
        movingLevel.enabled = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
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
        float bonusScore = 100;
        GameObject bonusUI = Instantiate<GameObject>(bonusUIprefab, bonusCanvasUI.transform);
        bonusUI.GetComponent<Text>().text = bonusScore.ToString();
        Destroy(bonusUI, 1.0f);
        score += bonusScore;
        scoreUI.text = score.ToString("F0");

    }

    public void GameOver()
    {
        GameMachine.Instance.StartMode(endGame);
        endGame.SetUp(score, timer, coinsCollected);
    }
}
