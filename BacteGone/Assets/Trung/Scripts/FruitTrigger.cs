using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTrigger : MonoBehaviour
{


    public delegate void OnSlashFruit(bool isGood);
    public static event OnSlashFruit SlashFruit;
    public Sprite[] goodFruit;
    public Sprite[] goodFruitSlash;
    public Sprite[] badFruit;
    public Sprite[] badFruitSlash;
    public SpriteRenderer spriteRender;
    public SpriteRenderer spriteSlash;
    public bool isGood;
    //public GameObject[] MedicinObject;
    public void Init(bool _isGood)
    {
        isGood = _isGood;

        if (isGood)
        {
            isGood = true;
            int x = Random.Range(0, goodFruit.Length);
            spriteRender.sprite = goodFruit[x];
            spriteSlash.sprite = goodFruitSlash[x];
        }
        else
        {
            int y = Random.Range(0, badFruit.Length);
            spriteRender.sprite = badFruit[y];
            spriteSlash.sprite = badFruitSlash[y];
         isGood = false;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandLeft") || other.CompareTag("HandRight"))
        {
            CatchObject(other);
            GetComponent<Rigidbody>().drag = 4;
        }


    }
    public GameObject yes;
    public GameObject no;


    public void CatchObject(Collider other)
    {
       
        GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Collider>().enabled = false;
        //  LeanTween.scale(gameObject, Vector3.zero, 1f);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        if (SlashFruit != null)
        {
            SlashFruit(isGood);
            if(isGood)
            {
                GSPlaying.Instance.ShowScorePopup(transform.localPosition, 1);
                Instantiate(yes, other.transform.position, Quaternion.identity);
            }
            else
            {
                GSPlaying.Instance.ShowScorePopup(transform.localPosition, -1);
                Instantiate(no, other.transform.position, Quaternion.identity);
            }
           
        }
         
        Destroy(gameObject, 1f);


    }
}
