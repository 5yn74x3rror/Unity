using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;

    private void Awake() {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.SetHealtAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDied += DestroyBuilding;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            healthSystem.Damage(10);
        }
    }

    private void DestroyBuilding(object sender, EventArgs e) {
        Destroy(gameObject);
    }
}
