using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public static Dog Instance { get; private set; }
    public float forwardSpeed, backSpeed, strafeSpeed;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();
        Move();
    }

    void Move()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * forwardSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * strafeSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.up * backSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * strafeSpeed * Time.deltaTime;
        }

    }

    void LookAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        transform.rotation = Quaternion.Euler(0, 0, AngleDeg-90);
    }

    void Bark()
    {

    }
}
