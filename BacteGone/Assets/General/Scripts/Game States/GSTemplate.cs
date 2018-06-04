using System.Collections;
using UnityEngine;

////////////////////////////////////////////////////////
//Author:
//TODO: a game state sample
////////////////////////////////////////////////////////

public class GSTemplate : IState
{
    public GameObject Main2D;
    public GameObject Main3D;

    protected bool IsFirstTime;

    protected override void Awake()
    {
        base.Awake();

        if (Main2D)
            Main2D.SetActive(false);

        if (Main3D)
            Main3D.SetActive(false);

        IsFirstTime = true;
    }

    /// <summary>
    ///     One time when start
    /// </summary>
    protected virtual void Init()
    {
    }

    protected virtual void OnBackKey()
    {
    }

    protected virtual void OnCheatState()
    {
    }

    public override void OnSuspend()
    {
        base.OnSuspend();
        GameStatesManager.OnBackKey = null;
        GameStatesManager.OnCheatState = null;

        if (Main2D)
            Main2D.SetActive(false);

        if (Main3D)
            Main3D.SetActive(false);
    }

    public override void OnResume()
    {
        base.OnResume();
        GameStatesManager.Instance.InputProcessor = Main2D;
        GameStatesManager.OnBackKey = OnBackKey;
        GameStatesManager.OnCheatState = OnCheatState;

        if (Main2D)
            Main2D.SetActive(true);

        if (Main3D)
            Main3D.SetActive(true);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (IsFirstTime)
        {
            IsFirstTime = false;
            Init();
        }
        OnResume();
    }

    public override void OnExit()
    {
        base.OnExit();
        OnSuspend();
    }
}