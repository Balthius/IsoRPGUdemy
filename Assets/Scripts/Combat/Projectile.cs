
using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float speed = 3f;
        Health target = null;
        float damage = 0;
        [SerializeField]private bool isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = .5f;
        GameObject instigator;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        // Update is called once per frame
        void Update()
        {
            if(target == null)return;
            
            if(isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target,GameObject instigator,  float damage)
        {
            this.target = target;
            this.instigator = instigator;
            this.damage = damage;
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if(targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height /2;

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Health>() != target)return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            speed = 0;
            if(hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
            }
            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
