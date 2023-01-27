using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangeEventArgs> OnActiveBuildingTypeChange;
    public class OnActiveBuildingTypeChangeEventArgs : EventArgs {
        public BuildingTypeSO activeBuildingType;
    }

    private Camera mainCamera;
    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;

    private void Awake() {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }
    
    private void Start() {
        mainCamera = Camera.main;
    }
    
    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            if (activeBuildingType != null && CanSpawnBuilding(activeBuildingType, Utilities.GetMouseWorldPosition())) {
                Instantiate(activeBuildingType.prefab, Utilities.GetMouseWorldPosition(), Quaternion.identity);
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType) {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChange?.Invoke(this,
            new OnActiveBuildingTypeChangeEventArgs { activeBuildingType = activeBuildingType }
        );
    }

    public BuildingTypeSO GetActiveBuildingType() {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position) {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2Darray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
    
        bool isAreaClear = collider2Darray.Length == 0;

        if (!isAreaClear) {
            return false;
        }

        collider2Darray = Physics2D.OverlapCircleAll(position, buildingType.minContructionRadius);
        // make sure buildings are not too close
        foreach (Collider2D collider in collider2Darray) {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null && buildingTypeHolder.buildingType == buildingType) {
                // already a building nearby of same type
                return false;
            }
        }

        float maxContructionRadius = 25f;
        collider2Darray = Physics2D.OverlapCircleAll(position, maxContructionRadius);
        // make sure new building is no too far away from the rest of buildings
        foreach (Collider2D collider in collider2Darray) {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null) {
                // already a building nearby of same type
                return true;
            }
        }

        return false;
    }
}
