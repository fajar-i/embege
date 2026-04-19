using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Karakter (CharacterCapsule)
    public Vector3 offset = new Vector3(0, 5, -7); // Jarak kamera (Atas, Belakang)

    [Header("Smooth Settings")]
    public float smoothSpeed = 5f;       // Kecepatan mengikuti posisi
    public float rotationSmooth = 2f;    // Kecepatan rotasi (semakin kecil semakin lambat)

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Menghitung posisi yang diinginkan berdasarkan rotasi karakter
        // Kamera akan selalu berada di belakang karakter secara lokal
        Vector3 desiredPosition = target.TransformPoint(offset);

        // 2. Interpolasi posisi (Smooth Move)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // 3. Membuat kamera selalu menghadap ke karakter
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        
        // Offset tambahan sedikit ke atas agar tidak tepat melihat kaki
        lookRotation *= Quaternion.Euler(-10, 0, 0); 

        // 4. Interpolasi rotasi (Slow Rotation)
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSmooth * Time.deltaTime);
    }
}