using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static AudioClip introSound;
    public static AudioClip paddleHitSound;
    public static AudioClip bricHitkSound;
    public static AudioClip wallHitSound;
    public static AudioClip gameOverSound;
    public static AudioClip victorySound;

    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        introSound = Resources.Load<AudioClip>("intro music");
        paddleHitSound = Resources.Load<AudioClip>("paddle sound");
        bricHitkSound = Resources.Load<AudioClip>("brick sound");
        wallHitSound = Resources.Load<AudioClip>("wall sound");
        gameOverSound = Resources.Load<AudioClip>("game over");
        victorySound = Resources.Load<AudioClip>("wonsound");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "intro":
                audioSrc.PlayOneShot(introSound);
                break;
            case "paddlehit":
                audioSrc.PlayOneShot(paddleHitSound);
                break;
            case "brickhit":
                audioSrc.PlayOneShot(bricHitkSound);
                break;
            case "wallhit":
                audioSrc.PlayOneShot(wallHitSound);
                break;
            case "gameover":
                audioSrc.PlayOneShot(gameOverSound);
                break;
            case "victory":
                audioSrc.PlayOneShot(victorySound);
                break;
        }
    }
}
