using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector3 target;

    public bool enemyBullet;

    [SerializeField]
    private float speed;

    private Vector3 direction;

    private void Start()
    {
        direction = target - transform.localPosition;
    }

    void Update()
    {
        float distThisFrame = speed * Time.deltaTime;

        transform.Translate(direction.normalized * distThisFrame, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyBullet)
        {
            if (other.tag == "Enemy")
            {
                return;
            }
            else if (other.tag == "Player")
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                return;
            }
            else if (other.tag == "Enemy")
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
