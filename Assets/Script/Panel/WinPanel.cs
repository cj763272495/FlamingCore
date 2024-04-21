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

    public void SetWinPanelLevelTxt(string wave) {
        curLevel.text = wave;
    }

    public void OpenWinPanel(int coinNum) {
        coinTxt.text = "X " + coinNum.ToString();
        gameObject.SetActive(true);
        StartCoroutine(CycleTextMeshProSize());
    }
    private IEnumerator CycleTextMeshProSize() {
        while (true) {
            float currentTime = 0f;
            while (currentTime < cycleDuration) {
                coinTxt.fontSize = Mathf.FloorToInt(Mathf.Lerp(minSize, maxSize, currentTime / cycleDuration));
                currentTime += Time.deltaTime;
                yield return null;
            }
            coinTxt.fontSize = maxSize; // ȷ���ı���С�ﵽ���ֵ
            currentTime = 0f; // ���ü�ʱ��
        }
    }

    public void ClickReturnHomeBtn() {
        StopAllCoroutines();
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickNextWaveBtn() {
        StopAllCoroutines();
        int nextWave = GameRoot.Instance.curWaveIndex + 1;
        GameRoot.Instance.StartBattle(1);
    }

    public void ClickAgainBtn() {
        StopAllCoroutines();
        int nextLevel = GameRoot.Instance.curWaveIndex;
        GameRoot.Instance.StartBattle(1);
    }


}
