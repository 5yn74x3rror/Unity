using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour {
    public static ArrowProjectile Create(Vector3 position, Enemy enemy) {
        Transform pfArrowProjectile = Resources.Load<Transform>("pfArrowProjectile");
        Transform arrowTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);
        
        ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);

        return arrowProjectile;
    }

    private Enemy targetEnemy;
    private Vector3 lastMoveDir;
    private float timeToDie = 2f;

    private void Update() {
        float moveSpeed = 20f;
        Vector3 moveDir;

        if (targetEnemy != null) {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        } else {
            moveDir = lastMoveDir;
        }

        transform.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVector(moveDir));
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        timeToDie -= Time.deltaTime;
        if (timeToDie < 0f) {
            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy targetEnemy) {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
