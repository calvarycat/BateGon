using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlImage : MonoBehaviour {
    public static ControlImage instance;
    public List<ImgVirus> listImage;
    public Sprite imageAllRight;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

}
[System.Serializable]
public class ImgVirus
{
    public int id;
    public Sprite spriteHoatChat;
    public  List<Sprite> spriteImage;
}
