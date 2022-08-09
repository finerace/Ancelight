using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BulletData", menuName = "BulletsData", order = 51)]
public class BulletData : ScriptableObject
{

    [SerializeField] private int maxBullets;
    [SerializeField] private int id;
    [SerializeField] private Sprite icon;
    public int MaxBullets { get { return maxBullets; } }
    public int Id { get { return id; } }
    public Sprite Icon { get { return icon; } }

}
