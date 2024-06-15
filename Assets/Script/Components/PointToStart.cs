using UnityEngine;
using TMPro;
using DG.Tweening;

public class PointToStart : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start() {
        text.color = new Color(text.color.r,text.color.g,text.color.b,1);
        text.DOFade(0,1).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
     
}
