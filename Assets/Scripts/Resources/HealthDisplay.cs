using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Resources
{

    public class HealthDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}",health.GetHealthPoints(), health.GetMaxHealthPoints());//0:0.0 to change to 1 higher level of precision
        }
    }

}