using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public enum EnemyType
    {
        Tower, Robot
    }

    public EnemyType enemy_type = EnemyType.Tower;
    public float fire_recharge = 1.0f;
    public float rotation_speed = 1.0f;
    public float movement_speed = 1.0f;
    public float projectile_force = 10.0f;
    public Player player;
    public GameObject projectile;
    public List<GameObject> robot_waypoints;
    public float fov = 90.0f;

    private bool shooting_ = false;
    private float fire_timer = 0.0f;
    private int current_waypoint = 0;
    private GameObject current_projectile;


    void Start()
    {
        ;
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
            gameObject.transform.parent.transform.LookAt(player.gameObject.transform, Vector3.up);
        }
        else
        {
            if (enemy_type == EnemyType.Tower)
            {
                gameObject.transform.Rotate(Vector3.up, rotation_speed * Mathf.PI * Time.deltaTime);
            }
            else
            {
                /*Vector3 dir = robot_waypoints[current_waypoint].transform.position - gameObject.transform.position;
                dir.y = 0.0f;
                gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, Vector3.Normalize(dir), rotation_speed * Time.deltaTime * Mathf.PI, 1.0f);
                gameObject.transform.position += Vector3.Normalize(dir) * Time.deltaTime * movement_speed;

                if (dir.magnitude < Mathf.Epsilon * 10)
                {
                    current_waypoint = (current_waypoint + 1) % robot_waypoints.Count;
                }*/
                gameObject.transform.parent.transform.Rotate(Vector3.up, rotation_speed * Mathf.PI * Time.deltaTime);
            }
        }
    }

    private void Look()
    {
        RaycastHit hit;
        //bool view_condition = Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity);
        bool view_condition =  Mathf.Abs(Mathf.Acos(Vector3.Dot(transform.parent.transform.forward, Vector3.Normalize(player.gameObject.transform.position - transform.position)))) < fov*Mathf.Deg2Rad;
        shooting_ = view_condition;
    }

    private void Shoot()
    {
        if (shooting_)
        {
            fire_timer += 0.1f;//Time.deltaTime;
            if (fire_timer >= fire_recharge)
            {
                fire_timer -= fire_recharge;
                current_projectile = Instantiate(projectile);
                current_projectile.transform.position = robot_waypoints[current_waypoint].transform.position;
                current_projectile.GetComponent<Rigidbody>().AddForce(transform.parent.transform.forward*100);
                current_projectile.GetComponent<Projectile>().player = player;
                current_waypoint = (current_waypoint + 1) % robot_waypoints.Count;
            }
        }
        else
        {
            fire_timer = 0.0f;
        }
    }
}
