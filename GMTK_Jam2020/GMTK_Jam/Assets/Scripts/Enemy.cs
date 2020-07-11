using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    Vector3 direction;
    public Vector3 bulletOffset;
    public GameObject bullet;
    public float fireRate;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.zero;
        StartCoroutine(Attack());

    }

    // Update is called once per frame
    void Update()
    {
        RotateToPlayer();
        Move();
    }

    /// <summary>
    /// Enemy moves away from the dog whenever the dog barks.
    /// </summary>
    void Move()
    {
        if(Input.GetMouseButtonDown(0))
        {
            direction = ((Dog.Instance.transform.position - transform.position) * -1).normalized;
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator Attack()
    {
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (Soldier.Instance.transform.position - transform.position).normalized);
            if (hit.collider.gameObject.CompareTag("Soldier"))
            {
                yield return StartCoroutine(Shoot());
            }

            yield return null;
        }
    }

    void RotateToPlayer()
    {
        Vector3 dest = new Vector3(Soldier.Instance.transform.position.x, Soldier.Instance.transform.position.y, 0);
        float AngleRad = Mathf.Atan2(dest.y - transform.position.y, dest.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
    }

    IEnumerator Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (Soldier.Instance.transform.position - transform.position).normalized);

        while (hit.collider.gameObject.CompareTag("Soldier"))
        {
            GameObject b = Instantiate(bullet);
            b.GetComponent<Bullet>().Init(transform.position + (transform.rotation * bulletOffset), transform.up, transform.rotation);
            yield return new WaitForSeconds(fireRate);
            hit = Physics2D.Raycast(transform.position, (Soldier.Instance.transform.position - transform.position).normalized);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
