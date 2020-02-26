
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;

namespace RPG.Resources
{
    [RequireComponent(typeof(BaseStats))]

    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> healthPoints;
        private bool isDead = false;
        [SerializeField] float regenerationPercentage = 70f;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        private void Start()
        {
            healthPoints.ForceInit();
        }
        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += FlatRegenerateHealth;
        }
        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= FlatRegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {

            //print(gameObject.name + " took damage: " + damage);
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if(healthPoints.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
         Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if (isDead) return;
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
            
            if(healthPoints.value <= 0)
            {
                Die();
            }
        }

        private void FlatRegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }
    }
}
