using UnityEngine;

public class SimpleLocalPositionSetter : MonoBehaviour
{
    [SerializeField] private Transform targetT;
    [SerializeField] private Vector3 toSetPos;

    public void SetPos()
    {
        targetT.localPosition = toSetPos;
    }

}
