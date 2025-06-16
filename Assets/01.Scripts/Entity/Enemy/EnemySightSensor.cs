using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightSensor : MonoBehaviour
{
    private Enemy enemy;
    private float maxDistance;
    [SerializeField] float viewAngle;
    int detectLayer;
    [SerializeField] private Transform targetInSightRange;
    private Vector3 TargetPos => targetInSightRange.position + Vector3.up * 1;  // 플레이어 발밑이 아닌 적당히 위로 레이를 쏘기 위함
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        detectLayer = LayerMask.NameToLayer("Player");
        maxDistance = GetComponent<SphereCollider>().radius;
        targetInSightRange = null;
    }

    void Update()
    {
        if (!targetInSightRange) return;
        Vector3 posDiffDirection = (TargetPos - transform.position).normalized;
            
        // 1. 시야각 안에 있음
        if (Vector3.Dot(transform.forward, posDiffDirection) < Mathf.Cos(viewAngle * Mathf.Deg2Rad))
        {
            enemy.SetTarget(null);
            return;
        }
            
        // 2. 가리는 물체가 없음
        Ray ray = new Ray(transform.position, posDiffDirection);
        if (Physics.Raycast(ray, out var hit, maxDistance))
        {
            if (hit.transform == targetInSightRange)
            {
                enemy.SetTarget(hit.transform);
            }
            else
            {
                enemy.SetTarget(null);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == detectLayer)
        {
            targetInSightRange = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == detectLayer)
        {
            enemy.SetTarget(null);
            targetInSightRange = null;
        }
    }
    private void OnDrawGizmos()
    {
        if (targetInSightRange != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, TargetPos);
            Gizmos.DrawSphere(TargetPos, 0.2f);
        }
    }
}
