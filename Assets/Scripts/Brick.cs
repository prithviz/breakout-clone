using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    public int HitPoints = 1;
    public const string LAYER_NAME = "Brick";
    private SpriteRenderer sr;
    public ParticleSystem DestroyEffect;

    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
    }

    // Collision Detaction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }
    // Start is called before the first frame update
    //void Start()
    //{
    //    this.sr = this.GetComponent<SpriteRenderer>();
    //    this.sr.sprite = BricksManager.Instance.Sprites[this.HitPoints - 1]; // set in init
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        this.HitPoints--;
        if(this.HitPoints <= 0)
        {
            // Removing all the bricks from Remainig bricks collection
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            // playing sound brickhit
            SoundManager.PlaySound("brickhit");
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            // chnage the sprite
            this.sr.sprite = BricksManager.Instance.Sprites[this.HitPoints - 1];
        }
    }

    // Adding Effect of Brick Destroy on collision with ball
    private void SpawnDestroyEffect()
    {
        Vector3 brickPosition = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPosition.x, brickPosition.y, brickPosition.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitPoints)
    {
        //
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;

        this.sr.sortingLayerName = LAYER_NAME;
        //this.sr.color = color;
        this.HitPoints = hitPoints;
    }
}
