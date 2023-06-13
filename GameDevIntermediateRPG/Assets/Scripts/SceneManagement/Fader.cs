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
        Coroutine currentActiveFade =null;
       

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

        public Coroutine FadeOut(float time)
        {
            return Fade(1,time);
        }
        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }


        public Coroutine Fade(float target,float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }

            currentActiveFade = StartCoroutine(FadeOutRoutine(target,time));
            return currentActiveFade;
        }

        private IEnumerator FadeOutRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,target, Time.deltaTime / time);
                print(canvasGroup.alpha);
                yield return null;
            }
        }


   
    }
}


