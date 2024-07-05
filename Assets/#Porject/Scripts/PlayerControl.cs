using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerControl : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction cubeFall;
    [SerializeField] RandomPieceGenerator pieceGenerator;
    private Piece currentPiece;
    Bounds playzone;
    [SerializeField] RightWristUI wrist;
    // Start is called before the first frame update
    void Start(){
        PlayMatrix playZoneMatrix = GameObject.FindAnyObjectByType<PlayMatrix>();
        Collider[] playZoneColliders = playZoneMatrix.GetComponentsInChildren<Collider>();
        playzone = new Bounds(playZoneMatrix.transform.position, Vector3.zero);
        foreach (Collider nextCollider in playZoneColliders) {
            playzone.Encapsulate(nextCollider.bounds);
        }
    }
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
            //test if piece held, blocking spawn of other piece if held above grid
            if (currentPiece.GetComponent<XRGrabInteractable>().interactorsSelecting.Count > 0){
                return;
            }
            Collider[] myColliders = currentPiece.GetComponentsInChildren<Collider>();
            Bounds pieceBounds = new Bounds(currentPiece.transform.position, Vector3.zero);
            foreach (Collider nextCollider in myColliders) {
                pieceBounds.Encapsulate(nextCollider.bounds);
            }
            //test if piece is within bounds
            if (pieceBounds.min.x < playzone.min.x || pieceBounds.max.x > playzone.max.x || pieceBounds.min.z < playzone.min.z || pieceBounds.max.z > playzone.max.z){
                return;
            }
            else{
                currentPiece.Fall();
            }
        }
        currentPiece = pieceGenerator.GenerateNewPiece();
        wrist.OnPieceFall();
    }
    void OnEnable(){
        cubeFall.Enable();
    }
    void OnDisable(){
        cubeFall.Disable();
    }
}
