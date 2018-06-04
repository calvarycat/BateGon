using UnityEngine;
using System.Collections;

public class leftmove : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //rigidbody.AddForce(-400, 0, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        GetComponent<Rigidbody>().AddForce(-2, 0, 0);
        transform.Rotate(UnityEngine.Random.Range(0f, 5f), UnityEngine.Random.Range(0f, 5f), UnityEngine.Random.Range(0f, 5f));
        transform.position = new Vector3(transform.position.x, transform.position.y, -3);
        if (transform.position.y < -17)
        {            
            Destroy(gameObject);
        }
	}
}
