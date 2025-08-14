using BackEnd;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class UserData
{
    public int Level = 1;
    public float Atk = 3.5f;
    public string Info = string.Empty;
    public Dictionary<string, int> Inventory = new ();
    public List<string> Equipment = new ();

    // 데이터를 디버깅하기 위한 함수입니다.(Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new ();
        result.AppendLine($"level : {Level}");
        result.AppendLine($"atk : {Atk}");
        result.AppendLine($"info : {Info}");

        result.AppendLine($"inventory");
        foreach (var itemKey in Inventory.Keys)
        {
            result.AppendLine($"| {itemKey} : {Inventory[itemKey]}개");
        }

        result.AppendLine($"equipment");
        foreach (var equip in Equipment)
        {
            result.AppendLine($"| {equip}");
        }

        return result.ToString();
    }
}

public class BackendGameData
{
    private static BackendGameData s_instance = null;

    public static BackendGameData Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new BackendGameData();
            }

            return s_instance;
        }
    }

    public static UserData UserData;

    private string GameDataRowInDate = string.Empty;

    public void GameDataInsert()
    {
        if (UserData == null)
        {
            UserData = new UserData();
        }

        Debug.Log("데이터를 초기화합니다.");
        UserData.Level = 1;
        UserData.Atk = 3.5f;
        UserData.Info = "친추는 언제나 환영입니다.";

        UserData.Equipment.Add("전사의 투구");
        UserData.Equipment.Add("강철 갑옷");
        UserData.Equipment.Add("헤르메스의 군화");

        UserData.Inventory.Add("빨간포션", 1);
        UserData.Inventory.Add("하얀포션", 1);
        UserData.Inventory.Add("파란포션", 1);

        Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
        Param param = new Param();
        param.Add("level", UserData.Level);
        param.Add("atk", UserData.Atk);
        param.Add("info", UserData.Info);
        param.Add("equipment", UserData.Equipment);
        param.Add("inventory", UserData.Inventory);


        Debug.Log("게임 정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임 정보의 고유값입니다.  
            GameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임 정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }

    public void GameDataGet()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");

        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                GameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임 정보의 고유값입니다.  

                UserData = new UserData();

                UserData.Level = int.Parse(gameDataJson[0]["level"].ToString());
                UserData.Atk = float.Parse(gameDataJson[0]["atk"].ToString());
                UserData.Info = gameDataJson[0]["info"].ToString();

                foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
                {
                    UserData.Inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                }

                foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
                {
                    UserData.Equipment.Add(equip.ToString());
                }

                Debug.Log(UserData.ToString());
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }// Step 3. 게임 정보 불러오기 구현하기
    }

    public void LevelUp()
    {
        // Step 4. 게임 정보 수정 구현하기
        Debug.Log("레벨을 1 증가시킵니다.");
        UserData.Level += 1;
        UserData.Atk += 3.5f;
        UserData.Info = "내용을 변경합니다.";
    }

    public void GameDataUpdate()
    {
        // Step 4. 게임 정보 수정 구현하기
        if (UserData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param();
        param.Add("level", UserData.Level);
        param.Add("atk", UserData.Atk);
        param.Add("info", UserData.Info);
        param.Add("equipment", UserData.Equipment);
        param.Add("inventory", UserData.Inventory);

        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(GameDataRowInDate))
        {
            Debug.Log("내 제일 최신 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{GameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2("USER_DATA", GameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임 정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }
}
