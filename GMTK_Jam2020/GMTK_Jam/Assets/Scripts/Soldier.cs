using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    Vector3 shootDirection;
    Vector3 walkDirection;
    float curAngle;
    int rotateDir;
    public float fireRate;
    public GameObject bullet;
    public float fireArcDegree;
    public float fireRotateSpeed;
    public float moveSpeed; 
    public Transform[] patrolPoints;
    Transform currentPoint;
    int index;

    void Start()
    {
        index = 0;
        curAngle = 0.0f;
        rotateDir = 1;
        walkDirection = Vector3.up;
        currentPoint = patrolPoints[index];
    }

    // Update is called once per frame
    void Update()
    {       
        MoveToPoint();
        RotateFire();
        
    }

    void MoveToPoint()
    {
        if(Vector3.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            index++;
            currentPoint = patrolPoints[index];
        }

        transform.position += (currentPoint.position - transform.position).normalized * moveSpeed * Time.deltaTime;
    }

    void ShootBullet()
    {

    }

    void RotateFire()
    {
        curAngle += rotateDir * fireRotateSpeed;
        if(curAngle <= -fireArcDegree || curAngle >= fireArcDegree)
        {
            rotateDir *= -1;
        }

        Debug.Log(curAngle);

        transform.rotation = Quaternion.AngleAxis(curAngle + Vector3.Angle(Vector3.up, transform.position - currentPoint.position), Vector3.forward);
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(fireRate);
            ShootBullet();
        }
    }
}
