using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform unitRagdollPrefab;
    [SerializeField] private Transform originalRootBone;

    private HealthSystem healthSystem;
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDie += HealthSystem_OnDie;
    }
    private void HealthSystem_OnDie(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(unitRagdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.SetUp(originalRootBone);
    }
}
