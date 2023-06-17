using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable 
    {

        [SerializeField] float regenerationPercentage = 100;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        //this is to add events to the unity editor
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        { }

        LazyValue<float> healthPoints;
        bool isDead = false;

        private void Awake()
        {
            //note this is showing how to do a lazy initialization
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            //note this is showing how to do a lazy initialization
            healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage,0);

            print(gameObject.name + "took damage: " + damage);

            if(healthPoints.value ==0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerationPercentage /100;
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        private void AwardExperience(GameObject instigator)
        {
             Experience experience = instigator.GetComponent<Experience>();
            if(experience ==null) { return; }

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage() { return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health)); }

        public float GetFraction() { return (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health)); }

        private void Die()
        {
            if(isDead) { return; }

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }


        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value > 0)
            {
                isDead = false;
                GetComponent<Animator>().ResetTrigger("die");
                //note the below is to prevent any entities that are in the death state that are not saved as dead to get back into
                //the general animation loop, very janky
                GetComponent<Animator>().Play("Locomotion");

            }
            else
            {
                Die();
            }
        }




    }
}


//legacy start code
//if(healthPoints <0)
//{
//    healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
//}
