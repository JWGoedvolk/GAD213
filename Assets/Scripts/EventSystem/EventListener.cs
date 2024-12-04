using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using SAE.EventSystem;

namespace SAE.EventSystem
{
    /// <summary>
    /// This script will wait for its GameEvent (the scriptable object) to raise it, at which point it will invoke all its events
    /// </summary>
    public class EventListener : MonoBehaviour
    {
        public GameEventScriptable GameEvent;
        public UnityEvent OnRaised;
        // Use this for initialization
        void Awake()
        {
            GameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            GameEvent.RemoveListener(this);
        }

        /// <summary>
        /// Will invoke all UnityEvents set in it.
        /// </summary>
        public void Raise()
        {
            Debug.Log($"{gameObject.name} event listener raised");
            OnRaised?.Invoke();
        }
    }
}