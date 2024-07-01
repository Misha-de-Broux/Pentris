using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] float rowHeight = .2f;
    
    public void Fall(int rows, float timeToFall) {
        StartCoroutine(FallCoroutine(rows, timeToFall));
    }
    
    IEnumerator FallCoroutine(int rows, float timeToFall) {
        Vector3 current = transform.position;
        for(float _ = 0; _ < timeToFall; _+= Time.deltaTime) {
            transform.Translate(Vector3.down *rows * rowHeight *Time.deltaTime / timeToFall);
            yield return null;
        }
        current.y -= rows * rowHeight;
        transform.position = current;
    }
}
