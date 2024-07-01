using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    const string CUBE_TAG = "Cube";
    [SerializeField] float speed = 1;
    public float currentSpeed = 0;
    public void Fall() {
        currentSpeed = speed;
    }

    public void Update() {
        transform.Translate(Vector3.down * currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag(CUBE_TAG)) {
            if (other.transform.parent != transform) {
                List<Transform> cubes = new List<Transform>(GetComponentsInChildren<Transform>());
                foreach (Transform cube in cubes) {
                    cube.parent = transform.parent;
                    Vector3 position = cube.position;
                    position.y -= position.y % 0.2f - 0.1f;
                    cube.position = position;
                }
                Destroy(gameObject);
            }
        }
    }
}
