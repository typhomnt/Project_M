using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float gate_open_time = 2f;

    public float curr_gate_time_ = 2f;
    private Material mat_;

    public void ResetGate()
    {
        curr_gate_time_ = gate_open_time;
    }

    public void OpenGate(float val)
    {
        curr_gate_time_ -= val;
        if(curr_gate_time_ <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateGateVisuals()
    {
        mat_.SetFloat("_GateStatus", curr_gate_time_ / gate_open_time);
    }

    // Start is called before the first frame update
    void Start()
    {
        mat_ = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGateVisuals();
    }
}
