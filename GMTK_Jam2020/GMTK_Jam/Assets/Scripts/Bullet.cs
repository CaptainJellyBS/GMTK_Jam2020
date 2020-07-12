using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dir;
    GameObject origin;
    public float moveSpeed;

    public void Init(Vector3 position, Vector3 direction, Quaternion rotation, GameObject source)
    {
        dir = direction.normalized;
        transform.position = position;
        transform.rotation = rotation;
        origin = source;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Don't collide with the original shooter
        if (collision.gameObject == origin)
        { Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>()); return; }

        switch(collision.gameObject.tag)
        {
            case "Bullet": Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>()); return;
            case "Player": Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>()); return;
            case "Soldier": Debug.Log("Lol soldier ded"); Destroy(gameObject);  break;
            case "Enemy": collision.gameObject.GetComponent<Enemy>().Die(); Destroy(gameObject); break;
            case "Obstacle": Destroy(gameObject); break;
            default: Destroy(gameObject); break;
        }
    }

}
