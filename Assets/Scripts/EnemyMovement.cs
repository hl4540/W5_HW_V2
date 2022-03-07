using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {


    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;


    [SerializeField] private float detectionRange;
    [SerializeField] private float firingRange;
    [SerializeField] private float fireRateValue;


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Material alertColor;

    private Material normalColor;

    private MeshRenderer thisRenderer;

    private Vector3 targetNode;
    private Vector3 destinationNode;

    private float fireRateTimer;
    private float destinationNumber;
    private float distance;


    private int state;


    private bool isAlert;


    private GameObject player;

    private NavMeshAgent thisAgent;

    void Start()
    {
        thisAgent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player");

        state = 0;
        destinationNumber = 2;

        thisRenderer = GetComponent<MeshRenderer>();
        normalColor = thisRenderer.material;
    }

    void Update()
    {
        fireRateTimer -= Time.deltaTime;


        switch (state)
        {
            case 0:
                {
                    Patrolling();

                    isAlert = false;

                    break;
                }
            case 1:
                {
                    Attacking(false);

                    isAlert = true;

                    break;
                }
            case 2:
                {
                    Attacking(true);

                    isAlert = true;

                    break;
                }
            case 3:
                {
                    DetectPlayer();

                    break;
                }
        }
        thisAgent.SetDestination(destinationNode);
        AlertActive(isAlert);

        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= detectionRange)
        {
            Vector3 direction = player.transform.position - transform.position;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Player")
                {
                    if (distance <= firingRange)
                    {
                        state = 2;
                    }
                    else
                    {
                        state = 1;
                    }
                    
                    Debug.DrawRay(transform.position, direction, Color.green);
                }
                else
                {

                    Debug.DrawRay(transform.position, direction, Color.red);

                    if (state == 0)
                    {
                        return;
                    }
                    else
                    {
                        state = 3;
                    }
                }
            }
        }
    }

    void AlertActive(bool alertState)
    {
        
        if (alertState)
        {
            thisRenderer.material = alertColor;
        }
        else
        {
            thisRenderer.material = normalColor;
        }
    }

    void Patrolling()
    {

        if (destinationNumber == 1)
        {
            destinationNode = point1.position;

            distance = Vector3.Distance(transform.position, destinationNode);

            if (distance <= 1f)
            {
                destinationNumber = 2;
            }
        }

        if (destinationNumber == 2)
        {
            destinationNode = point2.position;

            distance = Vector3.Distance(transform.position, destinationNode);

            if (distance <= 1f)
            {
                destinationNumber = 1;
            }
        }
    }

    void Attacking(bool isFiring)
    {
        destinationNode = player.transform.position;

        if (isFiring)
        {
            if (fireRateTimer < 0)
            {
                fireRateTimer = fireRateValue;
                FireAtTarget(player);
            }
        }
    }

    void FireAtTarget(GameObject target)
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);

        targetNode = target.transform.position;

        Bullet b = bulletGO.GetComponent<Bullet>();
        b.target = targetNode;
        b.enemyBullet = true;
    }

    void DetectPlayer()
    {

        distance = Vector3.Distance(transform.position, destinationNode);

        if (distance <= 1f)
        {
            state = 0;
        }
    }
}
