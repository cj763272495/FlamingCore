using UnityEngine;

public class AutoRotationMode: IRotationMode {
    private Transform transform;
    private float rotateSpeed;

   public AutoRotationMode(Transform transform, float roSpeed=10f) {
        this.transform = transform;
        rotateSpeed = roSpeed;
    }

    public void Rotate(Transform player = null) {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
    }
     
}
