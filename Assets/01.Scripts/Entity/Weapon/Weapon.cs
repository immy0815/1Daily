using System;
using System.Collections;
using _01.Scripts.Entity.Player.Scripts;
using _01.Scripts.Entity.Player.Scripts.Interface;
using _01.Scripts.Manager;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IThrowable, IInteractable
{
    [field: Header("Basic Weapon Settings")]
    [SerializeField] private float duration = 0.5f;
    [field: SerializeField] public WeaponData WeaponData { get; private set; }
    [field: SerializeField] public bool IsThrownByEnemy { get; protected set; }
    [field: SerializeField] public bool IsThrownByPlayer { get; protected set; }

    public Coroutine AttackCoroutine { get; protected set; }

    private void Start()
    {
        AttackCoroutine = null;
    }

    public abstract void OnThrow(Vector3 direction, bool isThrownByPlayer);
    public abstract void OnInteract(Transform pivot);
    public virtual void OnInteract() { }
    
    protected IEnumerator MoveToPivot(Transform pivot)
    {
        var time = 0f;
        transform.SetParent(pivot);
        
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            var t = time / duration;

            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, t);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, t);
            yield return null;
        }
        
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
    
    protected IEnumerator ChangeTimeScaleForSeconds(float timeDuration)
    {
        var time = 0f;
        TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Attack, 1);
        while (time < timeDuration)
        {
            time += Time.unscaledDeltaTime;
            var t = time / timeDuration;
            TimeScaleManager.Instance.ChangeTimeScale(PriorityType.Attack,Mathf.Lerp(Time.timeScale, 0.01f, t));
            yield return null;
        }
        AttackCoroutine = null;
    }

    public void ResetAttackCoroutine()
    {
        if (AttackCoroutine == null) return;
        StopCoroutine(AttackCoroutine);
        AttackCoroutine = null;
    }
}