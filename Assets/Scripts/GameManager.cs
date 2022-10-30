using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] TMP_Text bestParText;
    [SerializeField] PlayerController player;
    [SerializeField] Ball ball;
    
    void Start()
    {
        gameOverPanel.SetActive(false);
    }
    
    void Update()
    {

        if (ball.IsEnteringHole && gameOverPanel.activeInHierarchy == false)
        {
            gameOverPanel.SetActive(true);
            int sceneNumber = int.Parse((SceneManager.GetActiveScene().name).Split("Stage")[1]);
            gameOverText.text = "Stage " + sceneNumber + " Par: " + player.ShootCount;

            string stageParKey = SceneManager.GetActiveScene().name;

            if (PlayerPrefs.HasKey(stageParKey))
            {
                if (player.ShootCount < PlayerPrefs.GetInt(stageParKey))
                    PlayerPrefs.SetInt(stageParKey, player.ShootCount);
            } else
            {
                PlayerPrefs.SetInt(stageParKey, player.ShootCount);
            }

            bestParText.text = "Best: " + PlayerPrefs.GetInt(stageParKey);

        }
    }

    public void BackToMainMenu()
    {
        SceneLoader.Load("MainMenu");
    }

    public void Replay()
    {
        SceneLoader.ReloadStage();
    }

    public void PlayNext()
    {
        SceneLoader.LoadNextStage();
    }
}
