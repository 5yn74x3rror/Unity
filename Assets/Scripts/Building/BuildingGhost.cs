using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {
    private GameObject spriteGameObject;
    private ResourceNearbyOverlay resourceNearbyOverlay;

    private void Awake() {
        spriteGameObject = transform.Find("sprite").gameObject;
        resourceNearbyOverlay = transform.Find("pfResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();
        Hide();
    }

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChange += BuildingManager_OnActiveBuildingTypeChange;
    }

    private void Update() {
        transform.position = Utilities.GetMouseWorldPosition();
    }

    private void BuildingManager_OnActiveBuildingTypeChange(object sender, BuildingManager.OnActiveBuildingTypeChangeEventArgs e) {
        if (e.activeBuildingType == null) {
            Hide();
            resourceNearbyOverlay.Hide();
        } else {
            Show(e.activeBuildingType.sprite);
            if (e.activeBuildingType.hasResourceGeneratorData) {
                resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            } else {
                resourceNearbyOverlay.Hide();
            }
        }
    }

    private void Show(Sprite ghostSprite) {
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
        spriteGameObject.SetActive(true);
    }

    private void Hide() {
        spriteGameObject.SetActive(false);
    }
}
