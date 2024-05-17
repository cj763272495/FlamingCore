using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinPanel:MonoBehaviour {
    public TextMeshProUGUI curLevelTxt;
    public TextMeshProUGUI firstCoinTxt;

    public float breathRate = 2.0f;
    public Image breathImage;
    public Image breathImageFlash;
    public Chest chest;
    public Image curWaveBg;
    public RawImage chestShow;
    public GameObject tongGuangTxt;

    public GameObject LeftTopCoinObj;
    public GameObject btnGroup;
    public GameObject tagsGroup;
    public ParticleSystem ps;
    public AudioClip Hit2;
    public Image blendImg;

    public void SetWinPanelLevelTxt(string wave) {
        curLevelTxt.text = wave;
    }

    public void ShowClearance() {
        gameObject.SetActive(true);
        curWaveBg.gameObject.SetActive(true);

        breathImage.color = new Color(breathImage.color.r,breathImage.color.g,breathImage.color.b,0); 
        breathImage.gameObject.SetActive(false);

        chestShow.gameObject.SetActive(false);
        tongGuangTxt.SetActive(false);
        LeftTopCoinObj.SetActive(false);
        curLevelTxt.gameObject.SetActive(false);
        btnGroup.gameObject.SetActive(false);
        curWaveBg.transform.localScale = new Vector3(0.1f,0.1f,0.1f);

        Sequence sequence = DOTween.Sequence().SetUpdate(UpdateType.Normal,true);
        sequence.Append(curWaveBg.transform.DOScale(1,1f).OnComplete(() => {
            curLevelTxt.gameObject.SetActive(true);
            sequence.Join(curLevelTxt.DOFade(0,0.1f).SetLoops(8,LoopType.Yoyo).OnComplete(() => {
                curLevelTxt.color = new Color(curLevelTxt.color.r,curLevelTxt.color.g,curLevelTxt.color.b,1);
            }));
        }));
        tagsGroup.SetActive(true);
        foreach(Transform child in tagsGroup.transform) {
            child.gameObject.SetActive(false);
            sequence.AppendCallback(() => child.gameObject.SetActive(true));
            sequence.Append(child.DOScale(new Vector3(2f,2f,2f),0.05f));
            sequence.Append(child.DOScale(Vector3.one,0.1f));
            sequence.AppendCallback(() => AudioManager.Instance.PlaySound(Hit2));
        }

        sequence.AppendCallback(() => {
            breathImage.gameObject.SetActive(true); 
            BreathingImg(breathImage);
        }); 
        sequence.Play();
    }

    public void OpenWinPanel(int coinNum) {
            //ToolClass.ShowBlendImg(blendImg).onComplete += () => {
            UIManager.Instance.FadeIn().onComplete+=()=> {
                tagsGroup.SetActive(false);
                ps.Play();
                tongGuangTxt.SetActive(true);
                chestShow.gameObject.SetActive(true);
                btnGroup.gameObject.SetActive(true);
                firstCoinTxt.text = "+ " + coinNum.ToString();
                chest.OpenChest();
                UIManager.Instance.FadeOut();
            }; 
        //};
    }

    public void Show1stCoinTxt() {
        firstCoinTxt.gameObject.SetActive(true);
        ToolClass.MoveUpAndFadeOut(firstCoinTxt.gameObject,2f,1.5f);
    }

    private void BreathingImg(Image img) { 
        breathImage.color = new Color(breathImage.color.r,breathImage.color.g,breathImage.color.b,1);
        img.DOFade(0.3f,1).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutSine);
    }


    public void SecndShowCoinTxt() {
        LeftTopCoinObj.SetActive(true);
        int coinNum = (int)BattleSys.Instance.battleMgr.GetCoin();
        var txt = LeftTopCoinObj.GetComponentInChildren<TextMeshProUGUI>();
        txt.text = "+ 0";

        DOTween.To(() => 0,x => txt.text = "+ " + x,coinNum,1f).SetEase(Ease.OutQuint);
        ToolClass.MoveUpAndFadeOut(LeftTopCoinObj,3,1);
    }

    public void ClickReturnHomeBtn() {
        LeaveWinPanel();
        GameRoot.Instance.EnterMainCity();
        BattleSys.Instance.battleMgr.DestoryBattle();
    }

    public void ClickNextWaveBtn() {
        LeaveWinPanel();
        BattleSys.Instance.battleMgr.StratNextWave();
        UIManager.Instance.battleWnd.transitionLevelPanel.Reset();
    }

    public void ClickAgainBtn() {
        LeaveWinPanel();
        BattleSys.Instance.battleMgr.PlayAgain();
    }

    private void LeaveWinPanel() {
        gameObject.SetActive(false);
        DOTween.KillAll();
        StopAllCoroutines();
        chest.Exit();
    }
}
