using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCTA : MonoBehaviour
{
    public RawImage HandImage;
    public Color HandFromColor;
    public Color HandToColor;
    public float HandTweenTime;
    public float FillAmountToHide;

    public KinectUIHandType HandType = KinectUIHandType.Right;

    private KinectInputData _kinectInputData;
    private bool _isShowing;
    private LTDescr _handColorDescr;

    private void Awake()
    {
        _kinectInputData = KinectInputModule.Instance.GetHandData(HandType);
    }

    public void Show()
    {
        _isShowing = true;
        StartTween();
    }

    public void Hide()
    {
        _isShowing = false;
        StopTween();
    }

    private void Update()
    {
        if (_kinectInputData.IsHovering)
        {
            OnWaitCursor(_kinectInputData.WaitOverAmount);
        }
        else
        {
            OnWaitCursor(0);
        }
    }

    private void OnWaitCursor(float fillAmount)
    {
        if (!_isShowing)
            return;

        if (fillAmount >= FillAmountToHide)
        {
            StopTween();
        }
        else
        {
            if (_handColorDescr == null)
                StartTween();
        }
    }

    private void StartTween()
    {
        HandImage.color = HandFromColor;
        _handColorDescr = LeanTween.color(HandImage.rectTransform, HandToColor, HandTweenTime)
            .setLoopPingPong();
    }

    private void StopTween()
    {
        if (_handColorDescr != null)
        {
            LeanTween.cancel(HandImage.gameObject, _handColorDescr.id);
            _handColorDescr = null;
        }

        HandImage.color = Color.clear;
    }
}