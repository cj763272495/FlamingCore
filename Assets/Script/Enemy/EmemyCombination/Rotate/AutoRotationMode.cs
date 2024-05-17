using UnityEngine;

public class AutoRotationMode:IRotationMode {
    private Transform transform;
    private readonly float rotateSpeed = 10f;

   public AutoRotationMode(Transform transform) {
        this.transform = transform;
    }

    public void Rotate(Transform player = null) {
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
    }
     
}
