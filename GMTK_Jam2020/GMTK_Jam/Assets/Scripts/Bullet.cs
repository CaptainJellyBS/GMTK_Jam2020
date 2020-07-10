using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dir;
    public float moveSpeed;

    public void Init(Vector3 position, Vector3 direction, Quaternion rotation)
    {
        dir = direction.normalized;
        transform.position = position;
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

}
