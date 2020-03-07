using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Tower, Robot
}

public class TowerAI : MonoBehaviour
{
    public EnemyType enemy_type = EnemyType.Tower;
    public float fire_recharge = 1.0f;
    public float rotation_speed = 1.0f;
    public float movement_speed = 1.0f;
    public float projectile_force = 10.0f;
    public Player player;
    public GameObject projectile;
    public List<GameObject> robot_waypoints;

    private bool shooting_ = false;
    private float fire_timer = 0.0f;
    private int current_waypoint = 0;

    void Start()
    {
        
    }

    void Update()
    {
        Move();
        Look();
        Shoot();
    }

    private void Move()
    {
        if (shooting_)
        {
            gameObject.transform.LookAt(player.gameObject.transform, Vector3.up);
        }
        else
        {
            if(enemy_type == EnemyType.Tower)
            {
                gameObject.transform.Rotate(Vector3.up, rotation_speed * Mathf.PI * Time.deltaTime);
            }
            else
            {
                Vector3 dir = robot_waypoints[current_waypoint].transform.position - gameObject.transform.position;
                dir.y = 0.0f;
                gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, Vector3.Normalize(dir), rotation_speed * Time.deltaTime * Mathf.PI, 1.0f);
                gameObject.transform.position += Vector3.Normalize(dir) * Time.deltaTime * movement_speed;

                if(dir.magnitude < Mathf.Epsilon * 10)
                {
                    current_waypoint = (current_waypoint + 1) % robot_waypoints.Count;
                }
            }
        }
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            shooting_ = hit.collider.gameObject == player.gameObject;
        }
        else
        {
            shooting_ = false;
        }
    }

    private void Shoot()
    {
        if (shooting_)
        {
            fire_timer += Time.deltaTime;

            if(fire_timer >= fire_recharge)
            {
                fire_timer -= fire_recharge;
                GameObject bullet = Instantiate(projectile);
                projectile.GetComponent<Rigidbody>().AddForce(transform.forward);
            }
        }
        else
        {
            fire_timer = 0.0f;
        }
    }
}
