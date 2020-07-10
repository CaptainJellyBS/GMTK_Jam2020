using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
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

    void Attack()
    {

    }
}
