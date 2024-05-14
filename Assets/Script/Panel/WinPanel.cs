using System.Collections; 
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Text curLevel;
    public TextMeshProUGUI coinTxt;
     
    public float breathRate = 2.0f;
    public Image BreathImage; 
    public Chest chest;

    public void SetWinPanelLevelTxt(string wave) {
        curLevel.text = wave;
    }

    public void OpenWinPanel(int coinNum) {
        coinTxt.text = "+ " + coinNum.ToString();
        gameObject.SetActive(true);  
        chest.OpenChest();
        StartCoroutine(BreathingLight());
    }

    public void Show1stCoinTxt() {
        coinTxt.gameObject.SetActive(true);
        StartCoroutine(MoveAndFadeOut(coinTxt.gameObject,2f,1.5f));
    }

    private IEnumerator MoveAndFadeOut(GameObject obj,float delay,float duration) {
        yield return new WaitForSeconds(delay);

        Vector3 startPos = obj.transform.position;
        Vector3 endPos = startPos + new Vector3(0,1,0); // 上移的目标位置
        Color startColor = obj.GetComponent<TextMeshProUGUI>().color;
        Color endColor = new Color(startColor.r,startColor.g,startColor.b,0); // 透明的目标颜色

        float time = 0;
        while(time < duration) {
            time += Time.deltaTime;
            float t = time / duration;
            obj.transform.position = Vector3.Lerp(startPos,endPos,t);
            obj.GetComponent<TextMeshProUGUI>().color = Color.Lerp(startColor,endColor,t);
            yield return null;
        }

        obj.GetComponent<TextMeshProUGUI>().color = startColor;
        obj.SetActive(false);
    }

    private IEnumerator BreathingLight() {
        while(true) {
            float alpha = Mathf.PingPong(Time.time, 0.7f) + 0.3f;
            BreathImage.color = new Color(BreathImage.color.r,BreathImage.color.g,BreathImage.color.b,alpha);
            yield return null;
        }
    }

    public void ClickReturnHomeBtn() {
        LeaveWinPanel();
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickNextWaveBtn() {
        LeaveWinPanel();
        BattleSys.Instance.battleMgr.StratNextLevel();
    }

    public void ClickAgainBtn() {
        StopAllCoroutines();
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.PlayAgain();
        chest.Exit();
    }

    private void LeaveWinPanel() {
        gameObject.SetActive(false);
        BattleSys.Instance.battleMgr.DestoryBattle();
        StopAllCoroutines(); 
        chest.Exit();
    }
}
