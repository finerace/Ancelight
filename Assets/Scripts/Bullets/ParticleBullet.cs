using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBullet : MonoBehaviour
{
#pragma warning disable CS0649 // ���� "ParticleBullet.damage" ����� �� ������������� ��������, ������� ��� ������ ����� ����� �������� �� ��������� 0.
    [SerializeField] private float damage;
#pragma warning restore CS0649 // ���� "ParticleBullet.damage" ����� �� ������������� ��������, ������� ��� ������ ����� ����� �������� �� ��������� 0.
    [SerializeField] private PlayerWeaponsManager weaponsManager;
    [SerializeField] private Vector3 forceDirection; //������ ���� �� ���������
    [SerializeField] private float forcePower = 1f;

    private void Start()
    {
        weaponsManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();

        forceDirectionUpdate();
        weaponsManager.SubscribeShotEvent(forceDirectionUpdate);
    }

    private void OnParticleCollision(GameObject other)
    {
        Health health;
        Rigidbody rb;

        if(other.TryGetComponent<Health>(out health))
        {
            health.GetDamage(damage);
        }

        if (other.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(forceDirection*forcePower);
        }
    }

    private void forceDirectionUpdate()
    {
        forceDirection = weaponsManager.shootingPoint.forward;
    }

}
