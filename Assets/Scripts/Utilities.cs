using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {
    private static Camera mainCamera;
    public static Vector3 GetMouseWorldPosition() {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }

    public static Vector3 GetRandomDirection() {
        return new Vector3(Random.Range(3f, -3f), Random.Range(3f, -3f)).normalized;
    }

    public static float GetAngleFromVector(Vector3 vector) {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }
}
