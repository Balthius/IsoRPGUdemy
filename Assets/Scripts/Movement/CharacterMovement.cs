using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class CharacterMovement : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] Transform target;

        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;


        private void Awake()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            
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
        [System.Serializable]
        struct MovementSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;

        }
        public object CaptureState()
        {
            MovementSaveData data = new MovementSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;

        }

        public void RestoreState(object state)
        {
            MovementSaveData data = (MovementSaveData)state;
            navMeshAgent.enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();// mine WAS missing this, why?
        }

        private void OnDrawGizmos()
        {

        }

    }
}