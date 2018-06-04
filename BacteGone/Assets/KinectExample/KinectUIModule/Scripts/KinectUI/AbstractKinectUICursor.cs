using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
///     Abstract UI component class for hand cursor objects
/// </summary>
[RequireComponent(typeof(CanvasGroup), typeof(Image))]
public abstract class AbstractKinectUICursor : MonoBehaviour
{
    public KinectUIHandType HandType;
    public float AlphaSpeed;

    protected KinectInputData Data;
    protected CanvasGroup Group;
    protected Image MainImage;

    protected virtual void Awake()
    {
        Data = KinectInputModule.Instance.GetHandData(HandType);

        Group = GetComponent<CanvasGroup>();
        Group.blocksRaycasts = false;
        Group.interactable = false;
        Group.alpha = 0;

        MainImage = GetComponent<Image>();
        MainImage.raycastTarget = false;
    }

    protected virtual void Update()
    {
        if (Data == null || !KinectInputModule.Instance.AllowUpdate)
        {
            Hide();
            return;
        }

        if (Data.CurrentHandState == KinectInterop.HandState.NotTracked
            && Data.CurrentBufferTime <= 0)
        {
            Hide();
            return;
        }

        Show();
        ProcessData();
    }

    protected virtual void Show()
    {
        if (Group.alpha < 1)
        {
            float alpha = Group.alpha + Time.deltaTime * AlphaSpeed;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Group.alpha = alpha;
        }
    }

    protected virtual void Hide()
    {
        if (Group.alpha > 0)
        {
            float alpha = Group.alpha - Time.deltaTime * AlphaSpeed;
            alpha = Mathf.Clamp(alpha, 0, 1);
            Group.alpha = alpha;
        }
    }

    protected abstract void ProcessData();
}