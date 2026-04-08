using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseUtilities
{
    private static Camera _camera = Camera.main;

    //give it a Z value in case the position need to be a little bit higher or lower on the Z axis
    public static Vector3 MousePositionInWorldSpace(float zValue = 0f)
    {
        //plane wich will track the position, used to be able to always say on wich plane to drag the card
        Plane dragPlane = new(_camera.transform.forward, new Vector3(0, 0, zValue));
        //create a ray from the screen point
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        //if the ray hits a point on the plane, return the point. Otherwise, return vector3.zero, witch should never happen in this case
        if (dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
