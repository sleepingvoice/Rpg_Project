using UnityEngine;
using UnityEngine.UI;
using Item_Code;
using System.IO;
using System.Threading.Tasks;

public class UI_Inventory : MonoBehaviour
{
    [Header("아이템 창")]
    public RawImage[] Icons;

    [Header("빈칸 이미지")]
    public Texture Black;

    private Inventory_Items Inven;
    private Database Big_Data;

    private void Awake()
    {
        Check_Code();
        InventoryLoding();
    }


    public void Test()
    {
        
    }

    private async void InventoryLoding()
    {
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
                Icons[item.Order].texture = Find_Item_Img(item.Item_Code);
            }
            else
            {
                Icons[item.Order].texture = Black;
            }
        }
    }

    private void Save_Inventory()
    {
        bool b = true;
        for (int i = 0;i< Inven.Inventory.Count;i++)
        {
            if (Inven.Inventory[i].Item_Code != Find_Item_Code(Icons[i].texture.name)) // UI의 코드가 저장된 데이터의 코드와 다를경우
            {
                Inven.Inventory[i].Item_Code = Find_Item_Code(Icons[Inven.Inventory[i].Order].texture.name);
                b = false;   // 한번이라도 값이 바뀐적이 있다면 false 값이 나온다.
            }
        }

        if (!b)
        {
            Debug.Log("저장중...");
            File.WriteAllText(Application.dataPath + "/Resources/Inventory.json", JsonUtility.ToJson(Inven)); // Json을 현재 상태로 바꾸고 Josn에 저장
        }
    }


    private void Check_Code()
    {
        string str = File.ReadAllText(Application.dataPath + "/Resources/Item_Code.json");
        Big_Data = JsonUtility.FromJson<Database>(str);
    }

    /// <summary>
    /// 아이템 코드를 활용하여 아이템의 Data를 불러오는 함수
    /// </summary>
    public Data FindItem(string Code)
    {
        foreach (Data data in Big_Data.DataArray)
        {
            if (data.Item_Code == Code)
            {
                return data;
            }
        }

        Debug.LogError("없는 아이템 코드입니다.");
        return null;
    }

    /// <summary>
    /// 아이템 코드를 활용하여 텍스쳐를 불러오는 함수
    /// </summary>
    public Texture Find_Item_Img(string Code)
    {
        Data myItem = FindItem(Code);
        string Route = Find_Item_Route(myItem.Item_Code);
        Route += myItem.PrefabName;
        return Resources.Load<Texture>("Icon" + Route);
    }

    /// <summary>
    /// 아이템 코드로 텍스쳐 경로 찾는 함수
    /// </summary>
    private string Find_Item_Route(string Name)
    {
        string s = "";
        switch (Name[0])
        {
            case 'W':
                s += "/Weapon";
                break;
            case 'A':
                s += "/Ammor";
                break;
            case 'M':
                s += "/Material/";
                return s;
            case 'H':
                s += "/Postion/";
                return s;
        }
        switch (Name[2])
        {
            case 'S':
                s += "/Sword/";
                return s;
            case 'A':
                s += "/Axe/";
                return s;
            case 'D':
                s += "/Dagger/";
                return s;
            case 'M':
                s += "/Mace/";
                return s;
            case 'B':
                s += "/Body/";
                return s;
            case 'F':
                s += "/Foot/";
                return s;
            case 'G':
                s += "/Glove/";
                return s;
            case 'H':
                s += "/Head/";
                return s;
        }
        Debug.LogError("경로를 찾지 못했습니다.");
        return null;
    }


    /// <summary>
    /// 텍스쳐 이름으로 아이템 코드 찾는 함수
    /// </summary>
    private string Find_Item_Code(string Name)
    {
        if(Name == Black.name)
        {
            return "";
        }
        foreach (Data data in Big_Data.DataArray)
        {
            if(Name == data.PrefabName)
            {
                return data.Item_Code;
            }
        }

        Debug.LogError("찾은 이름의 프리펩이 존재하지 않습니다.");
        return null;
    }

}
