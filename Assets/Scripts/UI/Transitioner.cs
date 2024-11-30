using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.Scene.Transition
{
    public class Transitioner : MonoBehaviour
    {
        [SerializeField] private CanvasGroup transitionUI;
        [SerializeField] private float transitionTime;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowTransition()
        {
            transitionUI.alpha = Mathf.MoveTowards(transitionUI.alpha, 1f, transitionTime*Time.deltaTime);
        }

        public void HideTransition()
        {
            transitionUI.alpha = Mathf.MoveTowards(transitionUI.alpha, 0f, transitionTime * Time.deltaTime);
        }
    } 
}
