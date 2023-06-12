using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;


        void Update()
        {

        }

        public void Spawn(float damageAmount)
        {
           
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
            instance.SetValue(damageAmount);
        }
    }
}
