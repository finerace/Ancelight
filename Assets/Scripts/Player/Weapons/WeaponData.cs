using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponData", menuName = "WeaponsData", order = 51)]
public class WeaponData : ScriptableObject
{

    [SerializeField] private int name_TextId;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private int id;
    [SerializeField] private Color mainColor;
    [Space]
    [SerializeField] private float damage;
    [SerializeField] private int bulletID;
    [Space]
    [SerializeField] private bool raycast;
    [SerializeField] private bool isMelee;
    [SerializeField] private bool oneClick;
    [SerializeField] private float rateOfFire;
    [SerializeField] private bool endTimeShoot;
    [SerializeField] private ShootingMode shootingMode;
    [Space]
    [SerializeField] private GameObject shootingEffects;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject weaponPrefab;

    public int NameTextId { get { return name_TextId; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public int Id { get { return id; } }
    public Color MainColor { get { return mainColor; } }

    public float Damage { get { return damage; } }

    public int BulletsID { get { return bulletID; } }

    public bool IsRaycast { get { return raycast; } }

    public bool IsMelee { get { return isMelee; } }

    public bool OneClick { get { return oneClick; } }

    public float RateOfFire { get { return rateOfFire; } }

    public bool EndTimeShoot { get { return endTimeShoot; } }

    public ShootingMode ShootingMode_ { get { return shootingMode; } }

    public GameObject ShootingEffects { get { return shootingEffects; } }

    public GameObject BulletPrefab { get { return bulletPrefab; } }

    public GameObject WeaponPrefab { get { return weaponPrefab; } }

    public enum ShootingMode
    {
        Normal,
        Adjustable,
        Lazer,
        Particle
    }
}
