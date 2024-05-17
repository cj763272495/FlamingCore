using DG.Tweening; 
using UnityEngine; 
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;

public static class ToolClass {
    public static void SetGameObjectPosXZ(GameObject player,Vector3 pos) {
        player.transform.position = new Vector3(pos.x,player.transform.position.y,pos.z);
    }

    public static void MoveUpAndFadeOut(GameObject obj,float delay,float duration) {
        var children = obj.GetComponentsInChildren<Transform>();
        Vector3 startPos = obj.transform.position;

        DOVirtual.DelayedCall(delay,() => {
            foreach(var child in children) {
                var img = child.GetComponent<Image>();
                var txt = child.GetComponent<TextMeshProUGUI>();
                if(img != null) {
                    img.DOFade(0,duration).OnComplete(() =>
                    img.color = new Color(img.color.r,img.color.g,img.color.b,1));
                }
                if(txt != null) {
                    txt.DOFade(0,duration).OnComplete(() =>
                    txt.color = new Color(txt.color.r,txt.color.g,txt.color.b,1));
                }
            }
            obj.transform.DOMove(startPos + new Vector3(0,5,0),duration).OnComplete(() => {
                obj.transform.position = startPos;
                obj.SetActive(false);
            });
        });
    }
    public static Tween ShowBlendImg(Image img) {
        //DOTween.Kill(img); // 停止img的所有动画
        img.gameObject.SetActive(true);
        img.color = new Color(img.color.r,img.color.g,img.color.b,0);

        // 使用DoTween将透明度在1秒内变为1，然后在1秒内变回0
        Tween tween = img.DOFade(1,0.8f).OnComplete(() => {
            img.DOFade(0,1f).OnComplete(() => {
                img.gameObject.SetActive(false);
            });
        });

        return tween;
    }

    public static Tween ChangeCameraFov(Camera camera,float targetFov,float duration) {
        return camera.DOFieldOfView(targetFov,duration).SetUpdate(UpdateType.Normal,true); 
    }

    public static async void CallAfterDelay(float delay,Action function) {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        function();
    }
}
