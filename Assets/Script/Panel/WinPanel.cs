using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Text curLevel;
    public Text coinTxt;

    private int minSize = 30;
    private int maxSize = 80;
    private float cycleDuration = 2.0f;
    public ParticleSystem chestParticle;
    public float breathRate = 2.0f;
    public Image BreathImage;

    public void SetWinPanelLevelTxt(string wave) {
        curLevel.text = wave;
    }

    public void OpenWinPanel(int coinNum) {
        coinTxt.text = "+ " + coinNum.ToString();
        gameObject.SetActive(true);
        chestParticle.Play();
        StartCoroutine(CycleTextMeshProSize());
        InvokeRepeating("Breathe", 0f, breathRate);
    }
    private IEnumerator CycleTextMeshProSize() {
        while (true) {
            float currentTime = 0f;
            while (currentTime < cycleDuration) {
                coinTxt.fontSize = Mathf.FloorToInt(Mathf.Lerp(minSize, maxSize, currentTime / 2*cycleDuration));
                currentTime += Time.deltaTime;
                yield return null;
            }
            coinTxt.fontSize = minSize; // ȷ���ı���С�ﵽ���ֵ
            currentTime = 0f; // ���ü�ʱ��
        }
    }
    void Breathe() {
        float t = Mathf.PingPong(Time.time, breathRate) / breathRate;
        float alpha = Mathf.SmoothStep(0, 1, t); // ƽ������
        BreathImage.color = new Color(1, 1, 1, alpha);
    }

    public void ClickReturnHomeBtn() {
        StopAllCoroutines();
        CancelInvoke("Breathe");
        chestParticle.Stop();
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickNextWaveBtn() {
        StopAllCoroutines();
        CancelInvoke("Breathe");
        chestParticle.Stop();
        int nextWave = GameRoot.Instance.curWaveIndex + 1;
        GameRoot.Instance.StartBattle(nextWave);
    }

    public void ClickAgainBtn() {
        StopAllCoroutines();
        CancelInvoke("Breathe");
        chestParticle.Stop();
        GameRoot.Instance.StartBattle(GameRoot.Instance.curWaveIndex);
    }


}
