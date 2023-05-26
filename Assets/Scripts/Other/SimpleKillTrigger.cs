using UnityEngine;

public class SimpleKillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Health>(out Health bob))
        {
            bob.GetDamage(bob.Health_*100);
        }
    }
    

}
