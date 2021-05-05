using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Singleton
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public GameObject startGameScreen;
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public int AvailableLives = 3;
    public bool IsGameStarted { get; set; }
    public int Lives { get; set; }

    public static event Action<int> OnLiveLost;


    // Start is called before the first frame update
    void Start()
    {
        this.Lives = this.AvailableLives;
        Screen.SetResolution(540, 960, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    // Game menu start button
    public void PlayGame()
    {
        startGameScreen.SetActive(false);
    }

    // Check victory by counting remaining bricks
    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;
            if (this.Lives < 1)
            {
                // Game over 
                // Show Gameover screen
                SoundManager.PlaySound("gameover");
                gameOverScreen.SetActive(true);
            }
            else
            {
                // Reset balls
                // stop game play
                // reload the level
                // invoking the lost live event
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
                OnLiveLost?.Invoke(this.Lives);
            }
        }
    }

    // Show Victoryscreen 
    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }

}
