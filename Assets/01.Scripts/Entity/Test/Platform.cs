using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    private List<GameObject> ridingObjs;

    Vector3 prevPosition;
    private void Awake()
    {
        ridingObjs = new List<GameObject>();
        prevPosition = transform.position;
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            ridingObjs.Add(other.gameObject);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            ridingObjs.Remove(other.gameObject);
        }
    }

    public void UpdateRidingObjs()
    {
        Vector3 deltaPos = transform.position - prevPosition;
        foreach (var obj in ridingObjs)
        {
            obj.transform.position += deltaPos;
        }
    }
}
