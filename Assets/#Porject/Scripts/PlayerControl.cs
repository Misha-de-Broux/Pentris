using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction cubeFall;
    [SerializeField] RandomPieceGenerator pieceGenerator;
    private Piece currentPiece;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        cubeFall = playerInput.actions["cubeFall"];
        cubeFall.performed += ctx => {MakeFall(ctx);};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MakeFall(InputAction.CallbackContext ctx){
        if (currentPiece != null) {
            currentPiece.enabled = true;
            currentPiece.Fall();
        }
        currentPiece = pieceGenerator.GenerateNewPiece();
    }
    void OnEnable(){
        cubeFall.Enable();
    }
    void OnDisable(){
        cubeFall.Disable();
    }
}
