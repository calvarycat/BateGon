using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EggSpawner : MonoBehaviour
{
    [Tooltip("Prefab (model and components) used to instantiate eggs in the scene.")]
    public Transform eggPrefab;
    public Game1Manager game1Manager;
    public Transform[] positionDrop;

    private float nextEggTime = 0.0f;
    private float spawnRate = 1.2f;

    bool dropMainMedicin;
    int d = 0;
    float timechange = 10;
    float countTimeChange = 10;
    float speed = 1.5f;
    public float timeSpawnSpecial;
    void OnEnable()
    {
        timeSpawnSpecial = 0;
        speed = 1.5f;
        countTimeChange = 10;
        spawnRate = 1f;
    }
    void Update()
    {
        if (!GSPlaying.Instance.isFinishCould)
            return;
        countTimeChange -= Time.deltaTime;
        if (countTimeChange <= 0)
        {
            speed = speed + .5f;
            countTimeChange = 10;
            if (speed > 6)
            {
                speed = 6;
            }
            spawnRate -= .1f;
        }
       
        if (game1Manager.isCall)
            return;

        timeSpawnSpecial += Time.deltaTime;

        if(timeSpawnSpecial>=30)
        {
            InvokeSpecial();
            timeSpawnSpecial = 0;
        }

        if (nextEggTime < Time.time)
        {
            SpawnEgg();
            float randomtime = Random.Range(spawnRate - .2f, spawnRate + .2f);
            nextEggTime = Time.time + randomtime;
            d++;
            if (d > 3)
            {
                d = 0;
                dropMainMedicin = true;
            }

        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            InvokeSpecial();
        }

    }
    public GameObject special;
    public void InvokeSpecial()
    {
       Instantiate(special, new Vector3(0, 10f, 0), Quaternion.identity);
    }


    void SpawnEgg()
    {

        SpawnMedicin();

    }

    int oldPos = 0;
    void SpawnMedicin()
    {
        KinectManager manager = KinectManager.Instance;
        if (eggPrefab && manager && manager.IsInitialized() && manager.IsUserDetected())
        {
            long userId = manager.GetPrimaryUserID();
            Vector3 posUser = manager.GetUserPosition(userId);
           
            int posDrop = Random.Range(0, positionDrop.Length);
            while (posDrop == oldPos)
            {
                posDrop = Random.Range(0, positionDrop.Length);
            }
            oldPos = posDrop;
            float addXPos = positionDrop[posDrop].localPosition.x;
            Vector3 spawnPos = new Vector3(addXPos, 5f, posUser.z);
            Transform eggTransform = Instantiate(eggPrefab, spawnPos, Quaternion.identity) as Transform;
            int a = Random.Range(0, 5);
          
            if(!CheckListCurrent())
            {
                a = game1Manager.currentDisease;
            }
            eggTransform.GetComponent<MedicinTrigger>().Init(a, speed);         
            eggTransform.parent = transform;
        }
    }
   
    public int oldObject = 0;
    bool CheckListCurrent()
    {
       
        GameObject[] obj = GameObject.FindGameObjectsWithTag("MedicineGameTag");
        if (obj == null)
            return false;
        foreach (GameObject ob in obj)
        {
            MedicinTrigger md = ob.GetComponent<MedicinTrigger>();           
            if (md != null && md.medicinID == game1Manager.currentDisease)
            {
                return true;
            }
        }
        return (false);
    }
   
}
