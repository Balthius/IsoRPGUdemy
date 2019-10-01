using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;

        [SerializeField] float weaponDamage = 4f;

        [SerializeField] float timeBetweenAttacks = 1f;

        private bool canAttack = true;
        Health target;

        private void Update()
        {
            if(target == null)
            {
                return;
            }

            if(target.IsDead())
            {
                return;
            }

            if (!GetIsInRange())
            {
                GetComponent<CharacterMovement>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<CharacterMovement>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if(canAttack)
            {//Triggers Hit event
                StartCoroutine("HasAttackedRecently");
                TriggerAttack();
            }
        }

        

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null)
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();

            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<CharacterMovement>().Cancel();
        }
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        //Animation Event
        private void Hit()
        {   if (target != null)
            {
                target.TakeDamage(weaponDamage);
            }
        }
        private IEnumerator HasAttackedRecently()
        {
            canAttack = false;
            yield return new WaitForSeconds(timeBetweenAttacks);
            canAttack = true;
        }
    }
}