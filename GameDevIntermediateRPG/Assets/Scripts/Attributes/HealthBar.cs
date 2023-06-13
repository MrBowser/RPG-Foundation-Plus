using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCavas = null;

        void Update()
        {
            if (Mathf.Approximately(healthComponent.GetFraction(),0) || Mathf.Approximately(healthComponent.GetFraction(), 1))
            {
                rootCavas.enabled = false;
                return;
            } 

            rootCavas.enabled = true;

            foreground.localScale = new Vector3(healthComponent.GetFraction(),1,1);
        }
    }
}
