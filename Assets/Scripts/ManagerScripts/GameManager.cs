using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private InGame ig;

    private GameObject[] runners;

    List<RankingSystem> sortArray = new List<RankingSystem>();

    public int pass;

    public string firstPlace, secondPlace, thirdPlace;

    public bool finish,failed,start;
    private void Awake()
    {
        instance = this;

        runners = GameObject.FindGameObjectsWithTag("Runner");

        ig = FindObjectOfType<InGame>();

        if(PlayerPrefs.GetInt("Level") == 0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }
    }

    void Start()
    {
        for (int i = 0; i < runners.Length; i++)
        {
            sortArray.Add(runners[i].GetComponent<RankingSystem>());
        }
    }

   
    void Update()
    {
        CalculatingRank();
    }

    void CalculatingRank()
    {
        sortArray = sortArray.OrderBy(x => x.counter).ToList();
        switch (sortArray.Count)
        {
            case 3:
                sortArray[0].rank = 3;
                sortArray[1].rank = 2;
                sortArray[2].rank = 1;
                ig.a = sortArray[2].name;
                ig.b = sortArray[1].name;
                ig.c = sortArray[0].name;
                break;
            case 2:
                sortArray[0].rank = 2;
                sortArray[1].rank = 1;

                ig.a = sortArray[1].name;
                ig.b = sortArray[0].name;

                ig.thirdPlaceImg.color = Color.red;
                break;
            case 1:
                sortArray[0].rank = 1;
                if(firstPlace == "")
                {
                    firstPlace = sortArray[0].name;
                    ig.a = sortArray[0].name;
                    ig.secondPlaceImg.color = Color.red;
                    GameUI.instance.OpenLBP();
                } 
                break;
        }

        if(pass >= (float)runners.Length / 2)
        {
            pass = 0;
            sortArray = sortArray.OrderBy(x => x.counter).ToList();

            foreach (RankingSystem rs in sortArray)
            {
                if(rs.rank == sortArray.Count)
                {
                    if(rs.gameObject.name == "Player")
                    {
                        failed = true;
                        GameUI.instance.OpenLBP();
                    }

                    if (thirdPlace == "")
                        thirdPlace = rs.gameObject.name;
                    else if (secondPlace == "")
                        secondPlace = rs.gameObject.name;
                    
                    rs.gameObject.SetActive(false);
                }

            }
            runners = GameObject.FindGameObjectsWithTag("Runner");
            sortArray.Clear();

            for (int i = 0; i < runners.Length; i++)
            {
                sortArray.Add(runners[i].GetComponent<RankingSystem>());
            }

            if(runners.Length < 2)
            {
                finish = true;
                if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("Level"))
                    PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            }
        }
    }
}
