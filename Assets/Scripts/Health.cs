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

    public UnityEvent OnDamageTaken = new UnityEvent();

    public bool isStunned = false;




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

            TakeDamage(damager.damageAmount,damager.transform.position, damager.isKnockback);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Damager>(out Damager damager))
        {

            if (damager.damagerType != Damager.DamagerType.EnterOnly)
            {

                TakeDamage(damager.damageAmount, damager.transform.position, damager.isKnockback);
            }
        }
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
        OnDamageTaken.Invoke();

        if (knockback)
        {
            Vector2 knockbackDirection = (rb.position - damagerPos).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(TurnStunned());
        }
        
        StartCoroutine(TurnInvincible(invincibilityTime));
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        Destroy(gameObject,.2f);

    }



}
