using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGeneratorOverlay : MonoBehaviour {
    [SerializeField] private ResourceGenerator resourceGenerator;

    private Transform barTransform;

    private void Start() {
        barTransform = transform.Find("bar"); 
        ResourceGeneratorData generatorData = resourceGenerator.GetResourceGeneratorData();

        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = generatorData.resourceType.sprite;
        transform.Find("text").GetComponent<TextMeshPro>().SetText(
            resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1")
        );
    }

    private void Update() {
        barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(), 1f, 1f);
    }
}
