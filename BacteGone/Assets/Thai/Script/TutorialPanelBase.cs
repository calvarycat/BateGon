using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialPanelBase : MonoBehaviour
{
    public RectTransform PanelTransform;
    public Vector2 FromScale;
    public Vector2 ToScale;
    public float TweenTime;

    public RawImage TargetImage;
    public Texture ThumbnailTexture;
    public MovieTexture TargetMovieTexture;

    public Texture ThumbnailTextureVN;
    public MovieTexture TargetMovieTextureVN;

    public ButtonCTA StartButton;
    public Transform DescriptionPanel;

    public AudioClip ButtonClip;

    protected LTDescr PanelScaleDescr;
    protected Action Callback;

    public virtual void Init(Action callback = null)
    {
        Callback = callback;

        StartButton.TargetButton.onClick.AddListener(OnStartButtonClick);

        Reset();
        ShowPanel();
    }

    protected void Reset()
    {
        KinectInputModule.Instance.AllowUpdate = false;
        Localization.language = "vi";
        switch (Localization.language)
        {
            case "en":
                TargetImage.texture = ThumbnailTexture;
                break;

            case "vi":
                TargetImage.texture = ThumbnailTextureVN;
                break;

            default:
                TargetImage.texture = ThumbnailTexture;
                break;
        }

        TargetMovieTexture.loop = true;
        TargetMovieTexture.Stop();

        TargetMovieTextureVN.loop = true;
        TargetMovieTextureVN.Stop();

        StartButton.transform.localScale = Vector3.zero;
        DescriptionPanel.gameObject.SetActive(false);
    }

    protected virtual void ShowPanel()
    {
        StopTween();
        PanelTransform.localScale = FromScale;
        LeanTween.scale(PanelTransform, ToScale, TweenTime).setOnComplete(OnShowPanelFinish);
    }

    protected virtual void OnShowPanelFinish()
    {
        switch (Localization.language)
        {
            case "en":
                TargetImage.texture = TargetMovieTexture;
                TargetMovieTexture.Play();
                break;

            case "vi":
                TargetImage.texture = TargetMovieTextureVN;
                TargetMovieTextureVN.Play();
                break;

            default:
                TargetImage.texture = TargetMovieTexture;
                TargetMovieTexture.Play();
                break;
        }

        Invoke("OnAnimationFinish", 10f);
    }

    protected virtual void HidePanel()
    {
        StopTween();
        StartButton.Hide();
        PanelTransform.localScale = ToScale;
        LeanTween.scale(PanelTransform, FromScale, TweenTime).setOnComplete(OnHidePanelFinish);
    }

    protected virtual void OnHidePanelFinish()
    {
        if (Callback != null)
            Callback();

        Destroy(gameObject);
    }

    private void StopTween()
    {
        if (PanelScaleDescr != null)
        {
            LeanTween.cancel(PanelTransform.gameObject, PanelScaleDescr.id);
            PanelScaleDescr = null;
        }
    }

    protected void OnAnimationFinish()
    {
        if (StartButton.transform.localScale == Vector3.zero)
        {
            LeanTween.scale(StartButton.transform as RectTransform, Vector3.one, 0.4f)
                .setOnComplete(OnShowButtonFinish);
            KinectInputModule.Instance.AllowUpdate = true;
        }
    }

    protected void OnShowButtonFinish()
    {
        StartButton.Show();
        DescriptionPanel.gameObject.SetActive(true);
    }

    public void OnStartButtonClick()
    {
        HidePanel();
        AudioManager.PlaySound(ButtonClip);
    }
}