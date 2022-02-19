using UnityEngine;
using UnityEngine.UI;
using Item_Code;
using System.IO;
using System.Threading.Tasks;

public class UI_Inventory : MonoBehaviour
{
    [Header("아이템 창")]
    public RawImage[] Icons;

    private Inventory_Items Inven;

    private void Awake()
    {
        InventoryLoding();
    }


    public void Test()
    {
        
    }

    private async void InventoryLoding()
    {
        await Task.Delay(1000);
        while (true)
        {
            Load_Inventory();
            reload_Inventory();
            await Task.Delay(60000); //60초마다 세이브 로드를 반복
            Save_Inventory();
        }
    }

    private void Load_Inventory()
    {
        string str = File.ReadAllText(Application.dataPath + "/Resources/Inventory.json");
        Inven = JsonUtility.FromJson<Inventory_Items>(str);
    }

    private void reload_Inventory()
    {
        foreach(Inventory_Item item in Inven.Inventory)
        {
            if (item.Item_Code != "")
            {
                Icons[item.Order].texture = GameManager.Instance.Ui_Manage.Find_Item_Img(item.Item_Code);
            }
            else
            {
                Icons[item.Order].texture = GameManager.Instance.Ui_Manage.Black;
            }
        }
    }

    private void Save_Inventory()
    {
        bool b = true;
        for (int i = 0;i< Inven.Inventory.Count;i++)
        {
            if (Inven.Inventory[i].Item_Code != GameManager.Instance.Ui_Manage.Find_Item_Code(Icons[i].texture.name)) // UI의 코드가 저장된 데이터의 코드와 다를경우
            {
                Inven.Inventory[i].Item_Code = GameManager.Instance.Ui_Manage.Find_Item_Code(Icons[Inven.Inventory[i].Order].texture.name);
                b = false;   // 한번이라도 값이 바뀐적이 있다면 false 값이 나온다.
            }
        }

        if (!b)
        {
            Debug.Log("저장중...");
            File.WriteAllText(Application.dataPath + "/Resources/Inventory.json", JsonUtility.ToJson(Inven)); // Json을 현재 상태로 바꾸고 Josn에 저장
        }
    }




}
