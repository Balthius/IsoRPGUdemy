using RPG.Stats;

using UnityEngine;


[CreateAssetMenu(fileName = "Progression", menuName = "Stats/NewProgression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField] ProgressionCharacterClass[] characterClasses = null;

    public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach(ProgressionCharacterClass progClass in characterClasses)
            {
                if(progClass.characterClass == characterClass)
                {
                    return progClass.health[level - 1];
                }
            }
        return 0;
        }






    [System.Serializable]// Allows Unity to display this in the inspector
    class ProgressionCharacterClass
    {//repurpose for special class level gains? Not useful for stats
        public CharacterClass characterClass;
        public float[] health;
    }

    
}
