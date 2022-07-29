using UnityEngine;

public class RotateTrans : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3( 0, Time.deltaTime * 50, 0));
    }

}