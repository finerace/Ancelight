using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMasks : MonoBehaviour
{
    
    [SerializeField] private LayerMask playerShootingLayerMask;
     public LayerMask PlayerShootingLayerMask { get { return playerShootingLayerMask; } }

}
