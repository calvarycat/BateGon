using UnityEngine;
using System.Collections;

public class Game3Manager : MonoBehaviour
{
    [Tooltip("Prefab (model and components) used to instantiate eggs in the scene.")]

    public FruitTrigger fruitPrefab;
    private float nextEggTime = 0.0f;
    private float spawnRate = .5f;
    float timePlay = 85;
    public int percenGood = 70;
    public Transform parrentFruit;
    public AudioClip clipslashRight;
    public AudioClip clipslashwrong;
    void OnEnable()
    {
        FruitTrigger.SlashFruit += CatchObjectID;
    }
    void OnDisable()
    {
        FruitTrigger.SlashFruit -= CatchObjectID;
    }
    int score;
    GameController gs;
    public void Init(float _timeplay = 15, float extratime = 0)
    {
        score = 0;
        timePlay = _timeplay + extratime;
        gs = GameObject.FindObjectOfType<GameController>();
    }
    void Update()
    {
        GetHandState();
        timePlay -= Time.deltaTime;
        GSPlaying.Instance.ChangeTime(timePlay);
        if (timePlay <= 0)
        {
            GoToNextLevel(3);
        }
        if (nextEggTime < Time.time)
        {
            SpawnEgg();
            nextEggTime = Time.time + spawnRate;
            spawnRate = Mathf.Clamp(spawnRate, 0.3f, 99f);


            //  SpawnBotton();
        }
       
    }
    KinectManager manager;
    public KinectInterop.HandState leftHandState;
    public KinectInterop.HandState rightHandState;
    public GameObject[] hands;
    private void GetHandState()
    {
        if (manager == null)
        {
            manager = KinectManager.Instance;
        }

        if (manager.IsUserDetected())
        {
            long userId = manager.GetUserIdByIndex(0);
            leftHandState = manager.GetLeftHandState(userId);
            rightHandState = manager.GetRightHandState(userId);
            if (leftHandState == KinectInterop.HandState.NotTracked)
            {
                if (hands[0].activeSelf)
                    hands[0].SetActive(false);
            }
            else
            {
                if (!hands[0].activeSelf)
                    hands[0].SetActive(true);
            }
            if (rightHandState == KinectInterop.HandState.NotTracked)
            {
                if (hands[1].activeSelf)
                    hands[1].SetActive(false);
            }
            else
            {
                if (!hands[1].activeSelf)
                    hands[1].SetActive(true);
            }
        }
    }
    void GoToNextLevel(int lvl)
    {
        gs.currentGame = 0;
        gs.OnShowResult();

    }

    public FruitTrigger fruit;
    public void SpawnBotton()
    {
        Vector3 spawn = new Vector3(0, 0, 2);
        //  Instantiate(fruit, spawn, transform.rotation);

        KinectManager manager = KinectManager.Instance;

        if (fruitPrefab && manager && manager.IsInitialized() && manager.IsUserDetected())
        {
            int a = Random.Range(0, 100);

            long userId = manager.GetPrimaryUserID();
            Vector3 posUser = manager.GetUserPosition(userId);
            Debug.Log(posUser);
            //           float addXPos = Random.Range(-6f, 6f);

            //            Vector3 spawnPos = new Vector3(addXPos, 6, posUser.z);


            // FruitTrigger ft = Instantiate(fruitPrefab, spawnPos, Quaternion.identity);
            FruitTrigger ft = Instantiate(fruit, spawn, transform.rotation);
            if (a < percenGood)
            {
                //random good object
                ft.Init(true);
            }
            else
            {
                //random bad object
                ft.Init(false);
            }

            ft.transform.parent = parrentFruit;
        }
    }
    public Transform[] positionDrop;
    int oldPos = 0;
    void SpawnEgg()
    {
        KinectManager manager = KinectManager.Instance;

        if (fruitPrefab && manager && manager.IsInitialized() && manager.IsUserDetected())
        {
            int a = Random.Range(0, 100);

            long userId = manager.GetPrimaryUserID();
            Vector3 posUser = manager.GetUserPosition(userId);

            //float addXPos = Random.Range(-6f, 6f);

            //Vector3 spawnPos = new Vector3(addXPos, 6, posUser.z);
            int posDrop = Random.Range(0, positionDrop.Length);
            while (posDrop == oldPos)
            {
                posDrop = Random.Range(0, positionDrop.Length);
            }
            oldPos = posDrop;
            float addXPos = positionDrop[posDrop].localPosition.x;

            Vector3 spawnPos = new Vector3(addXPos, 5f, posUser.z);
            FruitTrigger ft = Instantiate(fruitPrefab, spawnPos, Quaternion.identity);
            if (a < percenGood)
            {
                //random good object
                ft.Init(true);
            }
            else
            {
                //random bad object
                ft.Init(false);
            }

            ft.transform.parent = parrentFruit;
        }
    }
    // score += 100;
    public void CatchObjectID(bool isGood)
    {
        if (isGood)
        {
            AudioManager.PlaySound(clipslashRight);
            GSPlaying.Instance.ChangeScore(1);
            gs.OnChangeScore(1);
        }
        else
        {
            AudioManager.PlaySound(clipslashwrong);
            GSPlaying.Instance.ChangeScore(-1);
            gs.OnChangeScore(-1);
        }


    }
}
