using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelMove : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("triggered");
        if(other.tag == "Player")
        {
            SceneManager.LoadScene("NovellaSecondScreen");
        }
    }
}
