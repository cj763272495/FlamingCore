using DG.Tweening; 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class LevelScrowView : SlideScrollView{

    protected override void OnIndexChange() {
        if(ResSvc.Instance) {
            for(int i = 0; i <= totalItemNum - 1; i++) {
                Transform curTrans = content.transform.GetChild(i);
                Image img = curTrans.GetComponent<Image>();
                Vector3 targetScale;
                if(i <= PlayersDataSystem.Instance.GetMaxUnLockWave() - 1) {
                    if(i == CurrentIndex - 1) {
                        targetScale = maxScale;
                        img.sprite = ResSvc.Instance.LoadSprite("Sprite/bg_stage_selected");
                        ChangeImgAlpha(img,1);
                    } else {
                        targetScale = minScale;
                        img.sprite = ResSvc.Instance.LoadSprite("Sprite/bg_stage_passed");
                        ChangeImgAlpha(img,0.5f);
                    }
                    curTrans.DOScale(targetScale,0.2f).SetRelative(false);
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

            if(PlayersDataSystem.Instance.GetMaxUnLockWave() > CurrentIndex) {
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
            currentContentLocalPos + new Vector3(moveDistance,0,0),0.1f).SetEase(Ease.Linear).SetRelative(false);
        currentContentLocalPos += new Vector3(moveDistance,0,0);
    }

}
