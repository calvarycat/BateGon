using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home3d : MonoBehaviour
{

    // Use this for initialization
    public delegate void UpdateOfGSPlaying();
    public event UpdateOfGSPlaying OnUpdateOfGSPlaying;
    public GameObject initestalSystem;
    public GameController gs;
    public GameObject[] hands;
    public Game2Manager game2;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShowIniteresSystem();
        //
    }
    void ShowIniteresSystem()
    {
        if (KinectManager.Instance.GetAllUserIds().Count > 0)
        {
            if (!initestalSystem.activeSelf)
            {
                initestalSystem.SetActive(true);
            }
        }
        else
        {
            if (initestalSystem.activeSelf)
            {
                initestalSystem.SetActive(false);
            }

        }
        if (gs.currentGame == 3)
        {
            if (initestalSystem.activeSelf)
            {
                initestalSystem.SetActive(false);
            }

        }
        if (game2.gameObject.activeSelf && game2.isCountDown)
        {
            if (initestalSystem.activeSelf)
            {
                initestalSystem.SetActive(false);
            }

        }

    }
    void ShowHideHands(bool isShow)
    {
     
        if (hands[0].activeSelf == !isShow || hands[0].activeSelf == !isShow)
        {
            hands[0].SetActive(isShow);
            hands[1].SetActive(isShow);
        }


    }
}
