using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GSSplash : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_EDITOR
        BaseOnline.Tracking = true;
#endif

        Application.targetFrameRate = 60;
        Random.InitState(DateTime.UtcNow.Millisecond + DateTime.UtcNow.Second + DateTime.UtcNow.Minute);
        LeanTween.init();

        Preferences.LoadPreferences();
        LocalizationData.LoadLocalizationLocal();

        SceneManager.LoadScene(SceneName.Home);
    }
}