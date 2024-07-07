using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    [Header("PatternManager Info")]
    public int patternIndex;
    public float bulletSpeed;
    public bool isFirst;

    [Header("Other")]
    [SerializeField] private Pattern[] patterns;

    private void Awake()
    {
        Invoke("PatternThink", 5f);
    }

    private void Update()
    {
        
    }

    private void PatternThink()
    {
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
        }

        print(patternIndex);
    }

    private void VerticalPattern()
    {
        for (int i = 0; i < patterns[patternIndex].bulletPositions.Length; i++)
        {
            GameObject bullet = Instantiate(patterns[patternIndex].bulletPrefab, patterns[patternIndex].bulletPositions[i]);
            bullet.transform.position += Vector3.right * ((patterns[patternIndex].curPatternCount - 1) * 1f);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            rigid.AddForce(Vector2.down * bulletSpeed, ForceMode2D.Impulse);
        }

        patterns[patternIndex].curPatternCount++;

        if (patterns[patternIndex].curPatternCount <= patterns[patternIndex].maxPatternCounts)
        {
            Invoke("VerticalPattern", 3);
        }

        else
        {
            Invoke("PatternThink", 1.5f);
        }
    }

    private void HorizontalPattern()
    {
        for (int i = 0; i < patterns[patternIndex].bulletPositions.Length; i++)
        {
            GameObject bullet = Instantiate(patterns[patternIndex].bulletPrefab, patterns[patternIndex].bulletPositions[i]);
            bullet.transform.Rotate(0, 0, 90);
            bullet.transform.position += Vector3.up * ((patterns[patternIndex].curPatternCount - 1) * 1f);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            rigid.AddForce(Vector2.right * bulletSpeed, ForceMode2D.Impulse);
        }

        patterns[patternIndex].curPatternCount++;

        if (patterns[patternIndex].curPatternCount <= patterns[patternIndex].maxPatternCounts)
        {
            Invoke("HorizontalPattern", 3);
        }

        else
        {
            Invoke("PatternThink", 1.5f);
        }
    }
}