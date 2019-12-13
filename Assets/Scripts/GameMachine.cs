using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode
{
    void Begin();
    void End();
}
public class GameMachine : MonoBehaviour
{    
    [SerializeField]
    MainMenu startMode;

    IGameMode currentMode;

    public static GameMachine Instance { get { return _instance; } }
    private static GameMachine _instance;

    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        StartMode(startMode);
    }
    
    public void StartMode(IGameMode mode)
    {
        if (currentMode != null) currentMode.End();
        currentMode = mode;
        currentMode.Begin();
    }

}
