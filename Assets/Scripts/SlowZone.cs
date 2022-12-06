using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.speed = 2.0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        controller.speed = 5.0f;

    }
}

