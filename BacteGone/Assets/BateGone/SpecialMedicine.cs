using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMedicine : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("start");
      
        Invoke("Move", 1f);
    }

   
    public void Move()
    {
       
        int random = Random.Range(1, 4);
       
        iTween.MoveTo(this.gameObject, iTween.Hash("path", iTweenPath.GetPath("Path"+random.ToString()), "time", 30, "easytype", iTween.EaseType.linear));
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Move();
        }
    }
}
