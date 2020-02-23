using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Stats
{

    public class ExperienceDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}",experience.GetPoints());//0.0 to change to 1 higher level of precision
        }
    }

}