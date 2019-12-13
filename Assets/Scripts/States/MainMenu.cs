using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour, IGameMode
{

    [Header("UI Elements")]
    [SerializeField]
    Text title;
    [SerializeField]
    Button playButton;

    [Header("Knwon GameModes")]
    [SerializeField]
    InGame inGame;
    public void Begin()
    {
       this.gameObject.SetActive(true);
    }

    public void End()
    {
        this.gameObject.SetActive(false);
    }
    private void Awake()
    {
        playButton.onClick.AddListener(Play);
    }
    public void Play()
    {
        GameMachine.Instance.StartMode(inGame);
    }

}
