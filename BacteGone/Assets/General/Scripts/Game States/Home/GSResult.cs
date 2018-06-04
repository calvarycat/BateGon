using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GSResult : GSTemplate
{
    public static GSResult Instance { get; private set; }

    public FireworkCreator TargetFireworkCreator;

    public RectTransform PanelTransform;
    public Vector3 FromScale;
    public Vector3 ToScale;
    public float TimeScale;

    public Text HappyValueText;
    public ButtonCTA BackButon;

    public float WaitTime;

    public AudioClip ButtonClip;

    public float HappyValue { get; private set; }

    private LTDescr _panelScaleDescr;
    private LTDescr _happyValueDescr;

    private bool _beginWait;
    private float _currentWait;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Init()
    {
        base.Init();

        BackButon.TargetButton.onClick.AddListener(OnBackButonClick);
    }

    public override void OnResume()
    {
        base.OnResume();

        StopTweenScale();
        StopTweenValue();
        KinectInputModule.Instance.AllowUpdate = false;
        HappyValueText.text = "00%";

        _beginWait = false;
        _currentWait = WaitTime;

        //TargetFireworkCreator.Play();
        ShowPanel();
    }

    private void Update()
    {
        if (_beginWait)
        {
            if (_currentWait > 0)
            {
                _currentWait -= Time.deltaTime;
                if (_currentWait <= 0)
                {
                    OnBackButonClick();
                }
            }
        }
    }

    public void SetHappyValue(float happyValue)
    {
        HappyValue = happyValue;
    }

    private void ShowPanel()
    {
        PanelTransform.localScale = FromScale;
        LeanTween.scale(PanelTransform, ToScale, TimeScale).setOnComplete(OnShowPanelFinish);
    }

    private void OnShowPanelFinish()
    {
        ShowHappyValue();
    }

    private void HidePanel()
    {
        BackButon.Hide();
        PanelTransform.anchoredPosition = ToScale;
        LeanTween.scale(PanelTransform, FromScale, TimeScale).setOnComplete(OnHidePanelFinish);
    }

    private void OnHidePanelFinish()
    {
        GameStatesManager.Instance.MyStateMachine.SwitchState(GSHome.Instance);
    }

    private void StopTweenScale()
    {
        if (_panelScaleDescr != null)
        {
            LeanTween.cancel(PanelTransform.gameObject, _panelScaleDescr.id);
            _panelScaleDescr = null;
        }
    }

    private void ShowHappyValue()
    {
        _happyValueDescr = LeanTween.value(HappyValueText.gameObject,
            OnHappyValueTween, 0, HappyValue, HappyValue / 50f).setOnComplete(OnShowHappyValueFinish);
    }

    private void OnHappyValueTween(float value)
    {
        HappyValueText.text = string.Format("{0:00}%", Mathf.RoundToInt(value));
    }

    private void OnShowHappyValueFinish()
    {
        //TargetFireworkCreator.Show();
        KinectInputModule.Instance.AllowUpdate = true;
        BackButon.Show();
        _beginWait = true;
    }

    private void StopTweenValue()
    {
        if (_happyValueDescr != null)
        {
            LeanTween.cancel(HappyValueText.gameObject, _happyValueDescr.id);
            _happyValueDescr = null;
        }
    }

    private void OnBackButonClick()
    {
        //TargetFireworkCreator.Reset();
        KinectInputModule.Instance.AllowUpdate = false;
        HidePanel();
        _beginWait = false;
        AudioManager.PlaySound(ButtonClip);
    }
}