using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPowerScript : PowerScript
{
    public GameObject Electricity;
    public override void Run()
    {
        GameObject player = GameObject.FindWithTag("Player");
        // player.GetComponent<PlayerScript>().IncreaseMoveSpeed(10);
        GameObject instantiatedElec = Instantiate(Electricity, player.transform);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = mousePosition - player.transform.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg; // - 90f

        instantiatedElec.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        // Transform firepoint = player.transform.GetChild(0).transform.GetChild(0);
        // instantiatedElec.transform.localPosition = firepoint.localPosition;
    }
}
