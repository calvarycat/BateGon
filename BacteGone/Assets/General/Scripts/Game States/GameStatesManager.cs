using UnityEngine;
using System;
using System.Collections;

public class GameStatesManager : MonoBehaviour
{
    public static GameStatesManager Instance { get; private set; }

    public GameObject InputProcessor { get; set; }
    public static Action OnBackKey { get; set; }
    public static Action OnCheatState { get; set; }
    public StateMachine MyStateMachine;
    public IState DefaultState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MyStateMachine.PushState(DefaultState);
    }
}