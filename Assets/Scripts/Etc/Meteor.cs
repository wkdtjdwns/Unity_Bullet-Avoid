using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Meteor Info")]
    public bool isRight;
    public float bulletSpeed;
    [SerializeField] private int curPatternCount;
    [SerializeField] private float cooldownTime;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPosition;

    private void Awake()
    {
        cooldownTime = 2f;
    }

    private void Update()
    {
        bulletPosition.transform.position = this.transform.position;
        if (GameManager.Instance.isDie) { StopAllCoroutines(); }
    }

    private void OnEnable()
    {
        curPatternCount = 0;
        bulletPosition = GameObject.Find("Meteor Bullet Spawn Position").transform;
        StartCoroutine(Shot());
    }

    private void OnDisable()
    {
        StopCoroutine(Shot());
    }

    private IEnumerator Shot()
    {
        int roundNumA = 50;
        int roundNumB = 37;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        SoundManager.Instance.PlaySound("Shot");
        for (int i = 0; i < 50; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.isRotate = true;

            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 2 * i / roundNum)
                                         , Mathf.Cos(Mathf.PI * 2 * i / roundNum) * (-1));

            rigid.AddForce(dirVec.normalized * bulletSpeed, ForceMode2D.Impulse);

            Vector3 rot_vec = Vector3.forward * 360 * i / roundNum;
            bullet.transform.Rotate(rot_vec);
        }

        yield return new WaitForSeconds(cooldownTime);

        curPatternCount++;
        StartCoroutine(Shot());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Destroy(this.gameObject);
        }
    }
}