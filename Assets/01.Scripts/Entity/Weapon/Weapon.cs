using System;
using System.Collections;
using _01.Scripts.Entity.Player.Scripts;
using _01.Scripts.Entity.Player.Scripts.Interface;
using _01.Scripts.Manager;
using _01.Scripts.Util;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IThrowable, IInteractable
{
    [field: Header("Basic Weapon Settings")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] protected ThrownObject thrownObject;
    [field: SerializeField] public WeaponData WeaponData { get; private set; }
    [field: SerializeField] public bool IsOwnedByPlayer { get; protected set; }
    [field: SerializeField] public bool IsThrownByPlayer { get; protected set; }

    public Coroutine AttackCoroutine { get; protected set; }

    protected virtual void Awake()
    {
        if (!thrownObject) thrownObject = gameObject.GetComponent_Helper<ThrownObject>();
    }

    protected virtual void Reset()
    {
        if (!thrownObject) thrownObject = gameObject.GetComponent_Helper<ThrownObject>();
    }

    protected virtual void Start()
    {
        AttackCoroutine = null;
        
        // TODO: 던지는 데미지를 적용해야 하는데 현재는 무기의 기본 데미지가 적용되어있음.(수정 필요!!!)
        thrownObject.Init(WeaponData.damage); 
        thrownObject.enabled = false;
    }

    public abstract void OnThrow(Vector3 direction, bool isThrownByPlayer);
    public abstract void OnInteract(Transform pivot, bool isOwnedByPlayer);
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