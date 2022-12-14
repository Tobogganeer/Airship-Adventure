using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PopUp.Show("You win!");
            HUD.SetBlack(true);
            Timer.Create(3f, () => SceneManager.LoadLevel(Level.MainMenu));
        }
    }
}
