using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class RightWristUI : MonoBehaviour
{
    [SerializeField] TMP_Text nextPieceText;
    private RandomPieceGenerator generator;
    // Start is called before the first frame update
    void Start()
    {
        generator = FindAnyObjectByType<RandomPieceGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnPieceFall(){
        Piece piece = generator.pool.Peek().gameObject;
    }
}
