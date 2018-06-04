using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductGame2 : MonoBehaviour
{


    public int medicinID;
    public delegate void OnChooseObject(int id, ProductGame2 obj);
    public static event OnChooseObject OnchooseObject;
    public Sprite[] spriteImage;
    public SpriteRenderer spriteRender;
    public int handStateLeft;
    public int handStateRight;
    public Sprite[] leftHandImage;
    public Sprite[] rightHandImage;
    public SpriteRenderer leftHandSprite;
    public SpriteRenderer rightHandSprite;  
    public List<Sprite[]> listSprite;
    public SpriteImage[] listSpriteMedicin;
    public Sprite[] spriteGanaton;
    public Sprite[] spriteDupatal;


    public void InitObject(int id)
    {
        medicinID = id;
        spriteRender.sprite = spriteImage[medicinID];

        ProcessImageMedicin(id);

        handStateLeft = Random.Range(2, 4);// chir laay so 2,3 nen phai tru di 2
        handStateRight = Random.Range(2, 4);
        leftHandSprite.sprite = leftHandImage[handStateLeft-2];
        rightHandSprite.sprite = rightHandImage[handStateRight-2];

        //if (Localization.language == "en")
        //{
        //    if (id == 2)
        //    {
        //        spriteRender.sprite = spriteGanaton[Random.Range(0, spriteGanaton.Length)];
        //    }
        //    if (id == 5)
        //    {
        //        spriteRender.sprite = spriteDupatal[Random.Range(0, spriteGanaton.Length)];
        //    }
        //}
    }
    public void ProcessImageMedicin(int id)
    {
        SpriteImage curentMedicin = listSpriteMedicin[id];
        if (curentMedicin.listSprite1.Length > 0)
        {
            int a = Random.Range(0, curentMedicin.listSprite1.Length);
            spriteRender.sprite = curentMedicin.listSprite1[a];
        }
    }
}
