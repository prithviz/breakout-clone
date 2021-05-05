using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Target;
    public Text ScoreText;
    public Text LivesText;

    public int Score { get; set; }

    private void Start()
    {
        // invoking on live lost event to update score text on start/restart
        OnLiveLost(GameManager.Instance.AvailableLives);
    }
    private void Awake()
    {
        // Subscribe to event of brick destruction
        Brick.OnBrickDestruction += OnBrickDestriction;
        // Subscribe to event of new level load 
        BricksManager.OnLevelLoaded += OnLevelLoaded;
        // Subscribe to event when a life is lost
        GameManager.OnLiveLost += OnLiveLost;
    }

    private void OnDisable()
    {
        // unscribe all the subscribed events when this gameobject disabled
        Brick.OnBrickDestruction -= OnBrickDestriction;
        BricksManager.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnLiveLost(int remainingLives)
    {
        LivesText.text = $"LIVES{Environment.NewLine}{remainingLives}";
    }

    private void OnLevelLoaded()
    {
        // Update Remaining bricks text after level is started/restarted
        UpdateRemaingBricksText();
        // Update Score text to zero after level is started/restarted
        UpdateScoreText(0);
        
    }

    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = $"SCORE{Environment.NewLine}{scoreString}";
    }

    private void OnBrickDestriction(Brick obj)
    {
        // everytime when brick is destroyed update the score & remaining bricks
        UpdateRemaingBricksText();
        UpdateScoreText(10);
    }

    private void UpdateRemaingBricksText()
    {

        Target.text = $"TARGET{Environment.NewLine}{BricksManager.Instance.RemainingBricks.Count}/{BricksManager.Instance.initialBricksCount}";
    }

}
