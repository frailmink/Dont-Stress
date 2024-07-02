using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collectDistance = 4f;

    public Transform player = null;
    public Rigidbody2D rb;

    void Update()
    {   
        if (player != null)
        {
            Vector3 pos = player.position;  
            Vector2 vel = new Vector2(pos.x - transform.position.x, pos.y - transform.position.y);
            vel = vel.normalized;
            vel *= moveSpeed;
            rb.velocity = vel;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        CoinManager.Instance.AddCoin();
        Destroy(gameObject);
    }
}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CoinScript : MonoBehaviour
// {
//     public float moveSpeed = 1f; // Speed at which the coin moves towards the player
//     public float collectDistance = 4f; // Distance at which the coin will be collected by the player

//     public Transform player = null;
//     public Rigidbody2D rb;
//     // public GameObject player;
//     void Update()
//     {   
//         // player = GameObject.FindGameObjectWithTag("Player").transform;
//         // Check if the coin is not collected and the player is within the collectDistance
//         if (player != null)
//         {
//             // transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
//             Vector3 pos = player.position;  
//             Vector2 vel = new Vector2(pos.x - transform.position.x, pos.y - transform.position.y);
//             vel = vel.normalized;
//             vel *= moveSpeed;
//             rb.velocity = vel;
//         }
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             CollectCoin();
//         }
//     }

//     void CollectCoin()
//     {
//         Destroy(gameObject); // Destroy the coin GameObject
//     }
// }

