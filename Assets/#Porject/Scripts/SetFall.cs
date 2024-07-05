using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFall : MonoBehaviour
{
    public void EnnableFall(int time) {
        Data.FallSpeed = time;
    }

    public void DisableFall() {
        Data.FallSpeed = float.MaxValue;
    }
}
