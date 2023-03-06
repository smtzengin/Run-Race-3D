using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public GameObject inGame, leaderBoardPanel;

    private Button nextLevel;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (GameManager.instance.failed)
        {
            if (leaderBoardPanel.activeInHierarchy)
            {
                GameManager.instance.failed = false;
                Restart();
            }
        }
        
        
    }

    

    public void OpenLBP()
    {
        inGame.SetActive(false);
        leaderBoardPanel.SetActive(true);
        //current level = current level
    }
    public void Restart()
    {
        nextLevel = GameObject.Find("/GameUI/LeaderBoardPanel/NextLevel").GetComponent<Button>();
        nextLevel.onClick.RemoveAllListeners();
        nextLevel.onClick.AddListener(() => Reload());
        nextLevel.transform.GetChild(0).GetComponent<Text>().text = "Again";
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
