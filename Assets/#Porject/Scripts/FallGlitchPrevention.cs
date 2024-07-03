using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGlitchPrevention : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Fall Outside Plane Teleport back
        if(transform.position.y < -5f){
            Vector3 resetPosition;
            resetPosition.x= 0;
            resetPosition.y= 0;
            resetPosition.z=-3;
            transform.position = resetPosition;
        }
    }
}
