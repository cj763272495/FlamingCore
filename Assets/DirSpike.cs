using UnityEngine;

public class DirSpike : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player") {
            other.GetComponent<PlayerController>().PlayerDead();
        }
    }
}
