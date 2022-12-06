using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarRush : MonoBehaviour
{
    public AudioClip SugarRushSound;
    public ParticleSystem SugarRushEffectObject;

    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.SpeedBoost(1);
            SugarRushEffectObject = Instantiate(SugarRushEffectObject, transform.position, Quaternion.identity);
            Destroy(gameObject);

            controller.PlaySound(SugarRushSound);
        }
    }
}
