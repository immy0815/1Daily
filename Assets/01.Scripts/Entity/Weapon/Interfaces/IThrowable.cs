using UnityEngine;

public interface IThrowable
{
    void OnThrow(Vector3 direction, bool isThrownByPlayer);
}