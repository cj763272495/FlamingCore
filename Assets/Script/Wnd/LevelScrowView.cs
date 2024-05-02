using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class LevelScrowView : SlideScrollView{

    protected override void Update() {
        if(ResSvc.Instance) {
            for(int i = 0; i <= totalItemNum - 1; i++) {
                Transform curTrans = content.transform.GetChild(i);
                Image img = curTrans.GetComponent<Image>();

                if(i <= GameRoot.Instance.PlayerData.max_unLock_wave - 1) {
                    if(i == CurrentIndex - 1) {
                        curTrans.localScale = new Vector3(maxScale,maxScale,maxScale);
                        img.sprite = ResSvc.Instance.LoadSprite("Sprite/bg_stage_selected");
                        ChangeImgAlpha(img,1);
                    } else {
                        curTrans.localScale = new Vector3(minScale,minScale,minScale);
                        img.sprite = ResSvc.Instance.LoadSprite("Sprite/bg_stage_passed");
                        ChangeImgAlpha(img,0.5f);
                    }
                } else {
                    img.sprite = ResSvc.Instance.LoadSprite("Sprite/bg_stage_locked");
                } 
            }
        } 
    }

    public override void OnEndDrag(PointerEventData eventData) {
        endMousePositionX = Input.mousePosition.x;
        float offSetX = 0;
        float moveDistance = 0;//当次需要滑动的距离
        offSetX = beginMousePositionX - endMousePositionX;

        if(offSetX > 0)//右滑
        {
            if(CurrentIndex >= totalItemNum) {
                return;
            }
            if(needSendMessage) {
                UpdatePanel(true);
            }

            if(GameRoot.Instance.PlayerData.max_unLock_wave > CurrentIndex) {
                moveDistance = -moveOneItemLength;
                CurrentIndex++;
            }
        } else//左滑
          {
            if(CurrentIndex <= 1) {
                return;
            }
            if(needSendMessage) {
                UpdatePanel(false);
            }
            moveDistance = moveOneItemLength;
            CurrentIndex--;
        }
        if(pageText != null) {
            pageText.text = CurrentIndex.ToString() + "/" + totalItemNum;
        }
        DOTween.To(
            () => contentTrans.localPosition,lerpValue
            => contentTrans.localPosition = lerpValue,
            currentContentLocalPos + new Vector3(moveDistance,0,0),0.3f).SetEase(Ease.Linear);
        currentContentLocalPos += new Vector3(moveDistance,0,0);
    }

}
