using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    [SerializeField] private bool runOnce;
    [SerializeField] private float positionOffset;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate() {
        float precision = 5f;
        spriteRenderer.sortingOrder = -(int)((transform.position.y + positionOffset) * precision);

        if (runOnce) {
            Destroy(this);
        }
    }
}
