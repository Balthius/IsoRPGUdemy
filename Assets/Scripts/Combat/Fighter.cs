using System.Collections;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] Transform rightHandTransform = null, leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        LazyValue<Weapon> equippedWeapon;
        private bool canAttack = true;
        Health target;

        private void Awake()
        {
            equippedWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
        }
        private Weapon SetDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Start()
        {
            equippedWeapon.ForceInit();
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
            equippedWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            equippedWeapon.value.Spawn(rightHandTransform, leftHandTransform, animator);
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
            return Vector3.Distance(transform.position, target.transform.position) < equippedWeapon.value.WeaponRange;
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.damage)
            {
                yield return equippedWeapon.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.damage)
            {
                yield return equippedWeapon.value.GetPercentageBonus();
            }
        }

        //Animation Event
        private void Hit()
        {   


            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.damage);
            if(equippedWeapon.value.HasProjectile())
            {
                float hardCodedDmg = 10;//replace  later
                equippedWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, hardCodedDmg);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }

            
        }

        void Shoot()
        {
            Hit();
        }
        private IEnumerator HasAttackedRecently()
        {
            canAttack = false;
            yield return new WaitForSeconds(equippedWeapon.value.TimeBetweenAttacks);
            canAttack = true;
        }

        public object CaptureState()
        {
            return equippedWeapon.value.name;
        }

        public void RestoreState(object state)
        {

            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

    }
}