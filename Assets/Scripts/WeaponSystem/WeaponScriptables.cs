using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class WeaponScriptables : ScriptableObject
{
    public float FireRate = 1f;
    [Tooltip("This is a modifier for how much more or less damage is dealt. 1 = 100%, 1.5 = 150%, etc.")]
    public float DamageModifier = 1f;
    [Tooltip("This is a the launch velocity of this weapon. This can be further increased/decreased by the bullet's modifier")]
    public float Velocity = 1f;
}
