using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Data.gameOverUpdate += Spawn;
    }

    private void Spawn(bool gameOver) {
        gameObject.SetActive(gameOver);
    }

    public void StartNewGame(bool isHard) {
        Data.IsHard = isHard;
        Data.GameOver = false;
        Data.Score = 0;
    }
}
