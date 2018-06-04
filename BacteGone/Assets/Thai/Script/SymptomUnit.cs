using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SymptomUnit : MonoBehaviour
{
    public MedicinName Type;
    public ShineEffector Effector;

    private LTDescr _valueDescr;

    public void Init()
    {
        _valueDescr = LeanTween.value(gameObject, OnValueTween, -0.3f, 0.3f, 0.8f);
    }

    private void OnValueTween(float value)
    {
        Effector.YOffset = value;
    }
}