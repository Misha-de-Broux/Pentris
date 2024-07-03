using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    public IEnumerator FallCoroutine(int rows, float timeToFall) {
        Debug.Log("Fall");
        Vector3 current = transform.position;
        for (float _ = 0; _ < timeToFall; _ += Time.deltaTime) {
            transform.Translate(Vector3.down * rows * transform.localScale.y * Time.deltaTime / timeToFall, Space.World);
            yield return null;
        }
        current.y -= rows * transform.localScale.y;
        transform.position = current;
    }


    public IEnumerator KillCoroutine(float timeToDie) {
        Debug.Log("kill");
        Renderer[] renderers = GetComponents<Renderer>();
        foreach (Renderer renderer in renderers) {
            renderer.enabled = false;
        }
        yield return new WaitForSeconds(timeToDie / 4);
        foreach (Renderer renderer in renderers) {
            renderer.enabled = true;
        }
        yield return new WaitForSeconds(timeToDie / 4);
        foreach (Renderer renderer in renderers) {
            renderer.enabled = false;
        }
        yield return new WaitForSeconds(timeToDie / 4);
        foreach (Renderer renderer in renderers) {
            renderer.enabled = true;
        }
        //StartCoroutine(DestroyCoroutine(timeToDie/4));
        yield return new WaitForSeconds(timeToDie / 4);
        Destroy(gameObject);
    }

    private IEnumerator DestroyCoroutine(float countdown) { 
        yield return new WaitForSeconds(countdown);
        Debug.Log("Destroy");
        Destroy(gameObject);
    }

    public void SetInPlayMatrix(Transform playMatrix) {
        transform.parent = playMatrix;
        Vector3 position = transform.position;
        position.y -= position.y % 0.2f - 0.1f;
        transform.position = position;
    }
}
