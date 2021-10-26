using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathMode : EnemyMode
{
    public override void EnemyOnTriggerEnter(EnemyController enemy, Collider other)
    {

    }

    public override void EnemyOnTriggerExit(EnemyController enemy, Collider other)
    {

    }

    public override void EnemyUpdate(EnemyController enemy)
    {

    }

    public override void EnterMode(EnemyController enemy)
    {
        enemy.GetComponent<Collider>().enabled = false;
        enemy.GetComponent<Rigidbody>().useGravity = false;
        enemy.GetComponent<Rigidbody>().isKinematic = true;
        enemy.Enemy.CanAttack = false;
        enemy.Enemy.CanBeHit = false;
        enemy.DestroyEnemy();
    }
}
