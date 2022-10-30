using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] Ball ball;

    [SerializeField] AudioSource victoryAudio;
    private void OnTriggerEnter(Collider other)
    {
        victoryAudio.Play(0);
        ball.IsEnteringHole = true;
    }
}
