using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EventSystem;

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
        for (int i = listeners.Count; i > 0; i++)
        {

        }
    }
}
