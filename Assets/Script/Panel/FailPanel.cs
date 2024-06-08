using DG.Tweening;
using TMPro;
using UnityEngine;

public class FailPanel:MonoBehaviour {
    private Sequence sequence;
    public GameObject curWaveBg;
    public TextMeshProUGUI curLevelTxt;

    private void Start() { 
        sequence = DOTween.Sequence().SetUpdate(UpdateType.Normal,true);
        sequence.Append(curWaveBg.transform.DOScale(1,1f));
        sequence.AppendCallback(()=>curLevelTxt.gameObject.SetActive(true));
        sequence.Append(curLevelTxt.DOFade(0,0.1f).SetLoops(8,LoopType.Yoyo)); 
        sequence.SetAutoKill(false);
    }

    public void Show() {
        curLevelTxt.color = new Color(curLevelTxt.color.r,curLevelTxt.color.g,curLevelTxt.color.b,1);
        curLevelTxt.gameObject.SetActive(false);
        curWaveBg.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        gameObject.SetActive(true);
        sequence.Restart();
    }

    public void ClickReturnHomeBtn() {
        BattleSys.Instance.battleMgr.DestoryBattle();
        gameObject.SetActive(false);
        GameRoot.Instance.EnterMainCity();
    }

    public void ClickTryAgainBtn() {
        BattleSys.Instance.battleMgr.PlayAgain();
        gameObject.SetActive(false);
    }

}
