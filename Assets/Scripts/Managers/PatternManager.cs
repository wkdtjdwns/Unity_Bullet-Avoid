using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    [Header("PatternManager Info")]
    public int patternIndex;
    public float bulletSpeed;
    public float meteorSpeed;
    public bool isFirst;

    [Header("Other")]
    [SerializeField] private Pattern[] patterns;

    private void Awake()
    {
        Invoke("PatternThink", 5f);
    }

    private void Update()
    {
        if (GameManager.Instance.isDie) { StopAllCoroutines(); }
    }

    private void PatternThink()
    {
        if (GameManager.Instance.isDie) { return; }

        patternIndex = Random.Range(0, patterns.Length);

        switch (patterns[patternIndex].patternName)
        {
            case "Vertical":
                bulletSpeed = 15;
                patterns[patternIndex].curPatternCount = 0;
                patterns[patternIndex].maxPatternCounts = 3;

                VerticalPattern();
                break;

            case "Horizontal":
                bulletSpeed = 15;
                patterns[patternIndex].curPatternCount = 0;
                patterns[patternIndex].maxPatternCounts = 3;

                HorizontalPattern();
                break;

            case "Meteor":
                meteorSpeed = 5f;
                bulletSpeed = 15;
                patterns[patternIndex].curPatternCount = 0;
                patterns[patternIndex].maxPatternCounts = Random.Range(3, 6);

                MeteorPattern();
                break;
        }

        print(patternIndex);
    }

    private void VerticalPattern()
    {
        if (GameManager.Instance.isDie) { return; }

        SoundManager.Instance.PlaySound("Shot");
        for (int i = 0; i < patterns[patternIndex].bulletPositions.Length; i++)
        {
            if (i % 2 == 0)
            {
                GameObject bullet = Instantiate(patterns[patternIndex].bulletPrefab, patterns[patternIndex].bulletPositions[i]);
                bullet.transform.position += Vector3.right * ((patterns[patternIndex].curPatternCount - 1) * 1f);
                bullet.transform.Rotate(0, 0, 270);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

                rigid.AddForce(Vector2.down * bulletSpeed, ForceMode2D.Impulse);
            }
        }

        patterns[patternIndex].curPatternCount++;

        if (patterns[patternIndex].curPatternCount <= patterns[patternIndex].maxPatternCounts)
        {
            Invoke("VerticalPattern", 3f);
        }

        else
        {
            Invoke("PatternThink", 1.5f);
        }
    }

    private void HorizontalPattern()
    {
        if (GameManager.Instance.isDie) { return; }

        SoundManager.Instance.PlaySound("Shot");
        for (int i = 0; i < patterns[patternIndex].bulletPositions.Length; i++)
        {
            if (i % 2 == 0)
            {
                GameObject bullet = Instantiate(patterns[patternIndex].bulletPrefab, patterns[patternIndex].bulletPositions[i]);
                bullet.transform.position += Vector3.up * ((patterns[patternIndex].curPatternCount - 1) * 1f);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

                rigid.AddForce(Vector2.right * bulletSpeed, ForceMode2D.Impulse);
            }
        }

        patterns[patternIndex].curPatternCount++;

        if (patterns[patternIndex].curPatternCount <= patterns[patternIndex].maxPatternCounts)
        {
            Invoke("HorizontalPattern", 3f);
        }

        else
        {
            Invoke("PatternThink", 1.5f);
        }
    }

    private void MeteorPattern()
    {
        if (GameManager.Instance.isDie) { return; }

        SoundManager.Instance.PlaySound("Meteor");
        int ranIndex = Random.Range(0, patterns[patternIndex].bulletPositions.Length);
        Meteor meteor = Instantiate(patterns[patternIndex].bulletPrefab, patterns[patternIndex].bulletPositions[ranIndex].position, Quaternion.identity).GetComponent<Meteor>();
        Rigidbody2D rigid = meteor.gameObject.GetComponent<Rigidbody2D>();
        meteor.isRight = ranIndex <= 15;

        float dir;
        if (meteor.isRight)
        {
            meteor.transform.Rotate(0, 0, 135);
            dir = 1f;
        }

        else
        {
            meteor.transform.Rotate(0, 0, 225);
            dir = -1f;
        }
        
        rigid.AddForce(Vector2.right * dir * bulletSpeed, ForceMode2D.Impulse);
        rigid.AddForce(Vector2.down * bulletSpeed, ForceMode2D.Impulse);

        patterns[patternIndex].curPatternCount++;

        if (patterns[patternIndex].curPatternCount <= patterns[patternIndex].maxPatternCounts)
        {
            Invoke("MeteorPattern", 5f);
        }

        else
        {
            Invoke("PatternThink", 1.5f);
        }
    }
}