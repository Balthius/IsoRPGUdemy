using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Combat
{

    public class EnemyHealthDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();

        }

        private void Update()
        {
            if(fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "N/A";
                return;
            }
            else
            {
                Health health = fighter.GetTarget().GetComponent<Health>();
                GetComponent<Text>().text = String.Format("{0:0}/1:0}%", health.GetHealthPoints(), health.GetMaxHealthPoints());//0.0 to change to 1 higher level of precision
            }
           
        }
    }
}
