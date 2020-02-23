using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Progression", menuName = "Stats/NewProgression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField] ProgressionCharacterClass[] characterClasses = null;

    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;
    public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
        BuildLookup();// Populate the lookupTable

        float[] levels = lookupTable[characterClass][stat];// check 

        if(levels.Length < level)
        {
            return 0;
        }
        return levels[level-1];
      
        }
    public int GetLevels(Stat stat, CharacterClass characterClass)
    {
        Debug.Log("stat: " + stat + " class: " + characterClass);
        Debug.Log(lookupTable[characterClass][stat]);
        BuildLookup();
        float[] levels = lookupTable[characterClass][stat];
        return levels.Length;
    }
    private void BuildLookup()
    {
        if (lookupTable != null) return;
        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();


        foreach (ProgressionCharacterClass progressionClass in characterClasses)
        {
            var statLookupTable = new Dictionary<Stat, float[]>();//Set each stat you want to perform a lookup on

            foreach (ProgressionStat progressionStat in progressionClass.stats)
            {
                statLookupTable[progressionStat.stat] = progressionStat.levels; //grab the value that cooresponds to the stat and it's matching level.
            }


                lookupTable[progressionClass.characterClass] = statLookupTable;
        }
    }

    [System.Serializable]// Allows Unity to display this in the inspector
    class ProgressionCharacterClass
    {//repurpose for special class level gains? Not useful for stats
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
    }
    [System.Serializable]
    class ProgressionStat
    {//This structure allows for saving "per attribute" gains in one place
        public Stat stat;
        public float[] levels;
    }
    
}
