using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessPopup : Popup
{
    private Action _callback;

    public void Init(Action callback = null)
    {
        _callback = callback;
    }

    public void OnButtonClick()
    {
        StartCoroutine(FadeOut());

        if (_callback != null)
            _callback();
    }
}