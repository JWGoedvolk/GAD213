using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAE.Scene
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Dissolver : MonoBehaviour
    {
        [SerializeField] private float dissolveTime = 1f;
        [SerializeField] private bool isDissolved = true;
        [SerializeField] Animation anim;
        [SerializeField] private bool destoryOnDissolve, disableOnDisolve = false;
        [SerializeField] private bool shouldDestroy = false;

        private void Awake()
        {
            dissolveTime = anim.GetClip("Dissolve").length;
        }
        
        public void Appear()
        {
            if (!anim.isPlaying)
            {
                if (isDissolved)
                {
                    anim.clip = anim.GetClip("Appear");
                    anim.Play();
                    isDissolved = false;
                }
            }
        }

        public void Hide()
        {
            anim.clip = anim.GetClip("Dissolve");
            anim.Play();
            isDissolved = true;
            StartCoroutine(DestoryOnDissolve());
        }

        public IEnumerator DestoryOnDissolve()
        {
            yield return new WaitForSeconds(dissolveTime);

            if (destoryOnDissolve)
            {
                Destroy(gameObject);
            }
            else if (disableOnDisolve)
            {
                gameObject.SetActive(false);
            }
        }
    } 
}
