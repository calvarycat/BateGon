using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GSHome : GSTemplate
{
    public static GSHome Instance { get; private set; }

    public RectTransform PanelTransform;
    public Vector2 FromPosition;
    public Vector2 ToPosition;
    public float TweenTime;

    public ButtonCTA StartButton;
    public AudioClip ButtonClip;
    public AudioClip MusicClip;

    private LTDescr _panelMoveDescr;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Init()
    {
        base.Init();

        StartButton.TargetButton.onClick.AddListener(OnStartButtonClick);
    }

    public override void OnResume()
    {
        Debug.Log("On rfessum home");
        base.OnResume();

        AudioManager.PlayMusic(MusicClip);
        KinectInputModule.Instance.AllowUpdate = true;
        ShowPanel();
    }

    private void ShowPanel()
    {
        StopTween();
        PanelTransform.anchoredPosition = FromPosition;
        _panelMoveDescr = LeanTween.move(PanelTransform, ToPosition, TweenTime).setOnComplete(OnShowPanelFinish);
    }

    private void OnShowPanelFinish()
    {
        StartButton.Show();
    }

    private void HidePanel()
    {
        StopTween();
        StartButton.Hide();
        PanelTransform.anchoredPosition = ToPosition;
        _panelMoveDescr = LeanTween.move(PanelTransform, FromPosition, TweenTime).setOnComplete(OnHidePanelFinish);
    }

    private void StopTween()
    {
        if (_panelMoveDescr != null)
        {
            LeanTween.cancel(PanelTransform.gameObject, _panelMoveDescr.id);
            _panelMoveDescr = null;
        }
    }

    private void OnHidePanelFinish()
    {
        // bắt đầu playing game
        GameStatesManager.Instance.MyStateMachine.SwitchState(GSPlaying.Instance);
    }

    public void OnStartButtonClick()
    {
        KinectInputModule.Instance.AllowUpdate = false;
        HidePanel();
        AudioManager.PlaySound(ButtonClip);
    }

    private void Update()
    {
        if(Cheat.isCheat)
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                OnStartButtonClick();
            }
        }
    }
}