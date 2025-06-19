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
    [field: SerializeField] public bool IsCurrentlyOwned { get; protected set; }
    [field: SerializeField] public bool IsOwnedByPlayer { get; protected set; }
    [field: SerializeField] public bool IsThrownByPlayer { get; protected set; }

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
}