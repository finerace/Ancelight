using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BulletData", menuName = "BulletsData", order = 51)]
public class BulletData : ScriptableObject
{

    [SerializeField] private int maxBullets;
    [SerializeField] private int id;

    [SerializeField] private float yellowPlasmaCreateCost;
    [SerializeField] private float redPlasmaCreateCost;
    [SerializeField] private float bluePlasmaCreateCost;
    
    [SerializeField] private Sprite icon;
    
    public int MaxBullets => maxBullets;
    public int Id => id;

    public float YellowPlasmaCreateCost => yellowPlasmaCreateCost;
    public float RedPlasmaCreateCost => redPlasmaCreateCost;
    public float BluePlasmaCreateCost => bluePlasmaCreateCost;
    
    public Sprite Icon => icon;
    
    
    
}
