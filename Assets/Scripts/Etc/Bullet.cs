using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Info")]
    public bool isRotate;

    private void Update()
    {
        if (isRotate) { transform.Rotate(Vector3.forward * 50); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Destroy(this.gameObject);
        }
    }
}