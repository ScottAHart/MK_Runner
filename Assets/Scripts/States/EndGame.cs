using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndGame : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    Text scoreUI;
    [SerializeField]
    Text timeUI;
    [SerializeField]
    Text coinsUI;
    [SerializeField]
    Button replayButton;

    [Header("Knwon GameModes")]
    [SerializeField]
    MainMenu mainMenu;
    //Sets up UI for final screen 
    public void Load(float score, float timer, int coinsCollected)
    {
        scoreUI.text = score.ToString("F0");
        timeUI.text = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);
        coinsUI.text = coinsCollected.ToString();
        replayButton.onClick.AddListener(Replay);
        gameObject.SetActive(true);
    }
    //Button click
    void Replay()
    {
        SceneManager.LoadScene(0); //Reload game
    }

}
