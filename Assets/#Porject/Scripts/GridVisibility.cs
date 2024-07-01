using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridVisibility : MonoBehaviour {
    [SerializeField] float decay = 0.3f;
    Material gridMaterial;
    Dictionary<GridVisibility, float> neighbours = new Dictionary<GridVisibility, float>();
    float visibility = 0;
    int cubes = 0;
    // Start is called before the first frame update
    void Start() {
        gridMaterial = GetComponent<Renderer>().material;
        LayerMask mask = LayerMask.GetMask("Grid");
        for (int i = 0; i < 6; i++) {
            Vector3 direction = new Vector3(i == 0 ? 1 : i == 1 ? -1 : 0, i == 2 ? 1 : i == 3 ? -1 : 0, i == 4 ? 1 : i == 5 ? -1 : 0);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 0.1f, mask)) {
                neighbours.Add(hit.collider.GetComponent<GridVisibility>(), 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Cube")) {
            cubes++;
            visibility = 1;
            StartCoroutine(SetVisibility(null, 1));
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Cube")) {
            cubes--;
            if (cubes == 0) {
                visibility = 0;
                StartCoroutine(SetVisibility(null, visibility));
            }
        }
    }

    private IEnumerator SetVisibility(GridVisibility neighbour, float visibility) {
        yield return null;
        float oldVisibility = Mathf.Max(neighbours.Values.Max(), this.visibility);
        if (neighbour != null) {
            neighbours[neighbour] = visibility > 0 ? visibility : 0;
        }
        float maxVisibility = Mathf.Max(neighbours.Values.Max(), this.visibility);
        if (oldVisibility != maxVisibility || neighbour == null) {
            SetMaterialVisibility(maxVisibility);
            foreach (GridVisibility next in neighbours.Keys) {
                StartCoroutine(next.SetVisibility(this, maxVisibility - decay));
            }
        }
    }


    private void SetMaterialVisibility(float visibility) {
        gridMaterial.SetFloat("_Visibility", visibility);
    }
}
