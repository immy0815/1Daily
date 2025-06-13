using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon, IHittable
{
    public void OnHit()
    {
        Debug.Log("Punch hit");
        
    }
}