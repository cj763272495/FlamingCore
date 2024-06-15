using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinPanel:MonoBehaviour {
    public TextMeshProUGUI curLevelTxt;
    public TextMeshProUGUI gotCoinTxt;
     
    public Image breathImage; 
    public Chest chest;
    public Image curWaveBg; 
    public GameObject chestGroup;  
    public GameObject tagsGroup; 
    public CanvasGroup rewardGroup;

    private Sequence sequence;
    public AudioClip Hit2;

    private Vector3 tagsOrgScale = new Vector3(0.5f,0.5f,1);

    public void Init() {
        //ResetUI();
        gameObject.SetActive(true);
        sequence = DOTween.Sequence().SetUpdate(UpdateType.Normal,true);
        sequence.Append(curWaveBg.transform.DOScale(1,1f).OnComplete(() => {
            curLevelTxt.gameObject.SetActive(true);
        }));
        sequence.Append(curLevelTxt.DOFade(0,0.1f).SetLoops(8,LoopType.Yoyo).OnComplete(() => {
            curLevelTxt.color = new Color(curLevelTxt.color.r,curLevelTxt.color.g,curLevelTxt.color.b,1);
        }));
        Vector3 tagsMaxScale = new Vector3(2f,2f,2f);
        foreach(Transform child in tagsGroup.transform) {
            child.gameObject.SetActive(false);
            sequence.AppendCallback(() => child.gameObject.SetActive(true));
            sequence.AppendCallback(() => AudioManager.Instance.PlaySound(Hit2));
            sequence.Append(child.DOScale(tagsMaxScale,0.05f));
            sequence.Append(child.DOScale(Vector3.one,0.1f));
        }
        sequence.AppendCallback(() => {
            breathImage.gameObject.SetActive(true);
            ToolClass.BreathingImg(breathImage,2);
        });
        sequence.Pause();
        sequence.SetAutoKill(false); 
    }

    public void ResetUI() {
        tagsGroup.SetActive(true);
        curWaveBg.gameObject.SetActive(true);
        breathImage.gameObject.SetActive(false);
        rewardGroup.alpha = 0;
        chestGroup.SetActive(false);
        curLevelTxt.gameObject.SetActive(false);
        curWaveBg.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        foreach(Transform child in tagsGroup.transform) {
            child.gameObject.SetActive(false);
            child.localScale = tagsOrgScale;
        }
    }

    public void SetWinPanelLevelTxt(string wave) {
        curLevelTxt.text = wave;
    }

    public void ShowClearance() {
        gameObject.SetActive(true);
        ResetUI();
        sequence.Restart();
    }

    public void OpenWinPanel(int coinNum) {
        Time.timeScale = 1;
        UIManager.Instance.FadeIn().onComplete+=()=> {
            UIManager.Instance.FadeOut();
            tagsGroup.SetActive(false);
            rewardGroup.alpha = 1;
            chestGroup.SetActive(true); 
            gotCoinTxt.text = "+ " + coinNum.ToString();
            chest.OpenChest();
            UIManager.Instance.ShowPlayerAssets();
        };
    }

    public void Show1stCoinTxt() {
        gotCoinTxt.gameObject.SetActive(true);
        ToolClass.MoveUpAndFadeOut(gotCoinTxt.gameObject,2f,1.5f);
    }

    public void ClickReturnHomeBtn() {
        GameRoot.Instance.EnterMainCity(LeaveWinPanel);
        BattleSys.Instance.battleMgr.DestoryBattle();
    }

    public void ClickNextWaveBtn() {
        if(BattleSys.Instance.battleMgr.StratNextWave()) {
            LeaveWinPanel();
            UIManager.Instance.battleWnd.transitionLevelPanel.Reset();
        }
    }

    public void ClickAgainBtn() {
        LeaveWinPanel();
        BattleSys.Instance.battleMgr.PlayAgain();
    }

    private void LeaveWinPanel() {
        gameObject.SetActive(false);
        StopAllCoroutines();
        chest.Exit();
        chestGroup.SetActive(false);
    }
}
