using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Kinect/Kinect UI Wait Cursor")]
public class KinectUIWaitCursor : AbstractKinectUICursor
{
    public Vector3 ScaleFrom = new Vector3(0.9f, 0.9f, 0.9f);
    public Vector3 ScaleTo = new Vector3(1.1f, 1.1f, 1.1f);
    public float ScaleTime = 0.4f;

    protected LTDescr Descr;

    protected override void Awake()
    {
        base.Awake();

        MainImage.type = Image.Type.Filled;
        MainImage.fillMethod = Image.FillMethod.Radial360;
        MainImage.fillAmount = 0f;

        transform.localScale = ScaleFrom;
        Descr = LeanTween.scale(gameObject, ScaleTo, ScaleTime).setLoopPingPong();
    }

    protected override void ProcessData()
    {
        transform.localPosition = Data.GetCanvasPosition();

        if (Data.IsHovering)
        {
            MainImage.fillAmount = Data.WaitOverAmount;
        }
        else
        {
            MainImage.fillAmount = 0f;
        }
    }
}