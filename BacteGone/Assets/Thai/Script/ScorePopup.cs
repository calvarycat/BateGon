using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Gradient = UnityEngine.Gradient;

public class ScorePopup : MonoBehaviour
{
    public Text TargetText;
    public Vector3 Distance;
    public float Time;

    public Color FromColor;
    public Color ToColor;

    public Gradient2 TargetGradient;
    public Gradient RightGradient;
    public Gradient WrongGradient;

    private RectTransform _rectTransform;

    public void Init(Vector3 worldPosition, int score)
    {
        _rectTransform = GetComponent<RectTransform>();

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        Vector3 canvasPosition = Utility.ConvertScreenPositionToCanvasPosition(KinectInputModule.Instance.TargetCanvas,
            screenPosition);
        _rectTransform.anchoredPosition = canvasPosition;

        if (score >= 0)
        {
            TargetText.text = "Perfect "+string.Format("+{0:00}", score);
            TargetGradient.EffectGradient = RightGradient;
        }
        else
        {
            // TargetText.text ="Opp "+ string.Format("{0:00}", score);
            TargetText.text = "Opp!";
            TargetGradient.EffectGradient = WrongGradient;
        }

        TargetText.color = FromColor;
        LeanTween.colorText(TargetText.rectTransform, ToColor, Time / 2.1f).setDelay(Time / 2.1f);

        Vector3 targetPosition = canvasPosition + Distance;
        LeanTween.move(_rectTransform, targetPosition, Time).setOnComplete(OnTweenComplete);
    }

    private void OnTweenComplete()
    {
        Destroy(gameObject);
    }
}