using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 점수 넣기
    [Header("Game Info")]
    public float score;
    public int life;
    public int maxLife;
    public Image[] lifeImage;
    public bool isHit;
    public bool isDie;
    public bool isOption;
    [SerializeField] private GameObject startScene;
    [SerializeField] private bool isStartScene;

    [Header("Save Data")]
    public string rankName1;
    public float rankScore1;

    public string rankName2;
    public float rankScore2;

    public string rankName3;
    public float rankScore3;

    [Header("Other")]
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private GameObject optionObj;
    [SerializeField] private GameObject rankingObj;

    private void Awake()
    {
        Instance = this;

        score = 0;
        maxLife = 5;
        life = maxLife;

        isStartScene = true;
    }

    private void Update()
    {
        startScene.SetActive(isStartScene);
        if (isStartScene || isOption ) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }

        if (!isDie) { score += Time.deltaTime; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Option();
        }

        CheatKey();
    }

    public void GameStart()
    {
        SoundManager.Instance.PlaySound("Button");
        isStartScene = false;
    }

    public void Option()
    {
        SoundManager.Instance.PlaySound("Button");
        isOption = !isOption;
        optionObj.SetActive(isOption);
        Time.timeScale = isOption ? 0 : 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public IEnumerator OnHit()
    {
        if (life > 1)
        {
            isHit = true;

            SoundManager.Instance.PlaySound("Hit");
            UpdateLifeIcon(--life);

            StopCoroutine(CameraShiver());
            StartCoroutine(CameraShiver());

            background.color = new Color(1, 0, 0);

            yield return new WaitForSeconds(0.1f);

            background.color = new Color(1, 1, 1);
        }

        else
        {
            UpdateLifeIcon(--life);
            OnDie();
        }
    }

    private IEnumerator CameraShiver()
    {
        Camera camera = Camera.main;
        for (int i = 0; i < 10; i++)
        {
            float x = (i % 2 == 0) ? -1 : 1;

            Vector3 dir = new Vector3(x, 0, 0);

            camera.transform.position += dir;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void UpdateLifeIcon(int life)
    {
        for (int i = maxLife - 1; i >= 0; i--)
        {
            lifeImage[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = life - 1; i >= 0; i--)
        {
            lifeImage[i].color = new Color(1, 1, 1, 1);
        }

        if (life >= 4) { SetBgm(1); }
        else if (life >= 2) { SetBgm(2); }
        else if (life >= 1) { SetBgm(3); }
        else { SetBgm(4); }
    }


    private void SetBgm(int type)
    {
        switch (type)
        {
            case 1: SoundManager.Instance.PlayBgm("Bgm 1"); break;
            case 2: SoundManager.Instance.PlayBgm("Bgm 2"); break;
            case 3: SoundManager.Instance.PlayBgm("Bgm 3"); break;
            default: SoundManager.Instance.PlayBgm("Null"); break;
        }
    }

    // Death Scene
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
        while (size > 1.25f)
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

        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0) { spriteRenderer.color = new Color(1, 0, 0); SoundManager.Instance.PlaySound("Dying"); }
            else { spriteRenderer.color = new Color(1, 1, 1); }

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.5f);

        Die();
    }

    private void Die()
    {
        Player player = GameObject.Find("Player").gameObject.GetComponent<Player>();
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        SoundManager.Instance.PlaySound("Die");
        spriteRenderer.color = new Color(0, 0, 0, 0);
        player.diePaticle.SetActive(true);

        Invoke("ShowRanking", 2f);
    }

    private void ShowRanking()
    {
        SoundManager.Instance.PlaySound("Ranking");
        rankingObj.SetActive(true);
    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            StartCoroutine(OnHit());
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            DataManager.Instance.Reset();
        }
    }
}