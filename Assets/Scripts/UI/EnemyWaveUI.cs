using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemyWaveUI : MonoBehaviour {
    [SerializeField] private EnemyWaveManager waveManager;

    private Camera mainCamera;
    private TextMeshProUGUI waveNumberText;    
    private TextMeshProUGUI messageText;
    private RectTransform nextWaveSpawnIndicator;
    private RectTransform closestEnemyIndicator;

    private void Start() {
        mainCamera = Camera.main;
        waveManager.OnWaveNumberChanged += WaveNumberChanged;
        SetWaveNumberText("Wave " + waveManager.GetWaveNumber());
    }

    private void Awake() {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        messageText = transform.Find("messageText").GetComponent<TextMeshProUGUI>();
        nextWaveSpawnIndicator = transform.Find("nextWaveSpawnIndicator").GetComponent<RectTransform>();
        closestEnemyIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
    }

    private void Update() {
        HandleNextWaveMessage();
        HandleNextWaveIndicator();
        HandleEnemyClosestPositionIndicator();
    }

    private void HandleNextWaveMessage() {
        float nextWaveSpawnTimer = waveManager.GetTimeForNextWave();

        if (nextWaveSpawnTimer <= 0f) {
            SetMessageText("");
        } else {
            SetMessageText("Next wave in " + nextWaveSpawnTimer.ToString("F1") + "s");
        }
    }

    private void HandleNextWaveIndicator() {
        Vector3 directionToNextSpawnPosition = (waveManager.GetNextSpawnPosition() - mainCamera.transform.position).normalized;
        float distanceToNextSpawnPosition = Vector3.Distance(waveManager.GetNextSpawnPosition(), mainCamera.transform.position);
        nextWaveSpawnIndicator.anchoredPosition = directionToNextSpawnPosition * 300f;
        nextWaveSpawnIndicator.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVector(directionToNextSpawnPosition));
        nextWaveSpawnIndicator.gameObject.SetActive(distanceToNextSpawnPosition > mainCamera.orthographicSize * 1.5f);
    }

    private void HandleEnemyClosestPositionIndicator() {
        float targetMaxRadius = 999f;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(mainCamera.transform.position, targetMaxRadius);
        Enemy targetEnemy = null;
        foreach (Collider2D collider2D in collider2Ds) {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null) {
                if (targetEnemy == null) {
                    targetEnemy = enemy;
                } else {
                    if (
                        Vector3.Distance(transform.position, enemy.transform.position) < 
                        Vector3.Distance(transform.position, targetEnemy.transform.position)
                    ) {
                        targetEnemy = enemy;
                    }
                }
            }
        }

        if (targetEnemy != null) {
            Vector3 directionToclosestEnemey = (targetEnemy.transform.position - mainCamera.transform.position).normalized;
            float distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, mainCamera.transform.position);
            closestEnemyIndicator.anchoredPosition = directionToclosestEnemey * 250f;
            closestEnemyIndicator.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVector(directionToclosestEnemey));
            closestEnemyIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f);
        } else {
            closestEnemyIndicator.gameObject.SetActive(false);
        }

    }

    private void WaveNumberChanged(object sender, EventArgs e) {
        SetWaveNumberText("Wave " + waveManager.GetWaveNumber());
    }

    private void SetMessageText(string text) {
        messageText.SetText(text);
    }

    private void SetWaveNumberText(string text) {
        waveNumberText.SetText(text);
    }
}
