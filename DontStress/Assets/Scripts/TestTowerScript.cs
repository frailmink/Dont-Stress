using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTowerScript : TowerScript
{
    public float maxTimer = 10;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("placed inside tower");
    }

    private void Update()
    {
        if (timer > maxTimer)
        {
            timer = 0;
            Debug.Log(strength);
        } else
        {
            timer += Time.deltaTime;
        }
    }
}
