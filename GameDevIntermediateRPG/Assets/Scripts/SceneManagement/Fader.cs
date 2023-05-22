using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        float fadeTime = 1f;
       

        private void Awake()
        {
            canvasGroup= GetComponent<CanvasGroup>();

            
        }

        public void fadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        IEnumerator FadeOutIn()
        {
            yield return FadeOut(fadeTime);
            print("FadeOut");
            yield return(FadeIn(fadeTime));
            print("Faded in");
        }

        public IEnumerator FadeOut(float time)
        {
            
             

            while(canvasGroup.alpha < 1)
            {
                
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
            
        }


        public IEnumerator FadeIn(float time)
        {

            while (canvasGroup.alpha > 0)
            {

                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }

        }
    }
}


