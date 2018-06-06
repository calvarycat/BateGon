using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GSPlaying : GSTemplate
{
    public static GSPlaying Instance { get; private set; }

    public delegate void TutorialFinish();

    public event TutorialFinish OnTutorialFinish;

    public RawImage ScreenEffectImage;
    public Texture2D EffectRight;
    public Texture2D EffectWrong;

    public Transform TutorialRoot;
    public GameObject TutorialPrefab;
    public Transform CountDownRoot;
    public GameObject CountDownPrefab;

    public Text ScoreText;
    public Text TimeText;
    public SymptomPanel TargetSymptomPanel;

    public Transform ScorePopupRoot;
    public GameObject ScorePopupPrefab;

    public Transform HandHelperRoot;
    public GameObject HandHelperPrefab;

    public Transform HandViewerRoot;
    public GameObject HandViewerPrefab;

    public TrailRenderer LeftHandTrail;
    public TrailRenderer RightHandTrail;

    public Text WarningText;
    public Color WarningFromColor;
    public Color WarningToColor;

    private LTDescr _colorScreenEffectDescr;

    private int _currentScore;
    private int _currentTweenScore;
    private LTDescr _scaleTextDescr;
    private LTDescr _scoreTextDescr;

    private LTDescr _timeColorDescr;
    private bool _isShowHandTrail;
    //private RectTransform _leftTrailRectTransform;
    //private RectTransform _rightTrailRectTransform;

    private LTDescr _warningColorDescr;

    public TexturedNumber ScoreTextureImage;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Init()
    {
        base.Init();

        //_leftTrailRectTransform = LeftHandTrail.GetComponent<RectTransform>();
        //_rightTrailRectTransform = RightHandTrail.GetComponent<RectTransform>();
    }

    public override void OnResume()
    {
       // Debug.Log("resume"); 
        base.OnResume();

        ResetScreenEffect();
      ResetTutorial();
       ResetScore();
        ResetScorePopup();
       ResetTime();
    
       // ResetHandHelper();
        ResetHandViewer();
        ResetHandTrail();
        ResetWarning();

        //ShowTutorial();
        OnShowTutorialResponse();
        isFinishCould = false;
    }

   public bool isFinishCould = false;
    #region ScreenEffect

    private void ResetScreenEffect()
    {
        ScreenEffectImage.color = Color.clear;

        if (_colorScreenEffectDescr != null)
        {
            LeanTween.cancel(ScreenEffectImage.gameObject, _colorScreenEffectDescr.id);
            _colorScreenEffectDescr = null;
        }
    }

    public void PlayScreenEfect(bool isRight, float duration)
    {
        ResetScreenEffect();

        if (isRight)
        {
            ScreenEffectImage.texture = EffectRight;
        }
        else
        {
            ScreenEffectImage.texture = EffectWrong;
        }

        ScreenEffectImage.color = Color.white;
        _colorScreenEffectDescr = LeanTween.color(ScreenEffectImage.rectTransform,
            Color.clear, duration / 2f).setDelay(duration / 2f);
    }

    #endregion

    #region Tutorial

    private void ResetTutorial()
    {
        Utils.RemoveAllChildren(TutorialRoot);
        Utils.RemoveAllChildren(CountDownRoot);
    }

    private void ShowTutorial()
    {
        KinectInputModule.Instance.AllowUpdate = true;

        GameObject panelObject = Utils.Spawn(TutorialPrefab, TutorialRoot);
        TutorialPanelBase tutorial = panelObject.GetComponent<TutorialPanelBase>();
        tutorial.Init(OnShowTutorialResponse);
    }
   public  AudioClip CounGo;
    private void OnShowTutorialResponse()
    {
        AudioManager.StopMusic();
        KinectInputModule.Instance.AllowUpdate = false;
        GameObject countObject = Utils.Spawn(CountDownPrefab, CountDownRoot);
        CountDownPanel countDown = countObject.GetComponent<CountDownPanel>();      
        countDown.Init(OnCountdownResponse, 1,"",false);
        AudioManager.PlaySound(CounGo);
    }

    private void OnCountdownResponse()
    {
        if (OnTutorialFinish != null)
            OnTutorialFinish();
        isFinishCould = true;
    }

    public void PlayCountDown(Action callback = null, int round = 0, string message = "",bool isShowSound=true)
    {
        GameObject countObject = Utils.Spawn(CountDownPrefab, CountDownRoot);
        CountDownPanel countDown = countObject.GetComponent<CountDownPanel>();
        countDown.Init(callback, round, message, isShowSound);
    }

    #endregion

    #region Score

    private void ResetScore()
    {
        if (_scaleTextDescr != null)
        {
            LeanTween.cancel(ScoreText.gameObject, _scaleTextDescr.id);
            _scaleTextDescr = null;
        }

        if (_scoreTextDescr != null)
        {
            LeanTween.cancel(ScoreText.gameObject, _scoreTextDescr.id);
            _scoreTextDescr = null;
        }

        _currentScore = 0;
        _currentTweenScore = 0;
        UpdateScoreText();
    }

    public void ChangeScore(int deltaScore)
    {
        if (deltaScore == 0)
            return;

        _currentScore += deltaScore;

        if (deltaScore > 0)
        {
            if (_scaleTextDescr != null)
            {
                LeanTween.cancel(ScoreText.gameObject, _scaleTextDescr.id);
                _scaleTextDescr = null;
            }

            ScoreText.rectTransform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            _scaleTextDescr = LeanTween.scale(ScoreText.rectTransform,
                Vector3.one, 0.2f);
        }

        if (_scoreTextDescr != null)
        {
            LeanTween.cancel(ScoreText.gameObject, _scoreTextDescr.id);
            _scoreTextDescr = null;
        }

        _scoreTextDescr = LeanTween.value(ScoreText.gameObject,
            OnScoreTextTween, _currentTweenScore, _currentScore, 0.2f);
    }

    private void OnScoreTextTween(float value)
    {
        _currentTweenScore = Mathf.RoundToInt(value);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        //ScoreText.text = string.Format("{0:000}", _currentTweenScore);
     
        ScoreTextureImage.Value = _currentTweenScore.ToString();
    }

    #endregion

    #region ScorePopup

    private void ResetScorePopup()
    {
        Utils.RemoveAllChildren(ScorePopupRoot);
    }

    public void ShowScorePopup(Vector3 worldPosition, int score)
    {
        GameObject scorePopupObject = Utils.Spawn(ScorePopupPrefab, ScorePopupRoot);
        ScorePopup scorePopup = scorePopupObject.GetComponent<ScorePopup>();
        scorePopup.Init(worldPosition, score);
    }

    #endregion

    #region Time

    private void ResetTime()
    {
        TimeText.color = Color.white;

        if (_timeColorDescr != null)
        {
            LeanTween.cancel(TimeText.gameObject, _timeColorDescr.id);
            _timeColorDescr = null;
        }
    }

    public void ChangeTime(float seconds)
    {
        TimeText.text = Utils.SecondsToString((int)seconds);

        if (seconds <= 10)
        {
            if (_timeColorDescr == null)
            {
                _timeColorDescr = LeanTween.colorText(TimeText.rectTransform, Color.red, 1f)
                    .setLoopType(LeanTweenType.linear).setLoopCount(-1);
            }
        }
        else
        {
            ResetTime();
        }
    }

    #endregion

    #region Symptom

    public void ShowSymptom(MedicinName medicine)
    {
        TargetSymptomPanel.Show(medicine);
    }

    #endregion

    #region HandHelper

    private void ResetHandHelper()
    {
        Utils.RemoveAllChildren(HandHelperRoot);
    }

    public void ShowHandHelper(Vector3 worldPosition, KinectInterop.HandState leftHandState,
        KinectInterop.HandState rightHandState, float canvasRadius = 98.5f)
    {
        GameObject handHelperObject = Utils.Spawn(HandHelperPrefab, HandHelperRoot);
        HandHelper handHelper = handHelperObject.GetComponent<HandHelper>();
        handHelper.Init(worldPosition, leftHandState, rightHandState, canvasRadius);
    }

    public void HideHandHelper()
    {
        ResetHandHelper();
    }

    #endregion

    #region HandViewer

    private void ResetHandViewer()
    {
        Utils.RemoveAllChildren(HandViewerRoot);
    }

    public void CreateHand(Vector3 worldPosition, float canvasRadius = 98.5f,
        float totalTime = 3f, Action callback = null)
    {
        GameObject handObject = Utils.Spawn(HandViewerPrefab, HandViewerRoot);
        HandViewer handViewer = handObject.GetComponent<HandViewer>();
        handViewer.Init(worldPosition, canvasRadius, totalTime, callback);
    }

    public void DestroyHand()
    {
        ResetHandViewer();
    }

    #endregion

    #region HandTrail

    private void ResetHandTrail()
    {
        //_isShowHandTrail = false;
        //LeftHandTrail.enabled = false;
        //RightHandTrail.enabled = false;
        //_leftTrailRectTransform.anchoredPosition = Vector3.zero;
        //_rightTrailRectTransform.anchoredPosition = Vector3.zero;
    }

    public void ShowHandTrail()
    {
        _isShowHandTrail = true;
        LeftHandTrail.enabled = true;
        RightHandTrail.enabled = true;
    }

    public void HideHandTrail()
    {
        ResetHandTrail();
    }

    private void UpdateHandTrail()
    {
       
    }

    #endregion

    #region Warning

    private void ResetWarning()
    {
        StopWarningColorTween();
        WarningText.text = "";
        WarningText.color = WarningFromColor;
    }

    private void StopWarningColorTween()
    {
        if (_warningColorDescr != null)
        {
            LeanTween.cancel(WarningText.gameObject, _warningColorDescr.id);
            _warningColorDescr = null;
        }
    }

    public void ShowWarning(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        StopWarningColorTween();

        WarningText.text = text;
        _warningColorDescr = LeanTween.colorText(WarningText.rectTransform, WarningToColor, 0.1f);
    }

    public void HideWarning()
    {
        StopWarningColorTween();
        _warningColorDescr = LeanTween.colorText(WarningText.rectTransform, WarningFromColor, 0.1f);
    }

    #endregion

    #region Result

    public void ShowResult(float happyValue)
    {
        GSResult.Instance.SetHappyValue(happyValue);
        GameStatesManager.Instance.MyStateMachine.SwitchState(GSResult.Instance);
    }

    #endregion


    private void CheckReset()
    {
        if (Input.GetKey(KeyCode.LeftControl)
            && Input.GetKey(KeyCode.LeftAlt)
            && Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneName.Empty);
        }
    }
}