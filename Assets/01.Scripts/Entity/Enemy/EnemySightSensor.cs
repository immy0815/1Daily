using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightSensor : MonoBehaviour
{
    private Enemy enemy;
    private float maxDistance;
    [SerializeField] float viewAngle;
    [SerializeField] private Transform targetInSightRange;
    [SerializeField] private LayerMask targetLayer;     // OnTrigger용
    [SerializeField] private LayerMask sightLayer;      // 시야 전용
    private Vector3 TargetPos => targetInSightRange.position + Vector3.up * 1f;  // 플레이어 발밑이 아닌 적당히 위로 레이를 쏘기 위함

    [SerializeField] private bool detectOverWall;
    public bool DetectOverWall { get; private set; }

    private bool canAimTarget;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        maxDistance = GetComponent<SphereCollider>().radius;
        targetInSightRange = null;
        canAimTarget = false;
    }

    void Update()
    {
        if (!targetInSightRange) return;
        Vector3 posDiffDirection = (TargetPos - transform.position).normalized;
            
        // 1. 시야각 안에 없으면
        if (Vector3.Dot(transform.forward, posDiffDirection) < Mathf.Cos(viewAngle / 2 * Mathf.Deg2Rad))
        {
            // Debug.Log(Mathf.Cos(viewAngle / 2 * Mathf.Deg2Rad));
            enemy.SetTarget(null);
            return;
        }

        if (detectOverWall)
        {
            enemy.SetTarget(targetInSightRange);
        }
        
        // 2. 가리는 물체가 있으면
        Ray ray = new Ray(transform.position, posDiffDirection);
        if (Physics.Raycast(ray, out var hit, maxDistance, sightLayer))
        {
            if (hit.transform == targetInSightRange)
            {
                enemy.SetTarget(hit.transform);
                canAimTarget = true;
            }
            else
            {
                if(!detectOverWall) enemy.SetTarget(null);
                canAimTarget = false;
            }
        }
        else
        {
            // Debug.Log("Missing");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & targetLayer) != 0)
        {
            targetInSightRange = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & targetLayer) != 0)
        {
            enemy.SetTarget(null);
            targetInSightRange = null;
        }
    }
    private void OnDrawGizmos()
    {
        if (targetInSightRange != null)
        {
            Vector3 posDiffDirection = (TargetPos - transform.position).normalized;

            // 레이와 같은 방향으로 기즈모 선을 그림
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, posDiffDirection * maxDistance);
        }
    }

    public bool CanAimTarget()
    {
        return canAimTarget;
    }
}
