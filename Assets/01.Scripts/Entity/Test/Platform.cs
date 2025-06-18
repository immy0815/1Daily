using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    [SerializeField] private List<CharacterController> ridingObjs;

    Vector3 prevPosition;
    private void Awake()
    {
        ridingObjs = new List<CharacterController>();
        prevPosition = transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            CharacterController characterController = other.gameObject.GetComponent<CharacterController>();
            if(characterController) ridingObjs.Add(characterController);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            CharacterController characterController = other.gameObject.GetComponent<CharacterController>();
            if(characterController) ridingObjs.Remove(characterController);
        }
    }

    public void UpdateRidingObjs()
    {
        Vector3 deltaPos = transform.position - prevPosition;
        foreach (var obj in ridingObjs)
        {
            obj.Move(deltaPos);
        }
        prevPosition = transform.position;
    }
}
