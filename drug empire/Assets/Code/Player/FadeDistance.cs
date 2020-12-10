using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDistance : MonoBehaviour
{

    List<Transform> targets = new List<Transform>();
    public float proximity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "House")
            targets.Add(collision.transform);
    }

    void Update()
    {
        foreach (Transform target in targets)
        {
            if (target != null)
            {
                Color newColor = target.GetComponent<SpriteRenderer>().color;
                newColor.a = Mathf.InverseLerp(proximity, 0, transform.position.y - target.position.y);
                target.GetComponent<SpriteRenderer>().color = newColor;
            }
        }
    }
}