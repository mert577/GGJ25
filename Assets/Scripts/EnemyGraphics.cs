using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyGraphics : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Health health;

    Color originalColor;
    [SerializeField]
    float punchScaleMagnitutde;


    [SerializeField]
    GameObject hurtParticles;







     private void Awake() {
        originalColor = spriteRenderer.color;
    }
    // Start is called before the first frame update
    void Start()
    {
        health.OnDamageTaken.AddListener(PlayDamageAnimation);
        health.OnDeath.AddListener(DeathAnnimation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayDamageAnimation(int health)
    {
        IEnumerator DamageAnimation()
        {
            //flash white and do scale bounce

            spriteRenderer.DORewind();
            spriteRenderer.transform.DORewind();
            spriteRenderer.DOColor(Color.white, 0.1f);
            spriteRenderer.transform.DOPunchScale(Vector3.one * punchScaleMagnitutde, 0.2f);
            GameObject particles = Instantiate(hurtParticles, transform.position, Quaternion.identity);
            Destroy(particles, 1f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.DOColor(originalColor, 0.1f);

     

        }


        StartCoroutine(DamageAnimation());
    }

    void DeathAnnimation()
    {
        //play death animation
    }

    private void OnDestroy() {
        health.OnDamageTaken.RemoveListener(PlayDamageAnimation);
        spriteRenderer.DOKill();
        transform.DOKill();
    }
}
