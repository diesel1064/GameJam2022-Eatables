using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEatable
{
    public GameObject BloodSplatter { get; set; }
    public GameObject BloodPool { get; set; }
    public void KillMe();
    public void TriggerBloodSplatter();
    public void TriggerBloodPool();
}
