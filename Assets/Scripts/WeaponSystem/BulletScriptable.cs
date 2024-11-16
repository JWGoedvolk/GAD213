using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Bullet", order = 2)]
public class BulletScriptable : ScriptableObject
{
    [Tooltip("What the player will see when they try to switch to it")]
    public string Name = "Weapon Name";
    [Tooltip("This is how much faster or slower the bullet is launched")]
    public float VelocityModifier = 1f;
    [Tooltip("How much damage the bullet does")]
    public float Damage = 1f;
    [Tooltip("How long the bullet lasts for before dies")]
    public float LifeTime = 1f;
}
