using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBoard : MonoBehaviour
{
    // �Ѿ��� ī�޶� �� ���⿡�� �߻�ȴ�.
    private void LateUpdate() { transform.forward = Camera.main.transform.forward; }
}
