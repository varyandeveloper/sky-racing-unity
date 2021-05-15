using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private float length = 0.4f;

    private bool isHitting;

    public bool IsHitting { get { return isHitting; } }

    void FixedUpdate()
    {
        isHitting = false;
        
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(0, 0.5f, transform.position.z), transform.TransformDirection(Vector3.forward), out hit, 2)) {
            isHitting = true;
            Debug.DrawRay(transform.position, hit.point, Color.red);
        }
    }
}
