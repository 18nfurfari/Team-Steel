using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRange = 10f;
    Transform target;
    NavMeshAgent agent;

    public Transform bulletSpawnPoint;

    public int currentAmmo;
    private bool reloading;
    private float reloadTime;

    [SerializeField] private float bulletSpeed = 30f;

    // Time between shots
    private float shootDelay = 1.0f;
    private float timeSinceLastShot = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        reloading = false;
        reloadTime = 3.0f;
        currentAmmo = 5;

        StartCoroutine(SetPlayerAsTargetAfterDelay(2f));
    }

    IEnumerator SetPlayerAsTargetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (reloading)
            {
                Reload();
            }

            if (distance <= lookRange)
            {
                    FaceTarget();
            }

            // Check if enough time has passed since the last shot
            if (distance <= lookRange && timeSinceLastShot >= shootDelay)
            {
                if (!reloading)
                {
                    Shoot();
                    timeSinceLastShot = 0.0f;
                }
            }

            timeSinceLastShot += Time.deltaTime;
        }else
        {
             StartCoroutine(SetPlayerAsTargetAfterDelay(2f));
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void Reload()
    {
        if (reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
        }
        else
        {
            reloading = false;
            currentAmmo = 1;
        }
    }

    private void Shoot()
    {

        Debug.Log("FIRE!");
        currentAmmo--;

        if (currentAmmo <= 0)
        {
            Debug.Log("Reloading!");
            reloading = true;
            reloadTime = 2.0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRange);
    }
}
