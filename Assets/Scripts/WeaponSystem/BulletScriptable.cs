using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Bullet", order = 2)]
public class BulletScriptable : ScriptableObject
{
    public float VelocityModifier = 1f;
    public float Damage = 1f;
    public float LifeTime = 1f;
}
