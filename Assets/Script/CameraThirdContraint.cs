using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThirdContraint : MonoBehaviour
{

    public GameObject player;
    public float forward_offet_fac = 1.25f;
    private Collider player_collider;
    // Start is called before the first frame update
    void Start()
    {
        player_collider = player.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player_collider || player)
        {
            //this.transform.rotation = player.transform.rotation;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation,0.1f);
            Vector3 bound_size = player_collider.bounds.size;
            this.transform.position = player.transform.position - transform.forward* bound_size.magnitude* forward_offet_fac + (bound_size*0.5f);
        }
    }
}
