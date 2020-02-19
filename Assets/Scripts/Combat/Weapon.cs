using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
        
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/MakeNewWeapon", order = 0)]
    public class Weapon : ScriptableObject
    {

        
        [SerializeField] float weaponRange = 2f;

        [SerializeField] float weaponDamage = 4f;

        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;

        [SerializeField] bool isRightHanded = true;

        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public float WeaponRange { get => weaponRange;}
        public float WeaponDamage { get => weaponDamage;}
        public float TimeBetweenAttacks { get => timeBetweenAttacks;}


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);


            if(equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }

            var overRideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overRideController != null)
            {
                    animator.runtimeAnimatorController = overRideController.runtimeAnimatorController;// if the animator is overridden reset the override to the "parent"
            }
        }


        private void DestroyOldWeapon(Transform right, Transform left)
        {
            Transform oldWeapon = right.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = left.Find(weaponName);
            }
            if (oldWeapon == null) return;


            oldWeapon.name = "destroying";
            Destroy(oldWeapon.gameObject);
        }
        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, weaponDamage);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        
    }
}