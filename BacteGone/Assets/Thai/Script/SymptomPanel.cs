using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SymptomPanel : MonoBehaviour
{
    public Transform SymptomRoot;
    public GameObject TransmetinPrefab;
    public GameObject KlacidFortePrefab;
    public GameObject GanatonPrefab;
    public GameObject CreonPrefab;
    public GameObject HidrasecPrefab;
    public GameObject DuspatalPrefab;
    public GameObject DicetelPrefab;
    public GameObject DuphalacPrefab;
    public GameObject DuphalacLiverPrefab;

    public void Show(MedicinName medicine)
    {
        Reset();

        GameObject symptomPrefab = null;

        switch (medicine)
        {
            case MedicinName.None:
                break;

            case MedicinName.Transmetin:
                symptomPrefab = TransmetinPrefab;
                break;

            case MedicinName.KlacidForte:
                symptomPrefab = KlacidFortePrefab;
                break;

            case MedicinName.Ganaton:
                symptomPrefab = GanatonPrefab;
                break;

            case MedicinName.Creon:
                symptomPrefab = CreonPrefab;
                break;

            case MedicinName.Hidrasec:
                symptomPrefab = HidrasecPrefab;
                break;

            case MedicinName.Duspatal:
                symptomPrefab = DuspatalPrefab;
                break;

            case MedicinName.Dicetel:
                symptomPrefab = DicetelPrefab;
                break;

            case MedicinName.DuphalacTntestine:
                symptomPrefab = DuphalacPrefab;
                break;

            case MedicinName.DuphalacLiver:
                symptomPrefab = DuphalacLiverPrefab;
                break;
        }

        if (symptomPrefab == null)
            return;

        GameObject symptomObject = Utils.Spawn(symptomPrefab, SymptomRoot);
        SymptomUnit symptomUnit = symptomObject.GetComponent<SymptomUnit>();
        symptomUnit.Init();
    }

    public void Reset()
    {
        Utils.RemoveAllChildren(SymptomRoot);
    }
}