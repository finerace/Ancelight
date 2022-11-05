using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour
{
    //??????????????? ???? ???? ????

    public static float Horizontal
    {
        get { return Input.GetAxisRaw("Horizontal"); }
    }

    public static float Vertical
    {
        get { return Input.GetAxisRaw("Vertical"); }
    }

    public static float Jump
    {
        get { return Input.GetAxisRaw("Jump"); }
    }

    public static float Fire1
    {
        get { return Input.GetAxisRaw("Fire1"); }
    }

    public static float Fire2
    {
        get { return Input.GetAxisRaw("Fire2"); }
    }

    public static float MouseX
    {
        get { return Input.GetAxis("Mouse X"); }
    }

    public static float MouseY
    {
        get { return Input.GetAxis("Mouse Y"); }
    }

    public static float MouseWheel
    {
        get { return Input.GetAxisRaw("Mouse ScrollWheel"); }
    }

}
