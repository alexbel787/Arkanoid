using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuHandler : MonoBehaviour
{
    public GameObject launchButtonObj;
    public Text scoreText;
    public GameObject levelCompletedMenuObj;
    public GameObject gameOverMenuObj;

    private GameManagerScript GMS;
    [SerializeField] private BallScript BS;

    private void Start()
    {
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        launchButtonObj.SetActive(true);
    }

    public void ScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void LaunchBallButton()
    {
        BS.LaunchBall();
        launchButtonObj.SetActive(false);
    }

    public IEnumerator LevelCompletedCoroutine()
    {
        yield return new WaitForSeconds(1);
        SoundManager.instance.PlaySingle(1, SoundManager.instance.gameSounds[0]);
        Time.timeScale = 0;
        levelCompletedMenuObj.SetActive(true);
    }

    public void NextLevelButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        SoundManager.instance.PlaySingle(1, SoundManager.instance.gameSounds[1]);
        gameOverMenuObj.SetActive(true);
    }
    public void StartAgainButton()
    {
        GMS.score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

}
