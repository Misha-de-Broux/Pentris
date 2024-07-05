using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerControl : MonoBehaviour {
    private PlayerInput playerInput;
    private InputAction cubeFall;
    [SerializeField] RandomPieceGenerator pieceGenerator;
    private Piece currentPiece;
    private Coroutine autoFall;
    private bool waitingForPieceToFall = false;
    Bounds playzone;
    [SerializeField] RightWristUI wrist;
    // Start is called before the first frame update
    void Start() {
        PlayMatrix playZoneMatrix = GameObject.FindAnyObjectByType<PlayMatrix>();
        Collider[] playZoneColliders = playZoneMatrix.GetComponentsInChildren<Collider>();
        playzone = new Bounds(playZoneMatrix.transform.position, Vector3.zero);
        foreach (Collider nextCollider in playZoneColliders) {
            playzone.Encapsulate(nextCollider.bounds);
        }
    }
    void Awake() {
        playerInput = GetComponent<PlayerInput>();
        cubeFall = playerInput.actions["cubeFall"];
        cubeFall.performed += ctx => { MakeFall(ctx); };
    }

    // Update is called once per frame
    void Update() {

    }
    public void MakeFall(InputAction.CallbackContext ctx) {
        MakeFall();
    }

    private void MakeFall() {
        if(currentPiece == null) {
            GetNewPiece();
        } else if (!waitingForPieceToFall) {
            currentPiece.GetComponent<XRGrabInteractable>().enabled = false;
            ForceAlignment pieceAlignment = currentPiece.GetComponent<ForceAlignment>();
            pieceAlignment.SnapOnRelease();
            pieceAlignment.SnapXZ();
            currentPiece.enabled = true;
            currentPiece.Fall();
            StopCoroutine(autoFall);
            waitingForPieceToFall = true;
        }

    }

    void GetNewPiece() {
        if (!Data.GameOver) {
            currentPiece = pieceGenerator.GenerateNewPiece();
            currentPiece.FallEnded += GetNewPiece;
            waitingForPieceToFall = false;
            autoFall = StartCoroutine(AutoFall());
            wrist.OnPieceFall();
        } else {
            Destroy(currentPiece?.gameObject);
        }
    }

    IEnumerator AutoFall() {
        yield return new WaitForSeconds(Data.FallSpeed);
        MakeFall();
    }


    void OnEnable() {
        cubeFall.Enable();
    }
    void OnDisable() {
        cubeFall.Disable();
    }
}
