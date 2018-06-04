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
    void OnEnable()
    {
        speed = 1.5f;
        countTimeChange = 10;
    }
    void Update()
    {
        countTimeChange -= Time.deltaTime;
        if (countTimeChange <= 0)
        {
            speed = speed + .5f;
            countTimeChange = 10;
            if (speed > 4)
            {
                speed = 4;
            }
        }
        //  Debug.Log(nextEggTime + "ti" + Time.time);
        if (game1Manager.isCall)
            return;
        if (nextEggTime < Time.time)
        {
            SpawnEgg();
            float randomtime = Random.Range(spawnRate - .2f, spawnRate + .2f);
            nextEggTime = Time.time + randomtime;

            //spawnRate = Mathf.Clamp(randomtime, 0.3f, 99f);
            d++;
            if (d > 3)
            {
                d = 0;
                dropMainMedicin = true;
            }

        }

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
            int a = Random.Range(1, 4);
            eggTransform.GetComponent<MedicinTrigger>().Init(game1Manager.currentDisease, speed);
            //while (CheckListCurrent(a) || a == 6)
            //{
            //    a = Random.Range(1, 4);
            //}

            //if (dropMainMedicin)
            //{
            //    dropMainMedicin = false;
            //    if (!CheckListCurrent(game1Manager.currentDisease))
            //    {
            //        eggTransform.GetComponent<MedicinTrigger>().Init(game1Manager.currentDisease, speed);
            //    }

            //    else
            //    {
            //        eggTransform.GetComponent<MedicinTrigger>().Init(a, speed);

            //    }
            //}
            //else
            //{
            //    eggTransform.GetComponent<MedicinTrigger>().Init(a, speed);

            //}
            eggTransform.parent = transform;
        }
    }
    public int oldObject = 0;
    bool CheckListCurrent(int id)
    {
        string t = " a ";
        GameObject[] obj = GameObject.FindGameObjectsWithTag("MedicineGameTag");
        if (obj == null)
            return false;
        foreach (GameObject ob in obj)
        {

            MedicinTrigger md = ob.GetComponent<MedicinTrigger>();
           
            if (md != null && md.medicinID == id)
            {

                return true;
            }


        }
        return (false);
    }

}
