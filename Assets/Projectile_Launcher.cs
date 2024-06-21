using UnityEngine;

public class Projectile_Launcher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public float launchSpeed = 10f;

    void Start()
    {
        // Call the LaunchAutomatically method every 3 seconds
        InvokeRepeating("LaunchAutomatically", 0, 3);
    }

    void LaunchAutomatically()
    {
        // Instantiate the projectile
        var _projectile = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation);
        // Set the velocity of the projectile
        _projectile.GetComponent<Rigidbody>().velocity = launchPoint.up * launchSpeed;
    }
}
