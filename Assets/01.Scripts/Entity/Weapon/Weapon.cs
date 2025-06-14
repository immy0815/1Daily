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
    [field: SerializeField] public bool IsThrown { get; protected set; }

    public abstract void OnThrow(Vector3 direction);
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
}