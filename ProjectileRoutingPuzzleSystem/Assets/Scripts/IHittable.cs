using UnityEngine;

public interface IHittable
{
    // This function is called when something hits the object
    // The GameObject parameter tells what object caused the hit
    public void Hit(GameObject gameObject);
}