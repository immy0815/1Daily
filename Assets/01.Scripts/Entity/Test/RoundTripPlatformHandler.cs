using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTripPlatformHandler : MonoBehaviour
{
    [SerializeField] private Platform platform;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;

    [Range(0f,1f)][SerializeField] private float offset;

    [SerializeField] float velocity;
    private Vector3 posDiff;    // p1과 p2 사이

    private void OnValidate()
    {
        posDiff = p2.position - p1.position;
        platform.transform.position = p2.position - Mathf.Abs(0.5f-offset) * 2 * posDiff;
    }

    void Update()
    {
        offset += velocity * Time.deltaTime;
        if (offset > 1) offset -= 1;
        platform.transform.position = p2.position - Mathf.Abs(0.5f-offset) * 2 * posDiff;

        platform.UpdateRidingObjs();
    }
}
