using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandHelper : MonoBehaviour
{
    public RawImage LeftImage;
    public RawImage RightImage;

    public Texture2D LeftHandClose;
    public Texture2D LeftHandOpen;
    public Texture2D RightHandClose;
    public Texture2D RightHandOpen;

    public Color FromColor;
    public Color ToColor;
    public float TweenTime;

    private RectTransform _rectTransform;

    public void Init(Vector3 worldPosition, KinectInterop.HandState leftHandState,
        KinectInterop.HandState rightHandState, float canvasRadius = 98.5f)
    {
        _rectTransform = GetComponent<RectTransform>();

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        Vector3 canvasPosition = Utility.ConvertScreenPositionToCanvasPosition(KinectInputModule.Instance.TargetCanvas,
            screenPosition);
        _rectTransform.anchoredPosition = canvasPosition;

        LeftImage.rectTransform.anchoredPosition = new Vector2(-canvasRadius, 0);
        RightImage.rectTransform.anchoredPosition = new Vector2(canvasRadius, 0);

        if (leftHandState == KinectInterop.HandState.Closed)
        {
            LeftImage.texture = LeftHandClose;
        }
        else
        {
            LeftImage.texture = LeftHandOpen;
        }

        if (rightHandState == KinectInterop.HandState.Closed)
        {
            RightImage.texture = RightHandClose;
        }
        else
        {
            RightImage.texture = RightHandOpen;
        }

        LeftImage.SetNativeSize();
        RightImage.SetNativeSize();

        LeftImage.color = FromColor;
        RightImage.color = FromColor;

        LeanTween.color(LeftImage.rectTransform, ToColor, TweenTime).setLoopPingPong();
        LeanTween.color(RightImage.rectTransform, ToColor, TweenTime).setLoopPingPong();
    }
}