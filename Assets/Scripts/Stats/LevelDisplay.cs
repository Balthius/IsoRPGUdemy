using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Stats
{

    public class LevelDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}",baseStats.GetLevel());//0.0 to change to 1 higher level of precision
        }
    }

}