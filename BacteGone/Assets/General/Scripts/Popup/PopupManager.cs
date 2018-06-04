using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Transform Root;

    public GameObject SuccessPrefab;
    public GameObject ErrorPrefab;

    public GameObject Loading;

    public void InitSuccessPopup(Action callback = null)
    {
        GameObject popupObject = Utils.Spawn(SuccessPrefab, Root);
        SuccessPopup successPopup = popupObject.GetComponent<SuccessPopup>();
        successPopup.Init(callback);
    }

    public void InitErrorPopup(Action tryAgainCallback = null, Action homeCallback = null)
    {
        GameObject popupObject = Utils.Spawn(ErrorPrefab, Root);
        ErrorPopup errorPopup = popupObject.GetComponent<ErrorPopup>();
        errorPopup.Init(tryAgainCallback, homeCallback);
    }

    public void ShowLoading()
    {
        Loading.SetActive(true);
    }

    public void HideLoading()
    {
        Loading.SetActive(false);
    }
}