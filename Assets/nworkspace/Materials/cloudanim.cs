using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudanim : MonoBehaviour
{
    public float loopTime;
    public Vector3 initScale;

    private void Start()
    {
        loopTime = 0.0f;
        initScale = this.transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
    }
}
