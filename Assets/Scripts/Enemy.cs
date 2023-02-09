using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public static Enemy Create(Vector3 position) {
        Transform pfEnemy = Resources.Load<Transform>("pfEnemy");
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();

        return enemy;
    }

    private Transform targetTransform;
    private Rigidbody2D rigidbody2D;
    private HealthSystem healthSystem;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;

    private void Start() {
        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        healthSystem = GetComponent<HealthSystem>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // if bunch enemies spawns at the same time they don't all look for targets at the same time
        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
        healthSystem.OnDied += EnemyDied;
    }

    private void Update() {
        HandleMovement();
        HandleTargetting();
    }

    private void EnemyDied(object sender, System.EventArgs e) {
        Destroy(gameObject);
    }

    private void HandleMovement() {
        if (targetTransform.position != null) {
            Vector3 moveDirection = (targetTransform.position - transform.position).normalized;
            float moveSpeed = 6f;
            rigidbody2D.velocity = moveDirection * moveSpeed;
        } else {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void HandleTargetting() {
        // if lots of enemies they all don't query LookForTargets constantly
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null) {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            Destroy(gameObject);
        }
    }

    private void LookForTargets() {
        float targetMaxRadius = 10f;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);
        foreach (Collider2D collider2D in collider2Ds) {
            Building building = collider2D.GetComponent<Building>();
            if (building != null) {
                if (targetTransform == null) {
                    targetTransform = building.transform;
                } else {
                    if (
                        Vector3.Distance(transform.position, building.transform.position) < 
                        Vector3.Distance(transform.position, targetTransform.position)
                    ) {
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if (targetTransform == null) {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
    }
}
