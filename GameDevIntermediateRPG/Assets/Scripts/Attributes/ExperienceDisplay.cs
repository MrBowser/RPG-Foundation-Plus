using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {

        Experience experience;
        TextMeshProUGUI xpValue;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            xpValue = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            xpValue.text = $"{experience.GetPoints()}";
        }
    }
}
