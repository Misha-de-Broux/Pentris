using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    const string CUBE_TAG = "Cube";
    [SerializeField] float speed = 1;
    public float currentSpeed = 0;
    private PlayMatrix PlayMatrix;

    private void Start() {
        PlayMatrix = GameObject.FindAnyObjectByType<PlayMatrix>();
    }
    public void Fall() {
        currentSpeed = speed;
    }

    public void Update() {
        transform.Translate(Vector3.down * currentSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag(CUBE_TAG)) {
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
