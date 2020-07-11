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
        Debug.Log("Switching to Attack State");
        enemy.shooting = true;
        enemy.StartCoroutine(enemy.Shoot());
    }
    public override void Behavior(Enemy enemy)
    {
        enemy.RotateToPlayer();
    }

    public override void CheckConditions(Enemy enemy)
    {
        if(Vector3.Distance(enemy.transform.position, Dog.Instance.transform.position) <= enemy.fleeDistance && Input.GetMouseButtonDown(0))
        {
            enemy.SwitchState(new FleeState()); return;
        }

        if(!enemy.IsSoldierInSight())
        {
            enemy.SwitchState(new IdleState()); return;
        }
    }

    public override void Exit(Enemy enemy)
    {
        enemy.shooting = false;
    }
}

public class IdleState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        Debug.Log("Switching to Idle State");
    }

    public override void Behavior(Enemy enemy)
    {
        enemy.RotateToPlayer();
    }

    public override void CheckConditions(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, Dog.Instance.transform.position) <= enemy.fleeDistance && Input.GetMouseButtonDown(0))
        {
            enemy.SwitchState(new FleeState()); return;
        }

        if (enemy.IsSoldierInSight())
        {
            enemy.SwitchState(new AttackState()); return;
        }
    }
}

public class FleeState : EnemyState
{

    public override void Enter(Enemy enemy)
    {
         enemy.direction = ((Dog.Instance.transform.position - enemy.transform.position) * -1).normalized;
        Debug.Log("Switching to Flee State");

    }
    public override void Behavior(Enemy enemy)
    {
        enemy.RunAway();
    }

    public override void CheckConditions(Enemy enemy)
    {
        if(Vector3.Distance(enemy.transform.position, Dog.Instance.transform.position) > enemy.fleeDistance)
        {
            enemy.SwitchState(new IdleState());
        }
    }
}