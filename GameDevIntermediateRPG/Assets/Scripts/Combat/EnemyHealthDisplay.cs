using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Attributes;
using static UnityEngine.UIElements.UxmlAttributeDescription;


namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {

        Fighter fighter;
        TextMeshProUGUI healthValue;

        // Start is called before the first frame update
        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            healthValue = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            Health health = fighter.GetTarget();

            if(fighter.GetTarget() != null )
            {
                healthValue.text = $"{Mathf.Round(health.GetPercentage())}%";
            }
            else
            {
                healthValue.text = "N/A";
            }            

            //below is alternate way to get the above
            //healthValue.text = string.Format("{0:0}%", health.GetPercentage());
        }
    }
}