using DG.Tweening; 
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionLevelPanel : MonoBehaviour
{
    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI CurrentWaveTxt;
    public GameObject tagGroup;
    public Image pointer;

    public Sprite lockedSprite;
    public Sprite wonSprite;
    public Sprite nextLevelSprite;
    public List <Transform> levelTagTrans = new();

    private void Start() {
        //Reset();
    }

    public void Reset() {
        foreach(var tag in levelTagTrans) {
            tag.GetComponent<Image>().sprite = lockedSprite;
        }
    }

    public Tween TransitionToNextLevel(int curWave, int curLevel,int hp) {
        levelTagTrans.Clear();
        for(int i = 0; i < tagGroup.transform.childCount; i++) {
            levelTagTrans.Add(tagGroup.transform.GetChild(i));
        }

        hpTxt.text = hp.ToString();
        hpTxt.gameObject.SetActive(true);
        CurrentWaveTxt.text = "0" +(curWave+1).ToString();
        CurrentWaveTxt.gameObject.SetActive(true);

        for(int i = 0; i < levelTagTrans.Count; i++) {
            if(i <= curLevel) {
                levelTagTrans[i].GetComponent<Image>().sprite = wonSprite;
            } else if(i == curLevel+1) {
                levelTagTrans[i].GetComponent<Image>().sprite = nextLevelSprite;
            } else {
                levelTagTrans[i].GetComponent<Image>().sprite = lockedSprite;
            }
        }
        pointer.transform.position = new Vector3(levelTagTrans[curLevel].position.x,pointer.transform.position.y,pointer.transform.position.z);
        pointer.gameObject.SetActive(true);
        gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence().SetUpdate(UpdateType.Normal,true);
        seq.AppendInterval(1f);
        seq.AppendCallback( ()=> levelTagTrans[curLevel + 1].GetComponent<Image>().sprite = wonSprite);
        seq.Append(pointer.transform.DOMoveX(levelTagTrans[curLevel + 1].position.x,0.5f).SetUpdate(UpdateType.Normal,true));
        seq.Join(levelTagTrans[curLevel + 1].DOScale(2f,0.3f));
        seq.Append(levelTagTrans[curLevel + 1].DOScale(1.5f,0.3f)); 
         
        return seq.Play();
    }
}
