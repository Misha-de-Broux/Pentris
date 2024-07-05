using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomPieceGenerator : MonoBehaviour
{
    [SerializeField]private GameObject[] poolOfPiece;
    public Stack<GameObject> pool = new();
    private List<int> indexes = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        //populate list of index for random generation of pieces 
        for(int i = 0; i < poolOfPiece.Count(); i++){
            indexes.Add(i);
        }
        ReplenishPool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Piece GenerateNewPiece(){
        Vector3 position = new Vector3(2,1,0);
        Quaternion rotation = Quaternion.identity;
        if(pool.Count != 0){
            return Instantiate(pool.Pop(), position, rotation).GetComponent<Piece>();
        }
        else{
            ReplenishPool();
            return GenerateNewPiece();
        }
    }
    void ReplenishPool(){
        //Randomize the list of indexes
        for (int i = indexes.Count - 1; i > 0; i--){
            int k = Random.Range(0 , i + 1);
            int value = indexes[k];
            indexes[k] = indexes[i];
            indexes[i] = value;
        }
        //replenish the pool of pieces with the randomize pieces via the list of randomized numbers
        for(int i = 0; i < indexes.Count; i++){
            pool.Push(poolOfPiece[indexes[i]]);
        }
    }
}
