using UnityEngine;
using UnityEngine.UI;
using Base_Class;
using System.Collections;

public class UI_Inventory : MonoBehaviour
{
    [Header("아이템 창")]
    public RawImage[] Icons;
    private Inventory_Items Inven;

    [Header("소지금")]
    public Text myMoney;


    public void Get_InvenLoding()
    {
        Load_Inventory();
    }

    /// <summary>
    /// 인벤토리 상태를 저장하고 갱신하는 함수
    /// </summary>
    public IEnumerator Inventory_Loding()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            reload_Inventory();
            yield return new WaitForSeconds(1);
            Save_Inventory();
        }
    }

    /// <summary>
    /// 인벤토리 로드
    /// </summary>
    public void Load_Inventory()
    {
        if(User_Info.Instance != null)
            Inven = JsonUtility.FromJson<Inventory_Items>(User_Info.Instance.save.Player_Data["Inven"]);
    }


    /// <summary>
    /// 인벤토리 세이브
    /// </summary>
    public void Save_Inventory()
    {
        bool b = true;
        for (int i = 0; i < Inven.Inventory.Count; i++)
        {
            if (Inven.Inventory[i].Item_Code != GameManager.Instance.Ui_Manager.Find_Item_Code(Icons[i].texture.name)) // UI의 코드가 저장된 데이터의 코드와 다를경우
            {
                Inven.Inventory[i].Item_Code = GameManager.Instance.Ui_Manager.Find_Item_Code(Icons[Inven.Inventory[i].Order].texture.name);
                b = false;   // 한번이라도 값이 바뀐적이 있다면 false 값이 나온다.
            }
        }

        if (int.Parse(myMoney.text) != Inven.Money) // 인벤토리의 돈과 화면에 표시된 돈이 다를 때
            b = false;

        if (!b&& User_Info.Instance != null)
            User_Info.Instance.save.Player_Data["Inven"] = JsonUtility.ToJson(Inven);

    }

    /// <summary>
    /// 인벤토리 상태에 따른 아이콘 갱신
    /// </summary>
    public void reload_Inventory()
    {
        myMoney.text = Inven.Money.ToString();
        foreach (Inventory_Item item in Inven.Inventory)
        {
            if (item.Item_Code != "")
            {
                Icons[item.Order].texture = GameManager.Instance.Ui_Manager.Find_Item_Img(item.Item_Code);
            }
            else
            {
                Icons[item.Order].texture = GameManager.Instance.Ui_Manager.Black_Item;
            }
        }
    }

    public void AddMoney(int a)
    {
        Inven.Money += a;
    }

    public bool RemoveMoney(int a)
    {
        if(Inven.Money >= a)
        {
            Inven.Money -= a;
            return true;
        }
        return false;
    }






}
