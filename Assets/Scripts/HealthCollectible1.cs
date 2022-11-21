using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible1 : MonoBehaviour

{
    public AudioClip collectedClip;
    public ParticleSystem HealthEffect;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if(controller.health  < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                HealthEffect = Instantiate(HealthEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
        }
    }
}


