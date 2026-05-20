using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private float _spreadAngle = 15f;

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _spawnPoint;

    private void SingleShot()
    {
        SpawnProjectile(0f);
    }

    private void SpreadShoot()
    {
        // Middle projectile
        SpawnProjectile(0f);

        // Left projectile
        SpawnProjectile(-_spreadAngle);

        // Right projectile
        SpawnProjectile(_spreadAngle);
    }

    void SpawnProjectile(float angleOffset)
    {
        Quaternion rotation = _spawnPoint.rotation * Quaternion.Euler(0f, angleOffset, 0f);

        Instantiate(_projectilePrefab, _spawnPoint.position, rotation);
    }
}
