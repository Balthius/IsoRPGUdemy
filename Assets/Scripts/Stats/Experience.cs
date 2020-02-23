using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        //public delegate void ExperienceGainedDelegate(); Action below functions as this line as well as the next line, so long as you don't need a return value
        public event Action OnExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            OnExperienceGained();
        }

        public object CaptureState()
        {
            Debug.Log("Capture state points " + experiencePoints);
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            
            experiencePoints = (float)state;
            Debug.Log("Restore state points " + experiencePoints);
        }

        public float GetPoints()
        {
            return experiencePoints;
        }
    }
}
