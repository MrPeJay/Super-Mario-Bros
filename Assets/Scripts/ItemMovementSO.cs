using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemMovement",menuName ="Custom/ItemMovement")]
public class ItemMovementSO : ScriptableObject
{
    public int accelerationSpeed = 10;
    public int maxSpeed = 5;
}
