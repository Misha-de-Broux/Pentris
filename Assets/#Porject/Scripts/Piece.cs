using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    const string CUBE_TAG = "Cube";
    [SerializeField] float speed = 1;
    float currentSpeed = 0;
    private PlayMatrix PlayMatrix;

    private void Awake () {
        PlayMatrix = GameObject.FindAnyObjectByType<PlayMatrix>();
        Data.gameOverUpdate += Unload;
        Unload(Data.GameOver);
    }



    private void Unload(bool gameOver) {
        if (gameOver) {
            Data.gameOverUpdate -= Unload;
            Destroy(gameObject);
        }
    }

    public void Fall() {
        foreach(Collider c in gameObject.GetComponentsInChildren<Collider>()) {
            c.isTrigger = true;
        }
        currentSpeed = speed;
    }

    public void Update() {
        transform.Translate(Vector3.down * currentSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other) {
        if (enabled) {
            if (other.CompareTag(CUBE_TAG)) {
                if (other.transform.parent != transform) {
                    List<Cube> cubes = new List<Cube>(GetComponentsInChildren<Cube>());
                    foreach (Cube cube in cubes) {
                        cube.SetInPlayMatrix(PlayMatrix.transform);
                    }
                    PlayMatrix.AddCubes(cubes);
                    Destroy(gameObject);
                }
            }
        }
    }
}
