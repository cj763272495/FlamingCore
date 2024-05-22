using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public GameObject soundPlayer;
    public GameObject go;

    void Start() {
        Instance = this;
        go = new() {
            name = "AudioPlayers"
        };
        go.transform.SetParent(transform);
        PoolManager.Instance.InitPool(soundPlayer, 10, go.transform);
    }

    public void PlaySound(AudioClip audioClip, float pitchMin = 1, float pitchMax = 1) {
        SoundPlayer player = PoolManager.Instance.
            GetInstance<GameObject>(soundPlayer,go.transform).GetComponent<SoundPlayer>();
        player.PlayClip(audioClip, pitchMin, pitchMax);
        StartCoroutine(ReturnPool(player));
    }

    IEnumerator ReturnPool(SoundPlayer player) { 
        yield return new WaitWhile(() => player.audioSource.isPlaying);
        PoolManager.Instance.ReturnToPool(player.gameObject);
    }
}
