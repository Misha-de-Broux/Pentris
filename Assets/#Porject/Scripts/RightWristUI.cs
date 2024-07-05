using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class RightWristUI : MonoBehaviour
{
    private RandomPieceGenerator generator;
    GameObject piece;
    // Start is called before the first frame update
    void Start()
    {
        generator = FindAnyObjectByType<RandomPieceGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPieceFall(){
        if(piece !=null){
            Destroy(piece);
        }
        piece = Instantiate(generator.pool.Peek(), transform.position, transform.rotation, transform);
        foreach(Collider collider in piece.GetComponentsInChildren<Collider>())
            Destroy(collider);
        piece.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
