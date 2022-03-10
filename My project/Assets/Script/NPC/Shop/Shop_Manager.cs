using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npc;
using System.IO;
using UnityEngine.UI;


public class Shop_Manager : MonoBehaviour
{
    [Header("상점창")]
    public GameObject Grid;
    public GameObject[] Shop_Window;
    [Header("기타")]
    public GameObject Shop;
    public GameObject Error_Msg;

    private ShopList nowShop;

    private void Awake()
    {
        Load_Shop();
    }

    public void Load_Shop()
    {
        string s = File.ReadAllText(Application.streamingAssetsPath + "/Json/Npc_Shop.json");
        nowShop = JsonUtility.FromJson<ShopList>(s);
    }

    #region 주요함수

    /// <summary>
    /// 상점창을 클릭했을때 초기화시켜주는 함수
    /// </summary>
    public void Reload_shop(string s)
    {
        Shop my_shop = null;
        Reset_Window();

        foreach (Shop Tmp in nowShop.List) // 이름과 맞는 상점json이 존재하는지 찾아본다.
        {
            if (Tmp.Npc_Name == s)
            {
                my_shop = Tmp;
                break;
            }
        }

        if (my_shop == null) // 만약 json 내용이 존재한다면
            return;

        for(int i=0;i<my_shop.myShop.Count;i++)
        {
            Shop_Item Item = my_shop.myShop[i];
            Shop_Menu WindowMenu = Shop_Window[i].GetComponent<Shop_Menu>();
            WindowMenu.Icon.texture = GameManager.Instance.Ui_Manager.Find_Item_Img(Item.item_code);
            WindowMenu.Name.text = GameManager.Instance.Ui_Manager.FindItem(Item.item_code).name;
            WindowMenu.Price.text = Item.Price.ToString();
            WindowMenu.Btk.onClick.AddListener(() => BuyItem(Item.Price, Item.item_code));
            WindowMenu.gameObject.SetActive(true);
        }

        Shop.SetActive(true); // 상점창을 킨다
        GameManager.Instance.Ui_Manager.Manager_Inven.gameObject.SetActive(true); // 인벤토리창을 킨다
    }

    #endregion

    #region 보조함수

    /// <summary>
    /// 상점창의 창들을 꺼주는 함수
    /// </summary>
    private void Reset_Window()
    {
        for(int i=0;i< Shop_Window.Length;i++)
        {
            Shop_Window[i].SetActive(false);
            Shop_Window[i].GetComponent<Shop_Menu>().Btk.onClick.RemoveAllListeners();
        }
    }

    /// <summary>
    /// 구입했을때 가격을 지불하고 아이템이 인벤토리에 들어오는 함수
    /// </summary>
    private void BuyItem(int price, string item_Code)
    {
        if (GameManager.Instance.Ui_Manager.Manager_Inven.RemoveMoney(price))
        {
            foreach (RawImage raw in GameManager.Instance.Ui_Manager.Manager_Inven.Icons)
            {
                if (raw.texture == GameManager.Instance.Ui_Manager.Black_Item)
                {
                    raw.texture = GameManager.Instance.Ui_Manager.Find_Item_Img(item_Code);
                    return;
                }
            }
        }
        else
        {
            Error_Msg.SetActive(true);
        }
    }

    #endregion
}
