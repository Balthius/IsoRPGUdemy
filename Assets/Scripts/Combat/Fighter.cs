using System.Collections;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform rightHandTransform = null, leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        Weapon equippedWeapon = null;
        private bool canAttack = true;
        Health target;

        private void Start()
        {
            if(equippedWeapon == null)
            {
            EquipWeapon(defaultWeapon);
            }
        }

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

        public void EquipWeapon(Weapon weapon)
        {
            equippedWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            equippedWeapon.Spawn(rightHandTransform, leftHandTransform , animator);
        }

        public Health GetTarget()
        {
            return target;
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
            return Vector3.Distance(transform.position, target.transform.position) < equippedWeapon.WeaponRange;
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
        {   


            if (target == null) return;
            if(equippedWeapon.HasProjectile())
            {
                equippedWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            }
            else
            {
                target.TakeDamage(gameObject, equippedWeapon.WeaponDamage);
            }

            
        }

        void Shoot()
        {
            Hit();
        }
        private IEnumerator HasAttackedRecently()
        {
            canAttack = false;
            yield return new WaitForSeconds(equippedWeapon.TimeBetweenAttacks);
            canAttack = true;
        }

        public object CaptureState()
        {
            return equippedWeapon.name;
        }

        public void RestoreState(object state)
        {

            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}