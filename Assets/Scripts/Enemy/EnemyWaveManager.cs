using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour {
    public event EventHandler OnWaveNumberChanged;

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPositionTransform;
    
    private enum State {
        WitingToSpawnNextWave,
        SpawningWave
    }

    private State state;
    private int waveNumber = 0;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTime;
    private int remainingEnemiesInThisWave;
    private Vector3 spawnPosition;

    private void Start() {
        state = State.WitingToSpawnNextWave;
        nextWaveSpawnTimer = 3f;
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
    }

    private void Update() {
        switch (state) {
            case State.WitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f) {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemiesInThisWave > 0) {
                    nextEnemySpawnTime -= Time.deltaTime;
                    if (nextEnemySpawnTime < 0f) {
                        nextEnemySpawnTime = UnityEngine.Random.Range(0f, .2f);
                        Enemy.Create(spawnPosition + Utilities.GetRandomDirection() * UnityEngine.Random.Range(0f, 10f));
                        remainingEnemiesInThisWave--;
                        if (remainingEnemiesInThisWave <= 0f) {
                            state = State.WitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave() {
        remainingEnemiesInThisWave = 5 + (5 * waveNumber);
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber() {
        return waveNumber;
    }

    public float GetTimeForNextWave() {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetNextSpawnPosition() {
        return spawnPosition;
    }
}
    
