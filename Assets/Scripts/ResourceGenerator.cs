using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {
    private BuildingTypeSO buildingType;
    private Dictionary<ResourceGeneratorData, float> timerDict;
    private Dictionary<ResourceGeneratorData, float> timerMaxDict;

    private void Awake() {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        timerDict = new Dictionary<ResourceGeneratorData, float>();
        timerMaxDict = new Dictionary<ResourceGeneratorData, float>();
 
        foreach (ResourceGeneratorData resourceGeneratorData in buildingType.resourceGeneratorData) {
            timerDict[resourceGeneratorData] = 0f;
            timerMaxDict[resourceGeneratorData] = resourceGeneratorData.timerMax;
        }
    }
 
    private void Update() {
        foreach (ResourceGeneratorData resourceGeneratorData in buildingType.resourceGeneratorData) {
            timerDict[resourceGeneratorData] -= Time.deltaTime;
            if (timerDict[resourceGeneratorData] <= 0f) {
                timerDict[resourceGeneratorData] += timerMaxDict[resourceGeneratorData];
                ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
            }
        }
    }
}
