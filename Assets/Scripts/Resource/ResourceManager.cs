using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public static ResourceManager Instance { get; private set; }
    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;
    public event EventHandler OnResourceAmountChange;

    [SerializeField] private List<ResourceAmount> startingResourceAmountList;

    private void Awake() {
        Instance = this;

        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSO resourceType in resourceTypeList.list) {
            resourceAmountDictionary[resourceType] = 0;
        }
        foreach (ResourceAmount resourceAmount in startingResourceAmountList) {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount) {
        resourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChange?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSO resourceType) {
        return resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray) {
        foreach (ResourceAmount resourceAmount in resourceAmountArray) {
            if (GetResourceAmount(resourceAmount.resourceType) < resourceAmount.amount) {
                return false;
            }
        }
        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray) {
        foreach (ResourceAmount resourceAmount in resourceAmountArray) {
            resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }
}
