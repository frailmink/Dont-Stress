using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float rapidFireRate = 0f; // Time between each shot in rapid fire mode

    private Coroutine rapidFireCoroutine;

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

    public void StartRapidFire(InputAction.CallbackContext context)
    {
        if (rapidFireCoroutine == null)
        {
            rapidFireCoroutine = StartCoroutine(RapidFire());
        }
    }

    public void StopRapidFire(InputAction.CallbackContext context)
    {
        if (rapidFireCoroutine != null)
        {
            StopCoroutine(rapidFireCoroutine);
            rapidFireCoroutine = null;
        }
    }

    private IEnumerator RapidFire()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(rapidFireRate);
        }
    }
}
