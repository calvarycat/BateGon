using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(KinectUIWaitOverButton))]
public class ButtonCTA : MonoBehaviour
{
    public HandCTA TargetHandCTA;

    private Button _button;

    public Button TargetButton
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();
            return _button;
        }
    }

    public void Show()
    {
        if (TargetHandCTA != null)
            TargetHandCTA.Show();
    }

    public void Hide()
    {
        if (TargetHandCTA != null)
            TargetHandCTA.Hide();
    }
}