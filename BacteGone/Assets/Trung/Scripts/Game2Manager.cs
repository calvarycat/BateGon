using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2Manager : MonoBehaviour
{
    public delegate void OnNewSymtom();

    public static event OnNewSymtom _OnNewSymtom;

    public const float BufferTime = 0.4f;

    public GameObject[] symptom;
    private float nextEggTime = 0.0f;
    private float spawnRate = 10f;
    public int currentDisease;
    private int score;
    private KinectManager manager;
    public KinectInterop.HandState leftHandState;
    public KinectInterop.HandState rightHandState;
    public LeftHandTrigger leftHandTrigger;
    public RightHandTrigger rightHandTrigger;
    private float timePlay = 5;

    private float _buffer;
    private bool _isCreateHand;
    public AudioClip clipChooseNextSymtom;
    public AudioClip clipslashRight;
    public AudioClip clipslashwrong;

    private GameController gs;
    public bool isCall;
    private bool isHelpFirstTime;
    private float timeHelpFirstTime = 8;
    private float totalTimePlayGame2;
    private float timeOld;
    public bool canshowWarning;
    public GameObject[] colRed;
    public bool isCountDown;
    public GameObject[] hands;
    void OnEnable()
    {
        if (!hands[0].activeSelf)
            hands[0].SetActive(true);
        if (!hands[1].activeSelf)
            hands[1].SetActive(true);
    }
    public void Init(float _timeplay = 60, float _extraTime = 0)
    {
        isCountDown = false;
        isCall = false;
        score = 0;
        timePlay = _timeplay + _extraTime;
        timeOld = timePlay;
        //show hand helper time
        timeHelpFirstTime = 5;
        isHelpFirstTime = false;

        gs = FindObjectOfType<GameController>();
        StartCoroutine(ShowSymptom());
        canshowWarning = true;
    }

    public GameObject FindTheRealOne()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("MedicineGameTag");
        if (obj == null)
            return null;
        foreach (GameObject ob in obj)
        {
            ProductGame2 real = ob.GetComponent<ProductGame2>();
            if (real.medicinID == currentDisease)
            {
                return ob;
            }
        }
        return null;
    }
    //   public GameObject initestalSystem;
    private void Update()
    {
        totalTimePlayGame2 += Time.deltaTime;
        GetHandState();
        HideInCountdown();
        if (!isHelpFirstTime)
        {
            //  Debug.logger("ko zo");
            timeHelpFirstTime
                -= Time.deltaTime;
            if (timeHelpFirstTime < 0)
            {
                timeHelpFirstTime = 5;
                isHelpFirstTime = true;
                GameObject ob = FindTheRealOne();
                if (ob != null)
                {
                    ProductGame2 dd = ob.GetComponent<ProductGame2>();
                    GSPlaying.Instance.ShowHandHelper(ob.transform.position, (KinectInterop.HandState)dd.handStateLeft,
                        (KinectInterop.HandState)dd.handStateRight, 100f);
                }
            }
        }
        timePlay -= Time.deltaTime;
        if (!isCall)
            GSPlaying.Instance.ChangeTime(timePlay);
        if (timePlay <= 0 && !isCall)
        {
            isCountDown = true;
            //   gs.collider
            canshowWarning = false;
            colRed[0].SetActive(false);
            colRed[1].SetActive(false);
            GSPlaying.Instance.HideWarning();
            GSPlaying.Instance.ShowSymptom(0);
            CancelInvoke();
            isCall = true;
            DeleteAfterChoose();
            AudioManager.StopMusic();
            Intestinalsystem.SetActive(false);
            GSPlaying.Instance.PlayCountDown(GoToNextLevel, 3);
        }

        ProductGame2 pp = GetProduct2Hand();

        if (leftHandTrigger.handTrigger && rightHandTrigger.handTrigger && pp !=null)
        {
            if (obj == null)
            {
                obj = GetProduct2Hand();//leftHandTrigger.currentProduct;
            }
            else
            {
                if (obj != GetProduct2Hand())
                {
                    _isCreateHand = false;
                    GSPlaying.Instance.DestroyHand();
                    obj = GetProduct2Hand();
                }
            }
        }
        else
        {
            obj = null;
        }

        if (obj)
        {
            if (!_isCreateHand) // variable to detect the ring have create or not
            {
                if (IsHandStateCorrect(false))
                {
                    _isCreateHand = true;

                    GSPlaying.Instance.CreateHand(obj.transform.localPosition, 98.5f, 1.5f, OnChooseFinish);
                    _buffer = BufferTime;
                }
            }
            else
            {
                if (!IsHandStateCorrect(true))
                {
                    _buffer -= Time.deltaTime;

                    if (_buffer <= 0)
                    {
                        _isCreateHand = false;
                        GSPlaying.Instance.DestroyHand();
                    }
                }
                else
                {
                    _buffer = BufferTime;
                }
            }
        }
        else
        {
            _isCreateHand = false;
            GSPlaying.Instance.DestroyHand();
        }
    }
   
    ProductGame2 GetProduct2Hand()
    {
        ////  Debug.Log("vo day");
        //if (rightHandTrigger.listCurrentProduct.Count > 0 && leftHandTrigger.listCurrentProduct.Count > 0)
        //    foreach(ProductGame2 ob in rightHandTrigger.listCurrentProduct)
        //    {
        //        foreach(ProductGame2 bb in leftHandTrigger.listCurrentProduct)
        //        {
        //            if (ob.medicinID == bb.medicinID)
        //                return bb;
        //        }
        //    }
          
        return null;
    }
    void HideInCountdown()
    {
        if (isCountDown)
        {
            if (Intestinalsystem.activeSelf)
            {
                Intestinalsystem.SetActive(false);
            }

        }
    }
    private bool IsHandStateCorrect(bool allowUnknown)
    {
        if ((int)leftHandState == obj.handStateLeft
            && (int)rightHandState == obj.handStateRight)
            return true;

        //if ((int)rightHandState == obj.handStateLeft
        //    && (int)leftHandState == obj.handStateRight)
        //    return true;

        if (allowUnknown)
        {
            if ((int)leftHandState == obj.handStateLeft
                /*|| (int)leftHandState == obj.handStateRight*/)
            {
                if (rightHandState == KinectInterop.HandState.Unknown)
                    return true;
            }

            if ( /*(int)rightHandState == obj.handStateLeft
                ||*/ (int)rightHandState == obj.handStateRight)
            {
                if (leftHandState == KinectInterop.HandState.Unknown)
                    return true;
            }
        }

        return false;
    }

    public void OnChooseFinish()
    {
        if (obj)
        {
            isHelpFirstTime = true;
            LeanTween.scale(obj.gameObject, Vector3.zero, 1f);
            LeanTween.move(obj.gameObject, Vector3.zero, 2f);
            if (obj.medicinID == currentDisease)
            {
                AudioManager.PlaySound(clipslashRight);
                OnChooseRightMedicine();
            }
            else
            {
                AudioManager.PlaySound(clipslashwrong);
                OnChooseWrongMedicine();
            }
            GSPlaying.Instance.HideHandHelper();
        }
    }

    public void OnChooseRightMedicine()
    {
        if (_OnNewSymtom != null)
        {
            _OnNewSymtom();
        }
        DisableAllSymtom();
        obj = null;
        GSPlaying.Instance.ChangeScore(4);
        score += 4;
        gs.OnChangeScore(4);
        GSPlaying.Instance.PlayScreenEfect(true, 2);
        DeleteAfterChoose();
        Invoke("CallNextSymtom", 1f); // call next and qua màn
    }

    private void CallNextSymtom()
    {
        AudioManager.PlaySound(clipChooseNextSymtom);
        StartCoroutine(ShowSymptom());
        if (score >= 40 && !isCall)
        {
            AudioManager.StopMusic();

            isCountDown = true;
            GSPlaying.Instance.ShowSymptom(0);
            canshowWarning = false;
            colRed[0].SetActive(false);
            colRed[1].SetActive(false);
            isCall = true;
            DeleteAfterChoose();
            GoToNextLevel();
        }
    }

    public void OnChooseWrongMedicine()
    {
        GSPlaying.Instance.PlayScreenEfect(false, 2);
    }

    public void DeleteAfterChoose()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("MedicineGameTag");
        if(obj!=null)
        foreach (GameObject ob in obj)
        {
            Destroy(ob);
        }
    }

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
        }
    }

    private int _oldSymptomid = -1;
    private List<int> _symtomList;

    private IEnumerator ShowSymptom()
    {
        if(_OnNewSymtom != null)
        {
            _OnNewSymtom();
        }
        if(_symtomList==null || _symtomList.Count==0)
        _symtomList = new List<int> { 1, 2, 3, 4, 5, 7, 8 };


        int index = Random.Range(0, _symtomList.Count);

        while (_oldSymptomid == _symtomList[index] )
        {
            index = Random.Range(0, _symtomList.Count);
            yield return null;
        }

        int symptomID = _symtomList[index];
        _symtomList.RemoveAt(index);

        _oldSymptomid = symptomID;
        currentDisease = symptomID;
        int mapEnum = currentDisease + 1;
        MedicinName e = (MedicinName)mapEnum;
        GSPlaying.Instance.ShowSymptom(e);
        CreateMedicin(symptomID);
        symptom[symptomID].gameObject.SetActive(true);
    }

    private IEnumerator DisableSymptom(int symptomID, float ftime)
    {
        yield return new WaitForSeconds(ftime);
        symptom[symptomID].gameObject.SetActive(false);
    }

    public Transform[] MedicinPosition;
    public ProductGame2 game2Prefabs;

    public void CreateMedicin(int symptomID)
    {
        int pos1 = 0;
        int pos2 = Random.Range(0, 5);       
        int[] listProduct = SwapInts(RandomList(symptomID), pos1, pos2);
        for (int i = 0; i < listProduct.Length; i++)
        {
            ProductGame2 bj = Instantiate(game2Prefabs, MedicinPosition[i].localPosition, Quaternion.identity);
            bj.InitObject(listProduct[i]);
        }
    }

    private int[] SwapInts(int[] array, int position1, int position2)
    {
        int temp = array[position1]; // Copy the first position's element
        array[position1] = array[position2]; // Assign to the second element
        array[position2] = temp; // Assign to the first element

        return array;
    }

    public int[] RandomList(int id)
    {
        int[] result = new int[5];
        int d = 0;
        result[0] = id;
        while (d < 4)
        {
            int a = 0;
            if (id == 7 || id == 8)
            {
                a = Random.Range(1, 7);
            }
            else
            {
                a = Random.Range(1, 8);
            }

            if (a != id && !CheckExit(a, result))
            {
                if (id == 7)
                {
                    if (a == 8)
                        a = 7;
                }
                if (id == 8)
                {
                    if (a == 7)
                        a = 8;
                }
                d++;
                result[d] = a;
            }
        }
        return result;
    }

    private bool CheckExit(int num, int[] result)
    {
        for (int i = 0; i < result.Length; i++)
        {
            if (num == result[i])
                return true;
            if (num == 5)
            {
                if (CheckNum56(6, result))
                    return true;
            }
            if (num == 6)
            {
              //  if (CheckNum56(5, result))
                    return true;
            }
        }
        return false;
    }

   
    private bool CheckNum56(int num, int[] result)
    {
        for (int i = 0; i < result.Length; i++)
        {
            if (num == result[i])
                return true;
        }
        return false;
    }

    public ProductGame2 obj;


    public GameObject Intestinalsystem;

    private void GoToNextLevel()
    {
        float timeExtra = 0;
        if (totalTimePlayGame2 < timeOld)
        {
            timeExtra = timeOld - totalTimePlayGame2;
            if (timeExtra < 0)
                timeExtra = 0;
        }
        GSPlaying.Instance.HideHandHelper();
        Intestinalsystem.SetActive(false);
        CancelInvoke();
        StopAllCoroutines();
        DisableAllSymtom();
        GSPlaying.Instance.ShowSymptom(0);
        canshowWarning = false;
        GSPlaying.Instance.HideWarning();

    }

    private void DisableAllSymtom()
    {
        for (int i = 0; i < symptom.Length; i++)
            symptom[i].gameObject.SetActive(false);
    }
}