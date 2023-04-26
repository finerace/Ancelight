using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRain : ExtraAbility
{
    /*[SerializeField] private GameObject miniBombPrefab;
    [SerializeField] private Transform[] miniBombsDirections;
    [SerializeField] private int bombsAttackCount = 3;
    [SerializeField] private float bombsFlyPower = 3f;
    [SerializeField] private float oneAttackTime = 0.15f;
    [SerializeField] private float attacksRadiusCount = 2.5f;
    [SerializeField] private float fallingSpeed = 2.5f;
    private List<Rigidbody> rigidbodies = new List<Rigidbody>(128);

    private void FixedUpdate()
    {
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            var item = rigidbodies[i];

            Vector3 fallingVector = Physics.gravity * fallingSpeed;

            if (item != null)
                item.AddForce(fallingVector, ForceMode.Acceleration);
            else
                rigidbodies.Remove(item);
        }
    }

    public override void LaunchAbility()
    {
        StartCoroutine(StartAttacks());

        IEnumerator StartAttacks()
        {
            for (int i = bombsAttackCount; i >= 1; i--)
            {
                Attack(bombsFlyPower * (i*attacksRadiusCount/5f));
                yield return new WaitForSeconds(oneAttackTime);
            }
        }

        void Attack(float bombsFlyPower)
        {
            foreach (var item in miniBombsDirections)
            {
                Rigidbody thisRb = Instantiate(miniBombPrefab, item.position, item.localRotation)
                    .GetComponent<Rigidbody>();

                thisRb.AddForce(thisRb.transform.forward * bombsFlyPower,ForceMode.VelocityChange);

                rigidbodies.Add(thisRb);
            }
        }

        base.LaunchAbility();
    }
    */

}
