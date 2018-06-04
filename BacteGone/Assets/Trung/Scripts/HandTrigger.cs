using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandTrigger : MonoBehaviour
{
    public delegate void OnCatchingACtion(int id);
    public static event OnCatchingACtion OnCatchingObject;
    public bool handTrigger;
    public int currentProductID = -1;
 
     private SpriteRenderer _spriteRenderer;
    public GameController gs;
    public Game2Manager game2;
    public List<Sprite> listSpriteHoatchat;
    public SpriteRenderer spriteHoatChat;


    void OnEnable()
    {

        Game2Manager._OnNewSymtom += OnNewSymtom;

    }
    void OnDisable()
    {
        Game2Manager._OnNewSymtom -= OnNewSymtom;

    }
    void OnNewSymtom()
    {


    }
    public void ChangeHoatChat(int id)
    {
        spriteHoatChat.sprite = listSpriteHoatchat[id];
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MedicineGameTag"))
        {


            CatchObject(other);
        }
    }


    public void CatchObject(Collider other)
    {      
        MedicinTrigger pro = other.gameObject.GetComponent<MedicinTrigger>();
        if (pro)
        {
            currentProductID = pro.medicinID;
            pro.isMove = false;

            other.GetComponent<Collider>().enabled = false;
            LeanTween.scale(other.gameObject, Vector3.zero, .5f);
            LeanTween.moveY(other.gameObject, other.gameObject.transform.position.y - 1, .5f);
            if (OnCatchingObject != null)
                OnCatchingObject(pro.medicinID);
            Destroy(other.gameObject, 2f);
        }
    }


    protected virtual void Update()
    {

    }
}