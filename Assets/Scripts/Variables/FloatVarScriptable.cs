using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.Variavles
{
    /// <summary>
    /// This is a scriptable object that holds a float variable
    /// </summary>
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
}
