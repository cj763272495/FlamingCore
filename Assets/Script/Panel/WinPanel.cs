using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Text curLevel;
    public Text coinTxt;

    private readonly int minSize = 30;
    private readonly int maxSize = 80;
    private readonly float cycleDuration = 2.0f;
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
        InvokeRepeating(nameof(Breathe), 0f, breathRate);
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
        }
    }
    void Breathe() {
        float t = Mathf.PingPong(Time.time, breathRate) / breathRate;
        float alpha = Mathf.SmoothStep(0, 1, t); // ƽ������
        BreathImage.color = new Color(1, 1, 1, alpha);
    }

    public void ClickReturnHomeBtn() {
        LeaveWinPanel();
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickNextWaveBtn() {
        LeaveWinPanel();
        int nextWave = GameRoot.Instance.CurWaveIndex + 1;
        GameRoot.Instance.StartBattle(nextWave);
    }

    public void ClickAgainBtn() {
        LeaveWinPanel();
        GameRoot.Instance.StartBattle(GameRoot.Instance.CurWaveIndex);
    }

    private void LeaveWinPanel() {
        StopAllCoroutines();
        CancelInvoke("Breathe");
        chestParticle.Stop();
    }


}