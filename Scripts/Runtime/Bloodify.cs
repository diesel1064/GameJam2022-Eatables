using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodify : MonoBehaviour
{
    private ParticleSystem bloodSplatter;
    private List<ParticleCollisionEvent> bloodifyEvents;

    private void Start()
    {
        bloodSplatter = GetComponent<ParticleSystem>();
        bloodifyEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        var bloodObject = other.GetComponentInChildren(typeof(BloodManager)) as BloodManager;
        if (bloodObject != null)
        {
            // Debug.Log($"bloodObject {bloodObject}");
            bloodObject.TriggerBloodifyEvent();
        }
        else
        {
            bloodObject = other.GetComponentInChildren(typeof(BloodManager), true) as BloodManager;
            if (bloodObject != null)
            {
                // Debug.Log($"bloodObject {bloodObject}");
                bloodObject.TriggerBloodifyEvent();
            }
        }
    }
}
