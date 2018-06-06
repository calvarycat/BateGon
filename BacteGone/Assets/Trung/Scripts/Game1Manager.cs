using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game1Manager : MonoBehaviour
{
    // Use this for initialization
    public GameObject[] symptom;

    private float nextEggTime = 0.0f;
    private float spawnRate = 10f;
    public int currentDisease;
    private int score;
    private float timePlay = 5;
    public Transform positionPopup;
    public GameController gs;

    public AudioClip clipChooseNextSymtom;
    public AudioClip clipslashRight;
    public AudioClip clipslashwrong;
    public bool isPlaycountDown;
    public GameObject RootLive;
    int numberofLive = 4;
    private void Awake()
    {
        if (CheatClass.OnCheat)
            timePlay = 1;
    }

    private void OnEnable()
    {
        HandTrigger.OnCatchingObject += CatchObjectID;
    }

    private void OnDisable()
    {
        HandTrigger.OnCatchingObject -= CatchObjectID;
    }

    private float totalTimePlay;

    bool isRunGame = false;
    public void Init(float _timeplay = 300)
    {
        Debug.Log("No vo co 1 lan init");
        numberofLive = 4;
        isRunGame = true;
        isGo = false;
        timeextra = 0;
        isCall = false;
        isPlaycountDown = true;
        score = 0;
        timePlay = _timeplay;
        totalTimePlay = 0;
        StartCoroutine(ShowSymptom());
        ResetLives(numberofLive);
    }
    public void AddLive()
    {
        numberofLive++;
        if(numberofLive>4)
        {
            numberofLive = 4;
        }
        ResetLives(numberofLive);
    }
    public void ResetLives(int live)
    {
        for (int i = 0; i < RootLive.transform.childCount; i++)
        {
            if (i < live)
            {
                RootLive.transform.GetChild(i).gameObject.SetActive(true);
            }              
            else
            {
                RootLive.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }
    // Update is called once per frame
    private void Update()
    {
        //if (!isRunGame)
        //{
        //    return;
        //}
        //totalTimePlay += Time.deltaTime;
        //timePlay -= Time.deltaTime;
        //if (!isCall)
        //    GSPlaying.Instance.ChangeTime(timePlay);
        //if (timePlay <= 0)
        //{
        //    if (!isCall)
        //    {
        //        isCall = true;
        //        AudioManager.StopMusic();
        //    }
        //}
        //if (timePlay <= 3 && !isGo)
        //{
        //    isGo = true;
        //    GSPlaying.Instance.PlayCountDown(GoToNextLevel, 2, "Finish");
        //}
    }
    bool isGo = false;
    public bool isCall = true;

    private int _oldSymptomid = -1;
    private List<int> _symtomList;

    private IEnumerator ShowSymptom()
    {
        if (isCall)
        {
            GSPlaying.Instance.ShowSymptom(0);
            yield break;
        }

        if (_symtomList == null || _symtomList.Count == 0)
        {
            _symtomList = new List<int> { 0,1, 2, 3, 4 };
        }

        int index = Random.Range(0, _symtomList.Count);

        while (_oldSymptomid == _symtomList[index])
        {
            index = Random.Range(0, _symtomList.Count);
            yield return null;
        }

        int symptomID = _symtomList[index];
        _symtomList.RemoveAt(index);

        _oldSymptomid = symptomID;

        AudioManager.PlaySound(clipChooseNextSymtom);
        currentDisease = symptomID;
      
        symptom[symptomID].gameObject.SetActive(true);
    }
    

    private IEnumerator DisableSymptom(int symptomID, float ftime)
    {
        Debug.Log("Ko zo disable");
      
        yield return new WaitForSeconds(ftime);
        symptom[symptomID].gameObject.SetActive(false);
        yield return ShowSymptom();
    }

    private bool canEnter;

    public void CatchObjectID(int id)
    {
     //   Debug.Log(id + "/" + currentDisease);
        if (id == currentDisease)
        {

            AudioManager.PlaySound(clipslashRight);
            score += 2;
            StartCoroutine(DisableSymptom(currentDisease, .1f));
            GSPlaying.Instance.PlayScreenEfect(true, 2);
            GSPlaying.Instance.ChangeScore(2);
            GSPlaying.Instance.ShowScorePopup(positionPopup.localPosition, 2);
            if (gs)
            {
                gs.OnChangeScore(2);
            }
         

        }
        else
        {
            numberofLive--;
            ResetLives(numberofLive);
           GSPlaying.Instance.ShowScorePopup(positionPopup.localPosition, -4);
            AudioManager.PlaySound(clipslashwrong);
            GSPlaying.Instance.PlayScreenEfect(false, 2);
        }
        if ((score >= 100 || numberofLive==0)  && !isCall )
        {

            isCall = true;
            AudioManager.StopMusic();
            // PlayCountDouwn();
            GoToNextLevel();
            // đã thua
        }
    }

    private float timeextra;

    private void GoToNextLevel()
    {
        DisableAllSymtom();
        Debug.Log("Go to next level");
        gs.OnShowResult();
    }

    public void PlayCountDouwn()
    {
        GSPlaying.Instance.ShowSymptom(0);
        DeleteAfterPlay();
        StopAllCoroutines();
        foreach (GameObject obj in symptom)
        {
            obj.SetActive(false);
        }
        GSPlaying.Instance.PlayCountDown(GoToNextLevel, 2, Localization.Get("Round2Message"));
    }

    public GameObject Intestinalsystem;

    private void DisableAllSymtom()
    {
        for (int i = 0; i < symptom.Length; i++)
            symptom[i].gameObject.SetActive(false);
        Intestinalsystem.SetActive(true);
    }

    public void DeleteAfterPlay()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("MedicineGameTag");
        if (obj != null)
            foreach (GameObject ob in obj)
            {
                Destroy(ob);
            }
    }
}