using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] private Text cooldownText;
    [SerializeField] private Text skillCooldownText;
    [SerializeField] private Image skillCooldownImage;
    [SerializeField] private string skillName;
    [SerializeField] private float maxCooldownTime;

    private float curCooldownTime;
    private bool isCooldown;

    private void Awake()
    {
        SetCooldownIs(false);
    }

    public void UseSkill()
    {
        if (GameManager.Instance.isDie) { return; }

        if (isCooldown)
        {
            StopCoroutine(OnCooldownText());
            StartCoroutine(OnCooldownText());
            return;
        }

        switch (skillName)
        {
            case "Destroy": DestroySkill(); break;
            case "Run": RunSkill(); break;
            case "Barrier": BarrierSkill(); break;
            case "Heal": HealSkill(); break;
        }
    }

    private IEnumerator OnCooldownText()
    {
        float alpha = 0;
        StartCoroutine(OnOffCooldownTextAnimation());
        cooldownText.text = string.Format("Cooltime : {0}sec", curCooldownTime.ToString("F2"));
        SoundManager.Instance.PlaySound("Wrong");

        // cooldownText alpha = 0 ~ 1.0f
        while (alpha < 1.0f)
        {
            alpha += 0.1f;
            cooldownText.color = new Color(255, 255, 255, alpha);
            
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(OffCooldownText());
    }

    private IEnumerator OnOffCooldownTextAnimation()
    {
        Animator anim = cooldownText.GetComponent<Animator>();
        anim.SetBool("isCooldown", true);

        yield return new WaitForSeconds(0.25f);

        anim.SetBool("isCooldown", false);
    }

    private IEnumerator OffCooldownText()
    {
        float alpha = 1;
        while (alpha >= 0)
        {
            alpha -= 0.1f;
            cooldownText.color = new Color(255, 255, 255, alpha);

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void DestroySkill()
    {
        SoundManager.Instance.PlaySound("Destroy");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        StartCoroutine(OnCooldownTime(maxCooldownTime));
    }

    private void RunSkill()
    {
        SoundManager.Instance.PlaySound("Run");
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.speed += 5;

        StartCoroutine(OnCooldownTime(maxCooldownTime));
        Invoke("OffRunSkill", 3);
    }

    private void OffRunSkill()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.speed -= 5;
    }

    private void BarrierSkill()
    {
        SoundManager.Instance.PlaySound("Barrier");
        StopCoroutine(OnBarrier());
        StartCoroutine(OnBarrier());
        StartCoroutine(OnCooldownTime(maxCooldownTime));
    }

    private IEnumerator OnBarrier()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.isBarrier = true;

        SpriteRenderer spriteRenderer = player.barrierObj.GetComponent<SpriteRenderer>();

        float alpha = 0;
        while (alpha < 0.5f)
        {
            alpha += 0.1f;
            spriteRenderer.color = new Color(255, 255, 0, alpha);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.45f);

        StopCoroutine(OffBarrier());
        StartCoroutine(OffBarrier());
    }

    private IEnumerator OffBarrier()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();

        SpriteRenderer spriteRenderer = player.barrierObj.GetComponent<SpriteRenderer>();

        float alpha = 0.5f;
        while (alpha > 0)
        {
            alpha -= 0.1f;
            spriteRenderer.color = new Color(255, 255, 0, alpha);

            yield return new WaitForSeconds(0.05f);
        }

        player.isBarrier = false;
    }

    private void HealSkill()
    {
        if (GameManager.Instance.life >= GameManager.Instance.maxLife)
        {
            StartCoroutine(ShowMaxLife());
            return;
        }

        SoundManager.Instance.PlaySound("Heal");
        GameManager.Instance.UpdateLifeIcon(++GameManager.Instance.life);
        StartCoroutine(OnCooldownTime(maxCooldownTime));
    }

    private IEnumerator ShowMaxLife()
    {
        float alpha = 0;
        StartCoroutine(OnOffCooldownTextAnimation());
        cooldownText.text = "Life is Full!";
        SoundManager.Instance.PlaySound("Wrong");

        // cooldownText alpha = 0 ~ 1.0f
        while (alpha < 1.0f)
        {
            alpha += 0.1f;
            cooldownText.color = new Color(255, 255, 255, alpha);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(OffCooldownText());
    }

    private IEnumerator OnCooldownTime(float maxCooldownTime)
    {
        curCooldownTime = maxCooldownTime;

        SetCooldownIs(true);

        while (curCooldownTime > 0)
        {
            curCooldownTime -= Time.deltaTime;

            skillCooldownImage.fillAmount = curCooldownTime / maxCooldownTime;

            skillCooldownText.text = curCooldownTime.ToString("F2");

            yield return null;
        }

        SetCooldownIs(false);
    }

    private void SetCooldownIs(bool boolean)
    {
        isCooldown = boolean;
        skillCooldownText.enabled = boolean;
        skillCooldownImage.enabled = boolean;
    }
}