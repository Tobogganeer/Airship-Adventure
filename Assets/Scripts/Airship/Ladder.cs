using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    float lastTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.HasTag("Player"))
        {
            PlayerMovement.instance.touchingLadder = true;
            PlayerMovement.instance.ladderDir = PlayerMovement.Position.Flattened().DirectionTo(transform.position.Flattened());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.HasTag("Player"))
        {
            lastTime = 0.25f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.HasTag("Player"))
        {
            PlayerMovement.instance.touchingLadder = false;
            PlayerMovement.instance.ladderDir = Vector3.zero;
        }
    }

    private void Update()
    {
        lastTime -= Time.deltaTime;

        if (lastTime < 0)
        {
            PlayerMovement.instance.touchingLadder = false;
            PlayerMovement.instance.ladderDir = Vector3.zero;
        }
    }
}
