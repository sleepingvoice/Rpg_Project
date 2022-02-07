using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject Monster_Bar;
    public GameObject Bar_Hp;
    public GameObject Bar_Mp;
    public GameObject Bar_Exp;

    private void Update()
    {
        Bar_Update();
    }

    private void Bar_Update()
    {
        Bar_ValueChange(Bar_Hp, "Hp");
        Bar_ValueChange(Bar_Mp, "Mp");
        Bar_ValueChange(Bar_Exp, "Exp");
        Monster();
    }

    private void Bar_ValueChange(GameObject Bar,string s)
    {
        Obj_State Player_state = GameManager.Instance.Player.GetComponent<Obj_State>();
        float max = Player_state.Health_Map["Max" + s];
        float now = Player_state.Health_Map[s];
        Bar.GetComponent<UI_BarContent>().Slider.value = now / max;
        Bar.GetComponent<UI_BarContent>().value.text = "<b>" + now + " / " + max + "</b>";
    }

    private void Monster()
    {
        GameObject Monster= GameManager.Instance.Player.GetComponent<Char_Attack>().Monster;
        if (Monster == null)
        {
            Monster_Bar.SetActive(false);
            return;
        }
        Obj_State Monster_state = Monster.GetComponent<Obj_State>();
        foreach (KeyValuePair<string, float> pair in Monster_state.Health_Map)
        {
            Debug.Log(Monster.name + "/" + pair.Value);
        }
        float max = Monster_state.Health_Map["MaxHp"];
        float now = Monster_state.Health_Map["Hp"];
        Debug.Log(Monster_state.Health_Map["Hp"]);
        Monster_Bar.GetComponent<UI_BarContent>().Slider.value = now / max;
        Monster_Bar.GetComponent<UI_BarContent>().value.text = "<b>" + now + " / " + max + "</b>";
        Monster_Bar.GetComponent<UI_BarContent>().name.text = Monster_state.Name;
        if (now > 0)
            Monster_Bar.SetActive(true);
        else
            Monster_Bar.SetActive(false);
    }
}
