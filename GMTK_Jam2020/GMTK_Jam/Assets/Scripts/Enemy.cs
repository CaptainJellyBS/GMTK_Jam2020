using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 direction;
    public Vector3 bulletOffset;
    public GameObject bullet;
    public float fireRate;
    public float fleeDistance;
    EnemyState currentState;
    public bool shooting = false;
    public float bulletSpread;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.zero;
        SwitchState(new IdleState());
    }

    // Update is called once per frame
    void Update()
    {
        //RotateToPlayer();
        //Move();

        currentState.Behavior(this);
        currentState.CheckConditions(this);
    }

    /// <summary>
    /// Enemy moves away from the dog whenever the dog barks.
    /// </summary>
    public void RunAway()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void RotateToPlayer()
    {
        Vector3 dest = new Vector3(Soldier.Instance.transform.position.x, Soldier.Instance.transform.position.y, 0);
        float AngleRad = Mathf.Atan2(dest.y - transform.position.y, dest.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
    }

    public IEnumerator Shoot()
    {
        while (shooting)
        {
            GameObject b = Instantiate(bullet);
            b.GetComponent<Bullet>().Init(transform.position + (transform.rotation * bulletOffset), transform.up + transform.right * Random.Range(-bulletSpread, bulletSpread), transform.rotation, gameObject);
            yield return new WaitForSeconds(fireRate);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void SwitchState(EnemyState newState)
    {
        if (currentState != null)
        { currentState.Exit(this); }
        currentState = newState;
        currentState.Enter(this);
    }

    public bool IsSoldierInSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (Soldier.Instance.transform.position - transform.position).normalized);
        return hit.collider.gameObject.CompareTag("Soldier");

    }
}
