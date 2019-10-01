using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class CharacterMovement : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] Transform target;

        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;
        private void Start()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            
            navMeshAgent.enabled = true;
        }
        void Update()
        {
            if(health.IsDead())
            {
            navMeshAgent.enabled = false;
            }
            
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            GetComponent<NavMeshAgent>().destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;

            Vector3 locaVelocity = transform.InverseTransformDirection(velocity);

            float speed = locaVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        private void OnDrawGizmos()
        {

        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
           SerializableVector3 position = (SerializableVector3)state;
           GetComponent<NavMeshAgent>().enabled = false; 
           transform.position = position.ToVector();
           GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}