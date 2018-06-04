using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorPopup : Popup
{
    private Action _tryAgainCallback;
    private Action _homeCallback;

    public void Init(Action tryAgainCallback = null, Action homeCallback = null)
    {
        _tryAgainCallback = tryAgainCallback;
        _homeCallback = homeCallback;
    }

    public void OnTryAgainButtonClick()
    {
        StartCoroutine(FadeOut());

        if (_tryAgainCallback != null)
            _tryAgainCallback();
    }

    public void OnBackToHomeButtonClick()
    {
        StartCoroutine(FadeOut());

        if (_homeCallback != null)
            _homeCallback();
    }
}