using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] sprites;
    public float animationSpeed;
    int index;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while(enabled)
        {
            yield return new WaitForSeconds(animationSpeed);
            index++; index %= sprites.Length;
            GetComponent<SpriteRenderer>().sprite = sprites[index];
        }
    }
}
