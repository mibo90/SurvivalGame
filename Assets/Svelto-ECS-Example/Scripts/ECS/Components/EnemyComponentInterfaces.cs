using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public interface IEnemySinkComponent : IComponent
    {
        float sinkAnimSpeed { get; }
    }

    public interface IEnemyAttackDataComponent: IComponent
    {
        int   damage         { get; }
        float attackInterval { get; }
        float timer          { get; set; }
    }

    public interface IEnemyMovementComponent: IComponent
    {
        bool navMeshEnabled {  set; }
        Vector3 navMeshDestination { set; }
        float moveSpeed { get; set; }
        bool setCapsuleAsTrigger { set; }
    }

    public interface IEnemyTriggerComponent: IComponent
    {
        EnemyCollisionData entityInRange { get; }
    }

    public struct EnemyCollisionData
    {
        public int otherEntityID;
        public bool collides;

        public EnemyCollisionData(int otherEntityID, bool collides)
        {
            this.otherEntityID = otherEntityID;
            this.collides = collides;
        }
    }

    public struct EnemyMovementInfo
    {
        public float movementSpeed { get; private set; }
        public int entityID { get; private set; }

        public EnemyMovementInfo(float movementSpeed, int entity) : this()
        {
            this.movementSpeed = movementSpeed;
            entityID = entity;
        }
    }

    public interface IEnemyVFXComponent: IComponent
    {
        Vector3             position { set; }
        DispatchOnSet<bool> play     { get; }
    }
}