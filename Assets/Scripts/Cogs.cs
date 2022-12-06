using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cogs : MonoBehaviour
{
    public AudioClip collectedClip;

    public ParticleSystem CogCollectEffectObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if(controller.ammo <= controller.currentAmmo)
            {
                controller.ChangeAmmo(4);
                controller.AmmoText();
                CogCollectEffectObject = Instantiate(CogCollectEffectObject, transform.position, Quaternion.identity);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
        }
    }
}
