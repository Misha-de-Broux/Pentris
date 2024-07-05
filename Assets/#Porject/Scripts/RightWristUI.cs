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
        GameObject piece = generator.pool.Peek().gameObject;
        Vector3 position = new Vector3(0,-.2f,0);
        Quaternion rotation = Quaternion.identity;
        Instantiate(piece, position, rotation);
    }
}
