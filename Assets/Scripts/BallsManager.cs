using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    #region Singleton

    private static BallsManager _instance;

    public static BallsManager Instance => _instance;

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

    [SerializeField]
    private Ball ballPrefab;
    private Ball initialBall;
    private Rigidbody2D initialBallRb;
    public GameObject startGameScreen;
    public float initialBallSpeed = 250;

    public List<Ball> Balls { get; set; }

    // Initializes the Ball instance
    private void InitBall()
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + 0.27f, 0);
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initialBallRb = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            initialBall
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        InitBall();
    }

    internal void ResetBalls()
    {
        foreach (var ball in this.Balls.ToArray()) // Error here on ToList()
        {
            Destroy(ball.gameObject);
        }

        InitBall();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!GameManager.Instance.IsGameStarted && startGameScreen.active == false)
        {
            // put ball to the paddle position
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + 0.27f, 0);
            initialBall.transform.position = ballPosition;

            if (Input.GetMouseButtonDown(0))
            {
                initialBallRb.isKinematic = false;
                initialBallRb.AddForce(new Vector2(0, initialBallSpeed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }
}
