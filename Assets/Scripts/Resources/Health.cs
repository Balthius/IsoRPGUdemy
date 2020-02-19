
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    [RequireComponent(typeof(BaseStats))]

    public class Health : MonoBehaviour, ISaveable
    {

        private bool isDead = false;

        private void Start()
        {
            if(healthPoints > 0)
            {

                healthPoints = GetComponent<BaseStats>().GetHealth();
            }
        }

        public bool IsDead()
        {
            return isDead;
        }
        [SerializeField] float healthPoints = 100f;

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if(healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
         Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints / GetComponent< BaseStats>().GetHealth());
        }

        private void Die()
        {
            if(!isDead)
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }

        public object CaptureState()
        {
           return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            
            if(healthPoints == 0)
            {
                Die();
            }
        }
    }
}
