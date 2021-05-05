using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    // Initializing Variables
    private Camera mainCamera;
    private float paddleInitialY;
    private float defaultPaddleWidthInPixels = 200;
    private float defaultLeftClampLimit = 135;
    private float defaultRightClampLimit = 410;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    private void Update()
    {
        PaddleMovement();
    }


    // Movement function for paddle
    private void PaddleMovement()
    {
        // Restricting paddle movement by clamping
        float paddleShift = (defaultPaddleWidthInPixels - ((defaultPaddleWidthInPixels / 2) * this.sr.size.x)) / 2;

        float leftClampLimit = defaultLeftClampLimit - paddleShift;
        float rightClampLimit = defaultRightClampLimit + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClampLimit, rightClampLimit);



        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionWorldX, paddleInitialY, 0);
    }

    // Ball refrence via tag
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            SoundManager.PlaySound("paddlehit");
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;
            float diffrence = paddleCenter.x - hitPoint.x;

            if (hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(diffrence * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(diffrence * 200)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }
}