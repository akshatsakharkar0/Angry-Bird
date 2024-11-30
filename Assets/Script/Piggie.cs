using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piggie : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float damageThreshold =0.2f;
    [SerializeField] private GameObject PiggieDeath;
    [SerializeField] private AudioClip deathClip;

    private float curentHealth;

    private void Awake()
    {
        curentHealth = maxHealth;
    }
    public void DamageDone(float damageAmount){
        curentHealth -= damageAmount;

        if(curentHealth <= 0f){
            Die();
        }
    }

    private void Die(){
        GameManager.instance.RemovePiggie(this);
        Instantiate(PiggieDeath, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathClip,transform.position);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;
        if(impactVelocity > damageThreshold){
            DamageDone(impactVelocity);
        }
    }
}
