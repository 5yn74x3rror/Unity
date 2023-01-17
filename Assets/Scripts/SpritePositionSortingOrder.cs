using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour {
    private SpriteRenderer spriteRenderer;

    [SerializeField] private bool runOnce;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate() {
        float precision = 5f;
        spriteRenderer.sortingOrder = -(int)(transform.position.y * precision);

        if (runOnce) {
            Destroy(this);
        }
    }
}
