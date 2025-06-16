using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Entity.Player.Scripts;
using UnityEngine;

public interface IShootable
{
    bool OnShoot(Player player);
    bool OnShoot(Enemy enemy);
}
