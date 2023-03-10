using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public GameObject inGame, leaderBoardPanel;
    

    public Text currentLevelText, nextLevelText;
    public Image fill;
    public Sprite orange, gray;

    private Button nextLevel;
    public Text countText;

    private void Awake()
    {
        instance = this;
        StartCoroutine(StartGame());
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

        print(PlayerPrefs.GetInt("Level"));
   
    }

    IEnumerator StartGame()
    {
        for(int i = 3; i >= 0; i--)
        {
            if(i == 0)
            {
                countText.text = "GO!";
                countText.color = Color.magenta;
            }
            else
            {
                countText.text = i.ToString();
                yield return new WaitForSeconds(1);
                countText.color = Color.magenta;
            }  
        }
        GameManager.instance.start = true;
        yield return new WaitForSeconds(.5f);
        countText.gameObject.SetActive(false);
    }
    

    public void OpenLBP()
    {
        inGame.SetActive(false);
        leaderBoardPanel.SetActive(true);
        if (GameManager.instance.failed)
        {
            currentLevelText.text = PlayerPrefs.GetInt("Level", 1).ToString();
            nextLevelText.text = PlayerPrefs.GetInt("Level",1) + 1 + "";
            fill.sprite = gray;
        }
        
        else
        {
            currentLevelText.text = PlayerPrefs.GetInt(("Level"), 1) - 1 + "";
            nextLevelText.text = PlayerPrefs.GetInt("Level", 1).ToString();
            fill.sprite = orange;
        }
        
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
