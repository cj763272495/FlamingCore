using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

/// <summary>
/// 单滑
/// </summary>
public class SlideScrollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler {

    protected RectTransform contentTrans;
    protected float beginMousePositionX;
    protected float endMousePositionX;
    private ScrollRect scrollRect;
    public GameObject content;

    public int cellLength;
    public int spacing;
    public int leftOffset;
    protected float moveOneItemLength;

    protected Vector3 currentContentLocalPos;//上一次的位置
    protected Vector3 contentInitPos;//Content初始位置
    protected Vector2 contentTransSize;//Content初始大小

    public int totalItemNum;

    public event Action<int> OnIndexChanged;
    private int _currentIndex;
    public int CurrentIndex { 
        protected set {
            if(_currentIndex != value) {
                _currentIndex = value;
                OnIndexChange();
                OnIndexChanged?.Invoke(value);
            }
        } 
        get { return _currentIndex; } }

    public Text pageText;

    public bool needSendMessage;

    public bool changeScale; 
    public float maxScale;
    public float minScale; 

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentTrans = scrollRect.content;
        moveOneItemLength = cellLength + spacing;
        currentContentLocalPos = contentTrans.localPosition;
        contentTransSize = contentTrans.sizeDelta;
        contentInitPos = contentTrans.localPosition;
        _currentIndex = 1;
        if (pageText != null) {
            pageText.text = _currentIndex.ToString() + "/" + totalItemNum;
        } 
    }

    protected virtual void OnIndexChange() {
        if(changeScale) {
            for(int i = 0; i <= totalItemNum - 1; i++) {
                Transform curTrans = content.transform.GetChild(i);
                Image img = curTrans.GetComponent<Image>();
                if(i == _currentIndex - 1) {
                    curTrans.localScale = new Vector3(maxScale,maxScale,maxScale);
                    ChangeImgAlpha(img, 1);
                } else {
                    curTrans.localScale = new Vector3(minScale,minScale,minScale);
                    ChangeImgAlpha(img, 0.5f);
                }
            }
        }
    }

    protected void ChangeImgAlpha(Image img, float a) {
        Color co = img.color;
        co.a = a;
        img.color = co;
    }

    public void Init()
    {
        CurrentIndex = 1;
        if (contentTrans != null)
        {
            contentTrans.localPosition = contentInitPos;
            currentContentLocalPos = contentInitPos;
            if (pageText != null) {
                pageText.text = CurrentIndex.ToString() + "/" + totalItemNum;
            }
        }
    }

    /// <summary>
    /// 通过拖拽与松开来达成翻页效果
    /// </summary>
    /// <param name="eventData"></param> 
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        endMousePositionX = Input.mousePosition.x;
        float offSetX = 0;
        float moveDistance = 0;//当次需要滑动的距离
        offSetX = beginMousePositionX - endMousePositionX;

        if (offSetX>0)//右滑
        {
            if (_currentIndex >=totalItemNum)
            {
                return;
            }
            if (needSendMessage)
            {
                UpdatePanel(true);
            }
            
            moveDistance = -moveOneItemLength;
            CurrentIndex++;
        }
        else//左滑
        {
            if (_currentIndex <=1)
            {
                return;
            }
            if (needSendMessage)
            {
                UpdatePanel(false);
            }
            moveDistance = moveOneItemLength;
            CurrentIndex--;
        }
        if (pageText != null)
        {
            pageText.text = _currentIndex.ToString() + "/" + totalItemNum;
        }
        DOTween.To(
            ()=>contentTrans.localPosition,lerpValue
            =>contentTrans.localPosition=lerpValue,
            currentContentLocalPos + new Vector3(moveDistance,0,0),0.3f).SetEase(Ease.Linear);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    /// <summary>
    /// 按钮来控制翻书效果
    /// </summary> 
    public void ToNextPage()
    {
        float moveDistance = 0;
        if (_currentIndex >=totalItemNum)
        {
            return;
        }

        moveDistance = -moveOneItemLength;
        CurrentIndex++;
        if (pageText!=null)
        {
            pageText.text = _currentIndex.ToString() + "/" + totalItemNum;
        }
        if (needSendMessage)
        {
            UpdatePanel(true);
        }
        DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void ToLastPage()
    {
        float moveDistance = 0;
        if (_currentIndex <=1)
        {
            return;
        }

        moveDistance = moveOneItemLength;
        CurrentIndex--;
        if (pageText != null){
            pageText.text = _currentIndex.ToString() + "/" + totalItemNum;
        }
        if (needSendMessage)
        {
            UpdatePanel(false);
        }
        DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePositionX = Input.mousePosition.x;
    }

    //设置Content的大小
    public void SetContentLength(int itemNum)
    {
        contentTrans.sizeDelta = new Vector2(contentTrans.sizeDelta.x+(cellLength+spacing)*(itemNum-1),contentTrans.sizeDelta.y);
        totalItemNum = itemNum;
    }

    //初始化Content的大小
    public void InitScrollLength()
    {
        contentTrans.sizeDelta = contentTransSize;
    }

    //发送翻页信息的方法
    public void UpdatePanel(bool toNext)
    {
        if (toNext)
        {
            gameObject.SendMessageUpwards("ToNextLevel");
        }
        else
        {
            gameObject.SendMessageUpwards("ToLastLevel");
        }
    }
}
