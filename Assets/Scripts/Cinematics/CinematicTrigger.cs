using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{

    public class CinematicTrigger : MonoBehaviour
    {

        private bool hasPlayed = false;
        // Start is called before the first frame update
       private void OnTriggerEnter(Collider other)
        {
            if(!hasPlayed && other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                hasPlayed = true;
            }
        }
    }
}