using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    [Header("Pattern Info")]
    public string patternName;
    public Transform[] bulletPositions;
    public int curPatternCount;
    public int maxPatternCounts;


    [Header("Other")]
    public GameObject bulletPrefab;
}