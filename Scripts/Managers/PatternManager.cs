using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    [Header("PatternManager Info")]
    [SerializeField] private int patternIndex;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float meteorSpeed;

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
                meteorSpeed = 10;
                bulletSpeed = 15;
                patterns[patternIndex].curPatternCount = 0;
                patterns[patternIndex].maxPatternCounts = Random.Range(3, 6);

                MeteorPattern();
                break;

            case "Circle":
                bulletSpeed = 2.5f;
                patterns[patternIndex].curPatternCount = 0;
                patterns[patternIndex].maxPatternCounts = Random.Range(3, 8);

                CirclePattern();
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
        meteor.isRight = ranIndex <= 14;

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
        
        rigid.AddForce(Vector2.right * dir * meteorSpeed, ForceMode2D.Impulse);
        rigid.AddForce(Vector2.down * meteorSpeed, ForceMode2D.Impulse);

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

    private void CirclePattern()
    {
        if (GameManager.Instance.isDie) { return; }

        int roundNumA = 50;
        int roundNumB = 37;
        int roundNum = patterns[patternIndex].curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        SoundManager.Instance.PlaySound("Shot");
        for (int i = 0; i < 50; i++)
        {
            if (i % 2 == 1) { continue; }

            Bullet bullet = Instantiate(patterns[patternIndex].bulletPrefab, patterns[patternIndex].bulletPositions[0].position, Quaternion.identity).GetComponent<Bullet>();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 2 * i / roundNum)
                                         , Mathf.Cos(Mathf.PI * 2 * i / roundNum) * (-1));

            rigid.AddForce(dirVec.normalized * bulletSpeed, ForceMode2D.Impulse);

            Vector3 rot_vec = Vector3.forward * 360 * i / roundNum;
            bullet.transform.Rotate(rot_vec);
        }

        patterns[patternIndex].curPatternCount++;

        if (patterns[patternIndex].curPatternCount <= patterns[patternIndex].maxPatternCounts)
        {
            Invoke("CirclePattern", 2f);
        }

        else
        {
            Invoke("PatternThink", 1.5f);
        }
    }
}