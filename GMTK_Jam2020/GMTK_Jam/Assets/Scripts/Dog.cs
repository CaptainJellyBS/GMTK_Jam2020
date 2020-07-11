using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public static Dog Instance { get; private set; }
    public float forwardSpeed;
    bool isMoving;
    public float walkAnimationSpeed, wagAnimationSpeed;
    public Sprite[] bodySprites, pawSprites;
    int curBodySprite, curPawSprite;
    public SpriteRenderer bodySpriteRenderer, pawSpriteRenderer;

    private AudioEmitter audioEmitter;

    Vector3 dest;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        audioEmitter = GetComponent<AudioEmitter>();
    }

    void Start()
    {
        dest = transform.position;
        curBodySprite = 0; curPawSprite = 0;
        StartCoroutine(WalkAnimation());
        StartCoroutine(WagAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(Input.GetMouseButton(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dest = new Vector3(mousePos.x, mousePos.y, 0);
            float AngleRad = Mathf.Atan2(dest.y - transform.position.y, dest.x - transform.position.x);
            // Get Angle in Degrees
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            // Rotate Object
            transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            audioEmitter.PlaySound();
        }

        isMoving = Vector3.Distance(dest, transform.position) > 0.05f;
        
        if(isMoving)
        {
           transform.position += (dest - transform.position).normalized * forwardSpeed * Time.deltaTime;
        }
    }

    void Bark()
    {

    }

    IEnumerator WalkAnimation()
    {
        while (true)
        {
            while (isMoving)
            {
                yield return new WaitForSeconds(walkAnimationSpeed);
                curPawSprite++; curPawSprite %= pawSprites.Length;
                pawSpriteRenderer.sprite = pawSprites[curPawSprite];
            }
            pawSpriteRenderer.sprite = null;
            yield return null;
        }
    }

    IEnumerator WagAnimation()
    {
        while (true)
        {
                yield return new WaitForSeconds(wagAnimationSpeed);
            curBodySprite++; curBodySprite %= bodySprites.Length;

            bodySpriteRenderer.sprite = bodySprites[curBodySprite];
        }
    }
}
