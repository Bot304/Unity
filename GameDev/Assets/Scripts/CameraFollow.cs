using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // ������ �� ������ ������
    public Vector3 offset = new Vector3(0, 3, -10); // �������� ������
    public float smoothTime = 0.3f; // ����� ����������� �������� ������

    private Vector3 currentVelocity; // ���������� ��� ������� �������� SmoothDamp

    void FixedUpdate()
    {
        if (player != null)
        {
            // ������� ������� ������ � ������ ��������
            Vector3 targetPosition = player.position + offset;

            // ������� �������� ������ � ������� �������
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        }
    }

}
