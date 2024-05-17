using DG.Tweening; 
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public HomeWnd homeWnd;
    public PausePanel pausePanel;
    public DeadPanel deadPanel;
    public WinPanel winPanel;
    public SetPanel setPanel;
    public StartPanel startPanel;
    public RegLoginWnd regLogInWnd;
    public LoadingWnd loadingWnd;
    public BattleWnd battleWnd;

    public Image blendImg;
    public CanvasGroup canvasGroup;
    public static UIManager Instance { get; private set; }

    public GameObject ImgEnergyDecrease;

    private void Awake() {
        Instance = this;
        canvasGroup = blendImg.GetComponent<CanvasGroup>();
    }

    public void SetBgAuidoOn(bool isOn) {
        setPanel.SetBgAudio(isOn);
    }
    public void ShowJoyStick(bool isOn) {
        setPanel.SetJoyStick(isOn);
    }

    public Tween ShowBlend() {
        return ToolClass.ShowBlendImg(blendImg);
    }

    public Tween FadeIn() {
        canvasGroup.gameObject.SetActive(true);
        blendImg.raycastTarget = true;
        canvasGroup.alpha = 0;
        return  canvasGroup.DOFade(1,0.5f).SetUpdate(UpdateType.Normal,true);
    }

    public Tween FadeOut() {
        canvasGroup.alpha = 1;
        return  canvasGroup.DOFade(0,1f).SetUpdate(UpdateType.Normal,true).OnComplete(() => {
            blendImg.raycastTarget = false;
        });
    }

    public void ShowImgEnergyDecrease() {
        ToolClass.MoveUpAndFadeOut(ImgEnergyDecrease,0,2);
    }

}
