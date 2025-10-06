using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_Text hiscoretxt, scoretxt, livestxt;

    public GameObject gameOver;
    int scor=0;
    int hiscor=0;
    int lives = 5;

    public static GameManager instance;

    public EnemySpawnner enemySpawnner;
    public PlayerMovement playermov;
    public ShootContinous shooter;
    public ShipController ship;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        gameOver.SetActive(false);
        hiscor = PlayerPrefs.GetInt("Hiscore", 0);
        hiscoretxt.text = hiscor.ToString();
        livestxt.text = lives.ToString();
        scor = 0;
        scoretxt.text = scor.ToString();
    }

    public void UpdateScore(int score)
    {
        scor+=score;

        scoretxt.text = scor.ToString();

        if (scor > hiscor)
        {
            hiscor=scor;
            hiscoretxt.text = hiscor.ToString();
            PlayerPrefs.SetInt("Hiscore", hiscor);
        }

    }

    public void Lifelost()
    {
        ResetGame();
        if(lives > 0)
        {
            lives--;
            livestxt.text = lives.ToString();
        }
        else
        {
            GameOver();
        }
        

    }

    void ResetGame()
    {
        playermov.gameObject.transform.position = Vector3.zero;
        playermov.gameObject.transform.rotation = Quaternion.identity;
        enemySpawnner.DisableAll();
    }


    void GameOver()
    {
        ResetGame();
        enemySpawnner.enabled = false;
        playermov.enabled = false;
        shooter.enabled = false;
        ship.enabled = false;
        gameOver.SetActive(true);
    }

    public void RespawnShip()
    {
        ship.gameObject.SetActive(true);
    }
    
}
