using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "floatVar", menuName = "Scriptable/Float")]
public class FloatVarScriptable : ScriptableObject
{
    [SerializeField] private float val;
    public float Value
    {
        get { return val; }
        set { val = value; }
    }
}
