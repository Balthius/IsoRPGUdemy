using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{    
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;

        [SerializeField] float respawnTime = 5;
        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }


        private void ShowPickup(bool shouldShow)
        {
            this.GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {   
                callingController.GetComponentInParent<CharacterMovement>().StartMoveAction(transform.position, 1f);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }
    }

}