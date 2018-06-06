using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CountDownPanel : MonoBehaviour
{
    public Transform TitleTransform;
    public Text TitleValue;
    public RawImage TitleCircle;
   // public Text TargetText;
    public Text MessageText;
    public Animator TargetAnimator;
    public int CountFrom = 3;
    public AudioClip CountdownClip;
    public AudioClip GoClip;


    private Action _callback;
    private int _round;
    private string _message;
    private int _currentNumber;
    private LTDescr _titleCircleDescr;
    bool isSound;
    public TexturedNumber CountDownTextImage;
    public GameObject go;
    public void Init(Action callback = null, int round = 0, string message = "", bool isShowSound = true)
    {
        CountDownTextImage.gameObject.SetActive(true);
        go.gameObject.SetActive(false);
        isSound = isShowSound;
        _callback = callback;
        _round = round;
        _message = message;
        _currentNumber = CountFrom;

        if (round <= 0)
        {
            TitleTransform.gameObject.SetActive(false);
        }
        else
        {
            TitleTransform.gameObject.SetActive(false);
            TitleValue.text = string.Format("{0}", _round);
            TitleCircle.color = Color.white;
            _titleCircleDescr = LeanTween.color(TitleCircle.rectTransform, Color.red, 1f)
                .setLoopType(LeanTweenType.linear).setLoopCount(-1);
        }

        MessageText.gameObject.SetActive(false);
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        TargetAnimator.SetTrigger("Idle");
        TargetAnimator.ResetTrigger("Play");

        if (_currentNumber > 0)
        {
           // TargetText.text = _currentNumber.ToString();
            CountDownTextImage.Value= _currentNumber.ToString();
            _currentNumber--;
            TargetAnimator.ResetTrigger("Idle");
            TargetAnimator.SetTrigger("Play");
            if (isSound)
                AudioManager.PlaySound(CountdownClip);
        }
        else if (_currentNumber == 0)
        {
            //  TargetText.text = Localization.Get("Go");
            CountDownTextImage.gameObject.SetActive(false);
            go.gameObject.SetActive(true);
            _currentNumber--;
            TargetAnimator.ResetTrigger("Idle");
            TargetAnimator.SetTrigger("Play");
            if (isSound)
                AudioManager.PlaySound(GoClip);
        }
        else
        {
            if (!string.IsNullOrEmpty(_message))
            {
              
                go.gameObject.SetActive(true);
               // MessageText.gameObject.SetActive(true);
              //  MessageText.text = _message;

                if (_titleCircleDescr != null)
                {
                    LeanTween.cancel(TitleCircle.gameObject, _titleCircleDescr.id);
                    _titleCircleDescr = null;
                    TitleCircle.color = Color.white;
                }

                yield return new WaitForSeconds(3);
            }

            if (_callback != null)
                _callback();

            Destroy(gameObject);
        }
    }

    private void OnAnimationFinish()
    {
        StartCoroutine(PlayAnimation());
    }
}