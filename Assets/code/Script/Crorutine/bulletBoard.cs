using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBoard : MonoBehaviour
{
    // 총알은 카메라 앞 방향에서 발사된다.
    private void LateUpdate() { transform.forward = Camera.main.transform.forward; }
}
