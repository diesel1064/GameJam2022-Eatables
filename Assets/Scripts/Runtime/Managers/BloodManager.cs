using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BloodManager : MonoBehaviour
{
    [SerializeField] private Sprite bloodSprite;
    [SerializeField] private UnityEvent bloodifyEvent;

    public void TriggerBloodifyEvent()
    {
        bloodifyEvent?.Invoke();
    }

    public void SwapSprites()
    {
        GetComponent<SpriteRenderer>().sprite = bloodSprite;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Debug.Log($"Trigger My Blood {col.gameObject.name}");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Debug.Log($"Trigger My Blood {col.gameObject.name}");
    }
}
