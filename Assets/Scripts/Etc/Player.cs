using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Info")]
    public float speed;
    public bool isBarrier;
    [SerializeField] private bool isTop;
    [SerializeField] private bool isLeft;
    [SerializeField] private bool isRight;
    [SerializeField] private bool isBottom;

    [Header("Other")]
    public GameObject barrierObj;
    public GameObject diePaticle;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        speed = 10;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (GameManager.Instance.isDie) { return; }

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
            if (GameManager.Instance.isHit || isBarrier)
            {
                Destroy(collision.gameObject);
                return;
            }

            OnHit();
            Destroy(collision.gameObject);
        }
    }

    private void OnHit()
    {
        SoundManager.Instance.PlaySound("Hit");

        StopCoroutine(GameManager.Instance.OnHit());
        StartCoroutine(GameManager.Instance.OnHit());

        StopCoroutine(OnHitAnimation());
        StartCoroutine(OnHitAnimation());
    }

    private IEnumerator OnHitAnimation()
    {
        for (int i = 0; i < 5; i++)
        {
            float alpha = (i % 2 == 0) ? 0.1f : 0.5f;

            spriteRenderer.color = new Color(1, 1, 1, alpha);
        
            yield return new WaitForSeconds(0.25f);
        }

        spriteRenderer.color = new Color(1, 1, 1, 1);

        GameManager.Instance.isHit = false;
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