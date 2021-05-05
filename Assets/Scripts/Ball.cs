using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;
    // Destroy ball
    public void Die()
    {
        OnBallDeath.Invoke(this);
        Destroy(gameObject, 1);
    }
}
