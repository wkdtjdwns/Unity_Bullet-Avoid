using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Info")]
    public float speed;
    public int life;
    public bool isBarrier;
    [SerializeField] private bool isTop;
    [SerializeField] private bool isLeft;
    [SerializeField] private bool isRight;
    [SerializeField] private bool isBottom;

    [Header("Other")]
    public GameObject barrierObj;

    private void Awake()
    {
        speed = 10;
        life = 3;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isRight && h == 1) || (isLeft && h == -1)) { h = 0; }

        float v = Input.GetAxisRaw("Vertical");
        if ((isTop && v == 1) || (isBottom && v == -1)) { v = 0; }

        Vector3 nextPosition = new Vector3(h, v, 0) * speed * Time.deltaTime;
        this.transform.position += nextPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top": isTop = true; break;
                case "Left": isLeft = true; break;
                case "Right": isRight = true; break;
                case "Bottom": isBottom = true; break;
            }
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);

            if (!isBarrier)
            {
                print("Game Over");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top": isTop = false; break;
                case "Left": isLeft = false; break;
                case "Right": isRight = false; break;
                case "Bottom": isBottom = false; break;
            }
        }
    }
}