using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {
    
    private GameObject spriteGameObject;
    private void Awake() {
        spriteGameObject = transform.Find("sprite").gameObject;

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
        } else {
            Show(e.activeBuildingType.sprite);
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
