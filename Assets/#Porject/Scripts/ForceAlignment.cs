using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAlignment : MonoBehaviour
{
    //order of angles: forward, back, up, down, left, right 
    Vector3[] allVectors = new Vector3[6]{Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.left, Vector3.right};
    // Start is called before the first frame update
    void Start()
    {
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
        Vector3 correctedPos= new Vector3 (0,2f,0);
        if (transform.position.x % 0.2 != 0) {
            if(transform.position.x % 0.2 < 0.1){
                correctedPos.x = transform.position.x - (transform.position.x % 0.2f);
            }
            else{
                correctedPos.x = transform.position.x - (transform.position.x % 0.2f) + 0.2f;
            }
        }
        if (transform.position.z % 0.2 != 0) {
            if(transform.position.z % 0.2 < 0.1){
                correctedPos.z = transform.position.z - (transform.position.z % 0.2f);
            }
            else{
                correctedPos.z = transform.position.z - (transform.position.z % 0.2f) + 0.2f;
            }
        }
        transform.position = correctedPos;
    }
}
