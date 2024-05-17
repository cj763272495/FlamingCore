using UnityEngine;
using UnityEngine.UI;

public class ToggleBtn : MonoBehaviour
{
    public GameObject imgBarSelect;
    public SoundPlayer soundPlayer;
    public GameObject bg;
    public Toggle tg;
    public Text txt;

    public HomeWnd HomeWnd;
    private Toggle toggle;
    public PageType pageType;

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
        if (isOn) {
            //soundPlayer.clipSource = Resources.Load<AudioClip>(Constants.ButtonClip);
            //soundPlayer.PlayOneShot();
            imgBarSelect.SetActive(true);
            bg.SetActive(true);
            ChangeTextAlpha(txt, 1f);
            HomeWnd.ChangePage(pageType);
        } else {
            imgBarSelect.SetActive(false);
            bg.SetActive(false);
            ChangeTextAlpha(txt, 0.5f);
        }
    }
}
