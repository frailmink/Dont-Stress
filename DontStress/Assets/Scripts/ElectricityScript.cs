using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.tag == "Enemy")
        {
            collidedObject.GetComponent<EnemyScript>().TakeDamage(50f);
        }
    }
}
