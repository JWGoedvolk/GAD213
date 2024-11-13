using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EventSystem
{
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

        public void Raise()
        {
            OnRaised?.Invoke();
        }
    }
}