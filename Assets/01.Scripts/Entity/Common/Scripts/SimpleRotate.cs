using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 500f;
    void Update()
    {
        transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime), Space.Self);
    }
}
