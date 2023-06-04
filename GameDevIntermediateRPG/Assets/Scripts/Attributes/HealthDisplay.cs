using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {

        Health health;
        TextMeshProUGUI healthValue;

        // Start is called before the first frame update
        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthValue = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            //print(healthValue.text);

            healthValue.text = $"{Mathf.Round(health.GetPercentage())}%";

            //below is alternate way to get the above
            //healthValue.text = string.Format("{0:0}%", health.GetPercentage());
        }
    }
}
