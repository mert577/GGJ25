using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;


    [SerializeField]
    float invincibilityTime;

    [SerializeField]
    bool isInvincible = false;

    public Rigidbody2D rb;

    public float knockbackForce;


    //event for when gets damaged

    public UnityEvent<int> OnDamageTaken = new UnityEvent<int>();


    public UnityEvent OnHeal = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();

    public bool isStunned = false;

    public bool isEnemy = false;

    public bool isDead = false;

    public int minResourceDrop, maxResourceDrop;
    public GameObject resourceDrop;

    public float resourceDropForce;


    public float freezeFrameTime;


    public GameObject deathParticles;

    public bool outsideOfWavePool = false;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Damager>(out Damager damager))
        {

            TakeDamage(damager.realDamage, damager.transform.position, damager.isKnockback);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Damager>(out Damager damager))
        {

            if (damager.damagerType != Damager.DamagerType.EnterOnly)
            {

                TakeDamage(damager.realDamage, damager.transform.position, damager.isKnockback);
            }
        }
    }


    IEnumerator HitFreezeFrame()
    {
        Time.timeScale = .35f;
        yield return new WaitForSecondsRealtime(freezeFrameTime);
        Time.timeScale = 1;
    }
    IEnumerator TurnInvincible(float time)
    {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }

    IEnumerator TurnStunned()
    {
        isStunned = true;
        yield return new WaitForSeconds(.125f);
        isStunned = false;
    }

    public void TurnInvincibleForTime(float time)
    {
        StartCoroutine(TurnInvincible(time));

    }

    public void TakeDamage(int damage, Vector2 damagerPos, bool knockback = true)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth -= damage;
        OnDamageTaken.Invoke(currentHealth);

        if (knockback)
        {
            Vector2 knockbackDirection = (rb.position - damagerPos).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(TurnStunned());
        }

        if (!isEnemy)
        {
            StartCoroutine(HitFreezeFrame());
        }

        StartCoroutine(TurnInvincible(invincibilityTime));
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(!isEnemy){
            UIManager.instance.UpdateHealthText(currentHealth);
        }

    }


    public void Die()
    {

        if (isDead)
        {
            return;
        }

        isDead = true;

        if (isEnemy)
        {
            if (!outsideOfWavePool)
            {

                WaveSpawner.instance.aliveEnemies--;
            }
            int resourceDropAmount = Random.Range(minResourceDrop, maxResourceDrop);
            resourceDropAmount = Mathf.RoundToInt(resourceDropAmount * ModifierManager.instance.TryGetModifierValue("wealth"));
            for (int i = 0; i < resourceDropAmount; i++)
            {
                Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle.normalized * .3f;

                GameObject res = Instantiate(resourceDrop, randomPos, Quaternion.identity);

                //add random force to the resource
                Rigidbody2D resRb = res.GetComponent<Rigidbody2D>();
                resRb.AddForce(Random.insideUnitCircle * resourceDropForce, ForceMode2D.Impulse);
            }
        }
        OnDeath.Invoke();

        if (deathParticles != null)
        {
            //   InstantiateDeathParticles();
            GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        }

        if (isEnemy)
        {
            GameManager.instance.AddScore(10);
            Destroy(gameObject, .2f);

        }
        else
        {
            GetComponent<PlayerControls>().playerCoreSprite.SetActive(false);
            GetComponent<PlayerControls>().enabled = false;
        }

    }



}
