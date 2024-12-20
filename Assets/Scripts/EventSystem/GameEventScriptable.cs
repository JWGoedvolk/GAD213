using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.EventSystem
{
    /// <summary>
    /// This scriptable object will call all subscribed GameEventListeners and raise them (invoking their events)
    /// </summary>
    [CreateAssetMenu(fileName = "Game Event", menuName = "JW/GameEvents")]
    public class GameEventScriptable : ScriptableObject
    {
        public List<EventListener> listeners = new List<EventListener>();

        public void AddListener(EventListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(EventListener listener)
        {
            listeners.Remove(listener);
        }

        public void Raise()
        {
            Debug.Log($"{name} raised");
            foreach (var item in listeners)
            {
                item.Raise();
            }
        }
    } 
}
