using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAlignment : MonoBehaviour
{
    //order of angles: forward, back, up, down, left, right 
    Vector3[] allVectors = new Vector3[6]{Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.left, Vector3.right};
    // Start is called before the first frame update

    Bounds playzone;
    void Start()
    {
        PlayMatrix playZoneMatrix = GameObject.FindAnyObjectByType<PlayMatrix>();
        Collider[] playZoneColliders = playZoneMatrix.GetComponentsInChildren<Collider>();
        playzone = new Bounds(playZoneMatrix.transform.position, Vector3.zero);
        foreach (Collider nextCollider in playZoneColliders) {
            Debug.Log(nextCollider.bounds.size);
            playzone.Encapsulate(nextCollider.bounds);
        }

        //for testing purpose
        //InvokeRepeating("SnapOnRelease", 2f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SnapOnRelease(){
        //test angle forward
        float minAngle = Mathf.Infinity;
        var minVectorFwd = Vector3.zero;
        foreach(Vector3 vec in allVectors) {
            // calculate minimum angle
            var angle = Vector3.Angle(transform.forward, vec);
            if (angle < minAngle) {
                minAngle = angle;
                minVectorFwd = vec;
            }
        }

        //test angle up
        minAngle = Mathf.Infinity;
        var minVectorUp = Vector3.zero;
        foreach(Vector3 vec in allVectors) {
            // calculate minimum angle
            var angle = Vector3.Angle(transform.up, vec);
            if (angle < minAngle) {
                minAngle = angle;
                minVectorUp = vec;
            }
        }
        //minVector is now the vector with the smallest angle up
        //snap minVector at the same moment for both
        transform.rotation = Quaternion.LookRotation(minVectorFwd, minVectorUp);
    }
    public void SnapXZ(){
        Vector3 correctedPos = transform.position;
        
        

        Collider[] myColliders = GetComponentsInChildren<Collider>();
        Bounds myBounds = new Bounds(transform.position, Vector3.zero);
        foreach (Collider nextCollider in myColliders) {
            myBounds.Encapsulate(nextCollider.bounds);
        }
        correctedPos.y -= (myBounds.min.y - playzone.max.y);
        correctedPos.y -= correctedPos.y%0.1f;

        

        Debug.Log($"playzone max : {playzone.max}, min : {playzone.min}\nbounds max : {myBounds.max}, min : {myBounds.min}");
        if(myBounds.min.x < playzone.min.x) {
            correctedPos.x -= (myBounds.min.x - playzone.min.x);
        }
        if (myBounds.max.x > playzone.max.x) {
            correctedPos.x -= (myBounds.max.x - playzone.max.x);
        }
        if (myBounds.min.z < playzone.min.z) {
            correctedPos.z -= (myBounds.min.z - playzone.min.z);
        }
        if (myBounds.max.z > playzone.max.z) {
            correctedPos.z -= (myBounds.max.z - playzone.max.z);
        }

        if (correctedPos.x % 0.2 != 0) {
            if (correctedPos.x % 0.2 < 0.1) {
                correctedPos.x -= correctedPos.x % 0.2f;
            } else {
                correctedPos.x -= correctedPos.x % 0.2f - 0.2f;
            }
        }
        if (correctedPos.z % 0.2 != 0) {
            if (correctedPos.z % 0.2 < 0.1) {
                correctedPos.z -= correctedPos.z % 0.2f;
            } else {
                correctedPos.z -= correctedPos.z % 0.2f - 0.2f;
            }
        }
        transform.position = correctedPos;
    }
}
