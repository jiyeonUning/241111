using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] Vector3 mouseDir;
    [SerializeField] float rotateSpeed;

    private float xRotate = 0.0f;

    private void LateUpdate()
    {
        float yRotateMove = Input.GetAxis("Mouse X") * rotateSpeed;
        float yRotate = transform.eulerAngles.y + yRotateMove;

        float xRotateMove = -Input.GetAxis("Mouse Y") * rotateSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateMove, -45f, 80f);

        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }
}
