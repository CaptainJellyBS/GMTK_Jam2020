using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public static Dog Instance { get; private set; }
    public float forwardSpeed, backSpeed, strafeSpeed;

    Vector3 dest;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dest = transform.position;
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

        if (Vector3.Distance(dest, transform.position) > 0.05f)
        {
           transform.position += (dest - transform.position).normalized * forwardSpeed * Time.deltaTime;
        }
    }

    void Bark()
    {

    }
}
