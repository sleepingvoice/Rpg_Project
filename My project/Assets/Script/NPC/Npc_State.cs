using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Npc
{
    [Serializable]
    public class Npc_State
    {
        public string Obj_name; // 게임 오브젝트의 이름
        public string Char_name; // 캐릭터 이름
        public List<string> Line; // 대사
        public List<string> Button_Function; // 버튼 기능
        public string Prefeb_img_name; // 이미지 프리펩 이름
    }

    [Serializable]
    public class Npc_States
    {
        public List<Npc_State> Npc = new List<Npc_State>();
    }
}
