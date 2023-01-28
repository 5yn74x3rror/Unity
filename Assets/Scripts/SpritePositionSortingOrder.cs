using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour {
    private SpriteRenderer spriteRendererInChild;
    private SpriteRenderer spriteRendererOnEl;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private bool runOnce;
    [SerializeField] private float positionOffset;

    private void Awake() {
        if (transform.Find("sprite")) {
            spriteRendererInChild = transform.Find("sprite").GetComponent<SpriteRenderer>();
        } else {
            spriteRendererInChild = null;
        }
        spriteRendererOnEl = GetComponent<SpriteRenderer>();
        
        if (spriteRendererInChild != null) {
            spriteRenderer = spriteRendererInChild;
        } else if (spriteRendererOnEl != null) {
            spriteRenderer = spriteRendererOnEl;
        }
    }

    private void LateUpdate() {
        float precision = 5f;
        if (spriteRenderer != null) {
            spriteRenderer.sortingOrder = -(int)((transform.position.y + positionOffset) * precision);
        }

        if (runOnce) {
            Destroy(this);
        }
    }
}
