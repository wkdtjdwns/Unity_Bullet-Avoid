using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 점수 넣기
    [Header("Game Info")]
    public int score;
    public int life;
    public bool isDie;

    private void Awake()
    {
        Instance = this;

        score = 0;
        life = 5;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) OnDie();
    }

    // 데스씬
    private void OnDie()
    {
        isDie = true;

        // Stop All Bullet
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        StartCoroutine(CameraCloseUp());
    }

    private IEnumerator CameraCloseUp()
    {
        float size = Camera.main.orthographicSize;
        while (size > 1)
        {
            size -= 0.1f;
            Camera.main.orthographicSize = size;

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(SetPlayerColor());
    }

    private IEnumerator SetPlayerColor()
    {
        GameObject player = GameObject.Find("Player").gameObject;
        SpriteRenderer spriteRenderer =  player.GetComponent<SpriteRenderer>();

        float alpha = 1f;
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0) { alpha = 0.5f; }
            else { alpha = 1f; }

            spriteRenderer.color = new Color(255, 0, 0, alpha);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.5f);

        Die();
    }

    private void Die()
    {
        Player player = GameObject.Find("Player").gameObject.GetComponent<Player>();
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        spriteRenderer.color = new Color(0, 0, 0, 0);
        player.diePaticle.SetActive(true);
    }
}