using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    private Camera cameraMain;
    private int currentPlayer = 0;

    public float speed = .5f;
    public float selectionPosition = 13f;
    public GameObject charParent;

    public Button playBtn, buyBtn, nextBtn, prevBtn;
    public int points = 60;
    public int[] colorPrice;

    private void Awake()
    {
        cameraMain = Camera.main;
        CheckIfBought();
        CameraPos();
    }

    void CameraPos()
    {
        currentPlayer = PlayerPrefs.GetInt("PlayerColor");

        cameraMain.transform.position = new Vector3(cameraMain.transform.position.x + (currentPlayer * 13),
            cameraMain.transform.position.y, cameraMain.transform.position.z);
    }

    public void Play()
    {
        SceneManager.LoadScene("1");
        PlayerPrefs.SetInt("PlayerColor",currentPlayer);
        
    }

    public void Buy()
    {
        switch (currentPlayer)
        {
            case 1:
                if (points > colorPrice[0] && PlayerPrefs.GetInt("BlueBuy",0) == 0)
                {
                    PlayerPrefs.SetInt("BlueBuy",1);
                    points -= colorPrice[0];
                }
                break;
            case 2:
                if (points > colorPrice[0] && PlayerPrefs.GetInt("YellowBuy",0) == 0)
                {
                    PlayerPrefs.SetInt("YellowBuy",1);
                    points -= colorPrice[1];
                }
                break;
            case 3:
                if (points > colorPrice[0] && PlayerPrefs.GetInt("GreenBuy",0) == 0)
                {
                    PlayerPrefs.SetInt("GreenBuy",1);
                    points -= colorPrice[2];
                }
                break;
        }
        CheckIfBought();
    }

    void CheckIfBought()
    {
        buyBtn.interactable = true;
        playBtn.interactable = true;
        buyBtn.transform.GetChild(0).GetComponent<Text>().text = "Buy";
        buyBtn.image.color = Color.blue;
        switch (currentPlayer)
        {
            case 0:
                buyBtn.interactable = false;
                buyBtn.transform.GetChild(0).GetComponent<Text>().text = "Bought";
                break;
            case 1:
                if (PlayerPrefs.GetInt("BlueBuy") == 1)
                {
                    buyBtn.interactable = false;
                    buyBtn.transform.GetChild(0).GetComponent<Text>().text = "Bought";
                }
                else
                {
                    playBtn.interactable = false;
                    if (points >= colorPrice[0])
                        buyBtn.image.color = Color.green;
                    else
                        buyBtn.image.color = Color.red;
                }
                break;
            case 2:
                if (PlayerPrefs.GetInt("YellowBuy") == 1)
                {
                    buyBtn.interactable = false;
                    buyBtn.transform.GetChild(0).GetComponent<Text>().text = "Bought";
                }
                else
                {
                    playBtn.interactable = false;
                    if (points >= colorPrice[1])
                        buyBtn.image.color = Color.green;
                    else
                        buyBtn.image.color = Color.red;
                }
                break;
            case 3:
                if (PlayerPrefs.GetInt("GreenBuy") == 1)
                {
                    buyBtn.interactable = false;
                    buyBtn.transform.GetChild(0).GetComponent<Text>().text = "Bought";
                }
                else
                {
                    playBtn.interactable = false;
                    if (points >= colorPrice[2])
                        buyBtn.image.color = Color.green;
                    else
                        buyBtn.image.color = Color.red;
                }
                break;
        }
    }

    public void Next()
    {
        if (currentPlayer < charParent.transform.childCount - 1)
        {
            currentPlayer++;
            StartCoroutine(MoveTONext());
            CheckIfBought();
        }
    }

    public void Prev()
    {
        if (currentPlayer > 0)
        {
            currentPlayer--;
            StartCoroutine(MoveTOPrev());
            CheckIfBought();
        }
    }

    IEnumerator MoveTONext()
    {
        Vector3 tempPos = new Vector3(cameraMain.transform.position.x + selectionPosition, cameraMain.transform.position.y,
            cameraMain.transform.position.z);
        nextBtn.interactable = false;
        prevBtn.interactable = false;
        while (cameraMain.transform.position.x < tempPos.x)
        {
            cameraMain.transform.position = Vector3.MoveTowards(cameraMain.transform.position, tempPos, speed * .5f) ;
            yield return new WaitForSeconds(Time.deltaTime * speed);
        }
        nextBtn.interactable = true;
        prevBtn.interactable = true;
        yield return null;
    }
    
    IEnumerator MoveTOPrev()
    {
        Vector3 tempPos = new Vector3(cameraMain.transform.position.x - selectionPosition, cameraMain.transform.position.y,
            cameraMain.transform.position.z);
        nextBtn.interactable = false;
        prevBtn.interactable = false;
        while (cameraMain.transform.position.x > tempPos.x)
        {
            cameraMain.transform.position = Vector3.MoveTowards(cameraMain.transform.position, tempPos, speed * .5f) ;
            yield return new WaitForSeconds(Time.deltaTime * speed);
        }
        nextBtn.interactable = true;
        prevBtn.interactable = true;
        yield return null;
    }
}
