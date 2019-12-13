using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndGame : MonoBehaviour, IGameMode
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

    public void Begin()
    {
        replayButton.onClick.AddListener(Replay);
        gameObject.SetActive(true);
    }

    public void End()
    {
        gameObject.SetActive(false);
    }

    public void SetUp(float score, float timer, int coinsCollected)
    {
        scoreUI.text = score.ToString("F0");
        timeUI.text = string.Format("{0}:{1:00}", (int)timer / 60, (int)timer % 60);
        coinsUI.text = coinsCollected.ToString();
    }

    void Replay()
    {
        SceneManager.LoadScene(0);
        //GameMachine.Instance.StartMode(mainMenu);
    }

}
