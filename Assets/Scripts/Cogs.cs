using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cogs : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if(controller.ammo <= controller.currentAmmo)
            {
                controller.ChangeAmmo(4);
                controller.AmmoText();
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
        }
    }
}
