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
    public Vector3 bulletOffset;

    public SpriteRenderer legsSprite, headSprite, bodySprite;
    public Sprite[] legSprites, headSprites, bodySprites;
    public int curLegSprite, curHeadSprite;
    public float walkAnimationSpeed, headAnimationSpeed;

    [SerializeField] private AudioEmitter shootingSound;
    [SerializeField] private AudioEmitter dyingSound;

    public static Soldier Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        index = 0;
        curAngle = 0.0f;
        rotateDir = 1;
        walkDirection = Vector3.up;
        currentPoint = patrolPoints[index];
        StartCoroutine(Shoot());
        StartCoroutine(LegsAnimation());
        StartCoroutine(HeadAnimation());
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
            index++; index %= patrolPoints.Length;
            currentPoint = patrolPoints[index];
        }

        transform.position += (currentPoint.position - transform.position).normalized * moveSpeed * Time.deltaTime;
    }

    void ShootBullet()
    {
        GameObject b = Instantiate(bullet);
        b.GetComponent<Bullet>().Init(transform.position + (transform.rotation * bulletOffset), transform.up, transform.rotation, gameObject);
        shootingSound.PlaySound();
    }

    void RotateFire()
    {
        curAngle += rotateDir * fireRotateSpeed;
        if(curAngle <= -fireArcDegree || curAngle >= fireArcDegree)
        {
            rotateDir *= -1;
        }

        transform.rotation = Quaternion.AngleAxis(curAngle + Vector3.SignedAngle(Vector3.up, currentPoint.position-transform.position,Vector3.forward), Vector3.forward);
        legsSprite.transform.localRotation = Quaternion.AngleAxis(-curAngle, Vector3.forward);
        headSprite.transform.localRotation = Quaternion.AngleAxis(-curAngle, Vector3.forward);
    }

    IEnumerator Shoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(fireRate);
            ShootBullet();
            
        }
    }

    IEnumerator LegsAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(walkAnimationSpeed);
            curLegSprite++; curLegSprite %= legSprites.Length;

            legsSprite.sprite = legSprites[curLegSprite];
        }
    }

    IEnumerator HeadAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(headAnimationSpeed);
            curHeadSprite++; curHeadSprite %= headSprites.Length;

            headSprite.sprite = headSprites[curHeadSprite];
        }
    }
}
