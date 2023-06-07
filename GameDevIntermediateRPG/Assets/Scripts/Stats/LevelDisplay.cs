using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {

        BaseStats baseStats;
        TextMeshProUGUI LVLvalue;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            LVLvalue = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            LVLvalue.text = $"{baseStats.GetLevel()}";
        }
    }
}
