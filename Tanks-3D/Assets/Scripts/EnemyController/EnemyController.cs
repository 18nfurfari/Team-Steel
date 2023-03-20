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
    public GameObject bulletPrefab;

     public int currentAmmo;
     private bool reloading;
     private float reloadTime;

    [SerializeField] private float bulletSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("PlayerPrefab").transform;
        agent = GetComponent<NavMeshAgent>();

        reloading = false;
        reloadTime = 3.0f;
        currentAmmo = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
        float distance = Vector3.Distance(target.position, transform.position);

        if (reloading)
        {
            Reload();
        }

        if (distance <= lookRange)
        {
        agent.SetDestination(target.position);
        if(distance <= agent.stoppingDistance)
            FaceTarget();
        }
        if (distance <= lookRange){
                if(!reloading){
                 var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position,
                 bulletSpawnPoint.rotation * Quaternion.Euler(90, 0, 0));
                 bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;

                 Debug.Log("FIRE!");
                 if (currentAmmo <= 0)
                 {
                    Debug.Log("Reloading!");
                    reloading = true;
                    reloadTime = 2.0f;
                 }
                 else{
                    currentAmmo -= 1;
                 }
                 }
          }
        }
    }
    void FaceTarget ()
    {
    Vector3 direction = (target.position - transform.position).normalized;
    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime* 5f);
    }

    private void Reload()
        {
            // Wait until waitTime is below or equal to zero.
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


    void onDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRange);
    }
}
