using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public HomeWnd homeWnd;
    public void OpenShopPanel() {
        homeWnd.mainShow.SetActive(false);
        gameObject.SetActive(true);
    }

    public void CloseShopPanel() {
        homeWnd.mainShow.SetActive(false);
        gameObject.SetActive(true);
    }
}
