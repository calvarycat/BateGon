using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineHealth : MonoBehaviour
{

    // Use this for initialization
    public GameObject health;

    void Start()
    {

    }
    public bool isScaleHealth;
    // Update is called once per frame
    float x;
    public SpriteRenderer healthBar;
    void Update()
    {
     
    }
    public void AddHealth(float Health)
    {
        //if (isScaleHealth)
        //{
        //    if (x < 1)
        //    {
        //        x += Time.deltaTime;
        //        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - mau * percent);
        //        //		
        //        //		// Set the scale of the health bar to be proportional to the player's health.
        //        healthBar.transform.localScale = new Vector3(healthScale.x * mau * percent, 2, 1);
        //    }
        //}
        //else
        //{
        //    x = 0;
        //}
        //health.transform.localScale = new Vector3(x, 1, 1);
    }
}
