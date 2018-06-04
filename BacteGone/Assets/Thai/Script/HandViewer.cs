using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HandViewer : MonoBehaviour
{
    public Image TargetImage;

    private RectTransform _rectTransform;
    private float _currentTime;
    private float _totalTime;
    private Action _callback;

    public void Init(Vector3 worldPosition, float canvasRadius,
        float totalTime, Action callback = null)
    {
        _rectTransform = GetComponent<RectTransform>();

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        Vector3 canvasPosition = Utility.ConvertScreenPositionToCanvasPosition(KinectInputModule.Instance.TargetCanvas,
            screenPosition);
        _rectTransform.anchoredPosition = canvasPosition;

        float diameter = canvasRadius * 2f;
        _rectTransform.sizeDelta = new Vector2(diameter, diameter);

        _currentTime = 0;
        _totalTime = totalTime;
        _callback = callback;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        float fillAmount = _currentTime / _totalTime;
        TargetImage.fillAmount = fillAmount;

        if (fillAmount >= 1)
        {
            if (_callback != null)
                _callback();

            Destroy(gameObject);
        }
    }
}