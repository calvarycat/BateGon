using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    public GameObject[] gamePlay;
    public GameObject Intestinalsystem;
    public Game1Manager game1;
    public int score;
    public AudioClip clipGame1;
    public AudioClip finishGame;
    public int currentGame;
    public AudioClip bgGame1;

    public AudioSource audio;

    void OnEnable()
    {
        GSPlaying.Instance.OnTutorialFinish += OnTutorialFinish;
    }

    void OnDisable()
    {
        GSPlaying.Instance.OnTutorialFinish -= OnTutorialFinish;
    }


    
    public GameObject initestalSystem;
    void ShowIniteresSystem()
    {
        // Debug.Log(KinectManager.Instance.isWaitingForUser);

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
        if (currentGame == 3)
        {
            if (initestalSystem.activeSelf)
            {
                initestalSystem.SetActive(false);
            }

        }

    }

    void OnTutorialFinish()
    {     
        GamePlay1Click();
    }

    public void GamePlay1Click()
    {

        Debug.Log("Game Play click");
        AudioManager.PlayMusic(bgGame1);
        currentGame = 1;
        score = 0;
        DeleteAfterplay();
        game1.gameObject.SetActive(true);
        game1.Init(60);     
        
    }


    public void SetActiveGame(int gameID)
    {

        for (int i = 0; i < 3; i++)
        {
            gamePlay[i].SetActive(false);
        }
        gamePlay[gameID].SetActive(true);
    }

    public void OnShowResult()
    {
        AudioManager.StopMusic();
        AudioManager.PlaySound(finishGame);
        GSPlaying.Instance.HideHandTrail();
        for (int i = 0; i < 1; i++)
        {
            gamePlay[i].SetActive(false);
        }
        if (score < 0)
            score = 0;
        if (score > 100)
            score = 100;
        GSPlaying.Instance.ShowResult((float)score);
    }
    public void DeleteAfterplay()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("MedicineGameTag");
        if (obj != null)
            foreach (GameObject ob in obj)
            {
                Destroy(ob);
            }
    }
    public void OnChangeScore(int _score)
    {
        score += _score;
    }
    float CaculateScore(int score)
    {
        float result = 0;


        if (score >= 150)
            return 100;
        if (score > 130)
        {
            float rs = ((score - 130) * 20) / 20;
            result = 80 + rs;
        }
        if (score > 25)
        {
            float rs = ((score - 25) * 30) / 105;
            result = 50 + rs;
        }
        else
        {
            float rs = (score * 50) / 25;
            result = rs;
        }
        return result;
    }


    public void Update()
    {
        Cheat();
    }
    void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
          OnShowResult();
            GamePlay1Click();
        }

    }
}
public enum MedicinName
{
    None = 0,
    Transmetin = 1, //vàng da
    KlacidForte = 2, // Loét dạ dày
    Ganaton = 3,//dầy bụng khó tiêu
    Creon = 4,//Suy dinh dưỡng
    Hidrasec = 5,//Tiêu chảy
    Duspatal = 6,//Hội chứng ruột kích thích
    Dicetel = 7,//Hội chứng ruột kích thích
    DuphalacTntestine = 8,//Táo bón ruột
    DuphalacLiver = 9,//Táo bón hôn mê
}