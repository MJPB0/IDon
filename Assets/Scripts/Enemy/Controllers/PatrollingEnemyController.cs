using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatrollingEnemyController : EnemyController
{
    [Header("Patrolling")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int waypointIndex = 0;
    [SerializeField] private float distanceToWaypoint = .5f;

    #region Getters & Setters
    public Transform[] Waypoints { get { return waypoints; } }
    public int WaypointIndex { get { return waypointIndex; } set { waypointIndex = value; } }
    public float DistanceToWaypoint { get { return distanceToWaypoint; } }
    #endregion

    public readonly EnemyPatrollingMode enemyPatrollingMode = new EnemyPatrollingMode();
}
