using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    public virtual void Enter(Enemy enemy)
    {

    }

    public abstract void Behavior(Enemy enemy);

    public abstract void CheckConditions(Enemy enemy);

    public virtual void Exit(Enemy enemy)
    {

    }

}

public class AttackState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        enemy.shooting = true;
        enemy.StartCoroutine(enemy.Shoot());
    }
    public override void Behavior(Enemy enemy)
    {
        enemy.RotateToPlayer();
    }

    public override void CheckConditions(Enemy enemy)
    {
        if(!enemy.IsSoldierInSight())
        {
            enemy.SwitchState(new IdleState()); return;
        }
    }
}

public class IdleState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        enemy.shooting = false;
    }
    public override void Behavior(Enemy enemy)
    {
        enemy.RotateToPlayer();
    }

    public override void CheckConditions(Enemy enemy)
    {
        if(enemy.IsSoldierInSight())
        {
            enemy.SwitchState(new AttackState()); return;
        }
    }
}

public class FleeState : EnemyState
{
    public override void Behavior(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }

    public override void CheckConditions(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }
}