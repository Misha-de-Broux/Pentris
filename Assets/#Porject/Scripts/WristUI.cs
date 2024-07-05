using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WristUI : MonoBehaviour
{
    [SerializeField] TMP_Text score;
    // Start is called before the first frame update
    void Start()
    {
        score.SetText($"0");
        Data.ScoreUpdate+=UpdateScore;
    }

    void UpdateScore(int newScore){
        score.SetText($"{newScore}");
    }
}
