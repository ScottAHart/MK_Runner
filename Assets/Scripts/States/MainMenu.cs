using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField]
    Text title;
    [SerializeField]
    Button playButton;

    [Header("Knwon GameModes")]
    [SerializeField]
    InGame inGame;
    public void Load()
    {
        this.gameObject.SetActive(true); //Activates UI 
    }
    private void Awake()
    {
        playButton.onClick.AddListener(Play);

    }
    public void Play()
    {
        this.gameObject.SetActive(false);
        inGame.Begin();
    }

}
