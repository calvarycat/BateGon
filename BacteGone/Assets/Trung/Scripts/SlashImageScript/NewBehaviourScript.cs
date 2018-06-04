using UnityEngine;
using System.Collections;
using System;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public int hits;
    public int points;
    int chits;
    public Vector3 velocity;
   
    void Start()
    {
       
        float RndXCo = UnityEngine.Random.Range(-3f, 3f);
        transform.position = new Vector3(RndXCo, -5, 2);
        float x = UnityEngine.Random.Range(-100, 200);
        float y = UnityEngine.Random.Range(650, 1100);
        GetComponent<Rigidbody>().AddForce(x, y, 0);

    }

   
    void Update()
    {
        
     //   transform.position = new Vector3(transform.position.x, transform.position.y, -3);
        //if (transform.position.y > -10)
        //{
        //    transform.GetComponent<Collider>().isTrigger = false;
        //}
        //else
        //{
        //    transform.GetComponent<Collider>().isTrigger = true;
        //}
        if (transform.position.y < -17 || transform.position.y > 20 || transform.position.x > 60 || transform.position.x < -60)
        {
            //transform.position = new Vector3(-16/*UnityEngine.Random.Range(-16f, 16f)*/, -16, -3);
            //Start();
            //Instantiate(gameObject);   //<--will make a new snowman in hiarchy as snowman(clone) with more (clone) each time may cause peoblems
            Destroy(gameObject);
        }
    
    }
}
