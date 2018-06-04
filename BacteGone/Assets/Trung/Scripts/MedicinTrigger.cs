using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class MedicinTrigger : MonoBehaviour
{
    public int medicinID;     
    public Sprite[] spriteImage;
    public SpriteRenderer spriteRender;
    public float speed = 1;  
    public SpriteRenderer[] spriteRenderDoi;

    public Sprite[] spriteGanaton;
    public Sprite[] spriteDupatal;

    public BoxCollider boxCollider;

    public void Init(int id, float _speed = 1.5f)
    {
        medicinID = id;      
        speed = _speed;
        ProcessImageMedicin(id);    
    }

    public void ProcessImageMedicin(int id)
    {
        int numimg = Random.Range(0, ControlImage.instance.listImage[id].spriteImage.Count);
        spriteRender.sprite = ControlImage.instance.listImage[id].spriteImage[numimg];
       
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.center = spriteRender.sprite.bounds.center;
        boxCollider.size = new Vector3(spriteRender.sprite.bounds.size.x,
            spriteRender.sprite.bounds.size.y,
            1);
    }

  

    private void Update()
    {
        if (isMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.localPosition.x, -10, transform.localPosition.z), step);
        }
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    public bool isMove = true;

  
}

[Serializable]
public class SpriteImage
{
    public Sprite[] listSprite1;
    public Sprite[] listSprite2;
}