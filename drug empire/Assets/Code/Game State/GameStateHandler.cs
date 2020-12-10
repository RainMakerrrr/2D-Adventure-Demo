using System;
using System.Collections;
using PlayerController;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateHandler : MonoBehaviour
{
    [SerializeField] private GameState _state;

    public GameState State
    {
        get => _state;
        set => _state = value;
    }

    public static GameStateHandler Instance { get; private set; }

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && _state == GameState.START)
            StartCoroutine(GetComponent<StartQuest>().StartFirstQuest());
    }
    
    
}

public enum GameState
{
    START, JOB ,LOSE
}
