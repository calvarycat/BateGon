using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Kinect/Kinect UI Cursor")]
public class KinectUICursor : AbstractKinectUICursor
{
    public Color NormalColor = new Color(1f, 1f, 1f, 0.5f);
    public Color HoverColor = new Color(1f, 1f, 1f, 1f);
    public Color ClickColor = new Color(1f, 1f, 1f, 1f);
    public Vector3 ClickScale = new Vector3(0.8f, 0.8f, 0.8f);

    public float SmoothTime = 0.05f;

    protected Vector3 InitScale;
    protected Vector3 Velocity;

    protected override void Awake()
    {
        base.Awake();

        InitScale = transform.localScale;
        MainImage.color = NormalColor;
    }

    protected override void ProcessData()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
            Data.GetCanvasPosition(), ref Velocity, SmoothTime);

        if (Data.IsPressing)
        {
            MainImage.color = ClickColor;

            if (MainImage.transform.localScale != ClickScale)
                MainImage.transform.localScale = ClickScale;

            return;
        }

        if (Data.IsHovering)
        {
            if (MainImage.color != HoverColor)
                MainImage.color = HoverColor;
        }
        else
        {
            if (MainImage.color != NormalColor)
                MainImage.color = NormalColor;
        }

        if (MainImage.transform.localScale != InitScale)
            MainImage.transform.localScale = InitScale;
    }
}