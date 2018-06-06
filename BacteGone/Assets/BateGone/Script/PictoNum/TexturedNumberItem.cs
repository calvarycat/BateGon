using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TexturedNumberItem : MonoBehaviour
{
    public Image MainImage;
    public Sprite[] NumberArray = new Sprite[10];

    public void Init(int value)
    {
        if(value>=0)
        MainImage.sprite = NumberArray[value];
    }
}