using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI damageText = null;

        public void SetValue(float amount)
        {
            damageText.text = $"{amount}";
        }
    }
}
