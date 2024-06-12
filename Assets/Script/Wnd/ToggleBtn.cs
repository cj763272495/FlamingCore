using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ToggleBtn : MonoBehaviour
{
    public GameObject imgBarSelect;
    public SoundPlayer soundPlayer;
    public GameObject bg;
    public Toggle tg;
    public Text txt;

    public HomeWnd HomeWnd;
    private Toggle toggle;
    public Image targetImg;
    public Sprite inactiveSprite;
    public Sprite activeSprite;
    public PageType pageType;
    private bool On;

    public void ChangeTextAlpha(Text textComponent, float alpha) {
        Color textColor = textComponent.color;
        textColor.a = alpha; // …Ë÷√Alpha÷µ
        textComponent.color = textColor;
    }

    private void Awake() {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn) {
        On = isOn;
        if (On) {
            bg.SetActive(true);
            ChangeTextAlpha(txt, 1f);
            targetImg.sprite = activeSprite;
            HomeWnd.ChangePage(pageType);
        } else {
            bg.SetActive(false);
            ChangeTextAlpha(txt, 0.5f);
            targetImg.sprite = inactiveSprite;
        }
        imgBarSelect.transform.DOMoveX(transform.position.x, 0.1f);
    }
}
