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

    // �����͸� ������ϱ� ���� �Լ��Դϴ�.(Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new ();
        result.AppendLine($"level : {Level}");
        result.AppendLine($"atk : {Atk}");
        result.AppendLine($"info : {Info}");

        result.AppendLine($"inventory");
        foreach (var itemKey in Inventory.Keys)
        {
            result.AppendLine($"| {itemKey} : {Inventory[itemKey]}��");
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

        Debug.Log("�����͸� �ʱ�ȭ�մϴ�.");
        UserData.Level = 1;
        UserData.Atk = 3.5f;
        UserData.Info = "ģ�ߴ� ������ ȯ���Դϴ�.";

        UserData.Equipment.Add("������ ����");
        UserData.Equipment.Add("��ö ����");
        UserData.Equipment.Add("�츣�޽��� ��ȭ");

        UserData.Inventory.Add("��������", 1);
        UserData.Inventory.Add("�Ͼ�����", 1);
        UserData.Inventory.Add("�Ķ�����", 1);

        Debug.Log("�ڳ� ������Ʈ ��Ͽ� �ش� �����͵��� �߰��մϴ�.");
        Param param = new Param();
        param.Add("level", UserData.Level);
        param.Add("atk", UserData.Atk);
        param.Add("info", UserData.Info);
        param.Add("equipment", UserData.Equipment);
        param.Add("inventory", UserData.Inventory);


        Debug.Log("���� ���� ������ ������ ��û�մϴ�.");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ������ ���Կ� �����߽��ϴ�. : " + bro);

            //������ ���� ������ �������Դϴ�.  
            GameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("���� ���� ������ ���Կ� �����߽��ϴ�. : " + bro);
        }
    }

    public void GameDataGet()
    {
        Debug.Log("���� ���� ��ȸ �Լ��� ȣ���մϴ�.");

        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());

        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json���� ���ϵ� �����͸� �޾ƿɴϴ�.  

            // �޾ƿ� �������� ������ 0�̶�� �����Ͱ� �������� �ʴ� ���Դϴ�.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            }
            else
            {
                GameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //�ҷ��� ���� ������ �������Դϴ�.  

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
            Debug.LogError("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
        }// Step 3. ���� ���� �ҷ����� �����ϱ�
    }

    public void LevelUp()
    {
        // Step 4. ���� ���� ���� �����ϱ�
        Debug.Log("������ 1 ������ŵ�ϴ�.");
        UserData.Level += 1;
        UserData.Atk += 3.5f;
        UserData.Info = "������ �����մϴ�.";
    }

    public void GameDataUpdate()
    {
        // Step 4. ���� ���� ���� �����ϱ�
        if (UserData == null)
        {
            Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Insert Ȥ�� Get�� ���� �����͸� �������ּ���.");
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
            Debug.Log("�� ���� �ֽ� ���� ���� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{GameDataRowInDate}�� ���� ���� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.UpdateV2("USER_DATA", GameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess())
        {
            Debug.Log("���� ���� ������ ������ �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("���� ���� ������ ������ �����߽��ϴ�. : " + bro);
        }
    }
}
