using BackEnd;
using System.Text;
using UnityEngine;

public class BackEndRanking
{
    private static BackEndRanking s_instance = null;

    public static BackEndRanking Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new BackEndRanking();
            }

            return s_instance;
        }
    }

    public void RankInsert(int score)
    {
        // [���� �ʿ�] '������ UUID ��'�� '�ڳ� �ܼ� > ��ŷ ����'���� ������ ��ŷ�� UUID������ �������ּ���.  
        string rankUUID = "0198a1d5-c81d-73d4-ba93-df3d82f26163"; // ���� : "4088f640-693e-11ed-ad29-ad8f0c3d4c70"

        string tableName = "USER_DATA";
        string rowInDate = string.Empty;

        // ��ŷ�� �����ϱ� ���ؼ��� ���� �����Ϳ��� ����ϴ� �������� inDate���� �ʿ��մϴ�.  
        // ���� �����͸� �ҷ��� ��, �ش� �������� inDate���� �����ϴ� �۾��� �ؾ��մϴ�.  
        Debug.Log("������ ��ȸ�� �õ��մϴ�.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("������ ��ȸ �� ������ �߻��߽��ϴ� : " + bro);
            return;
        }

        Debug.Log("������ ��ȸ�� �����߽��ϴ� : " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�. ������ ������ �õ��մϴ�.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("������ ���� �� ������ �߻��߽��ϴ� : " + bro2);
                return;
            }

            Debug.Log("������ ���Կ� �����߽��ϴ� : " + bro2);

            rowInDate = bro2.GetInDate();
        }

        Debug.Log("�� ���� ������ rowInDate : " + rowInDate); // ����� rowIndate�� ���� ������ �����ϴ�.  

        Param param = new Param();
        param.Add("level", score);

        // ����� rowIndate�� ���� �����Ϳ� param������ ������ �����ϰ� ��ŷ�� �����͸� ������Ʈ�մϴ�.  
        Debug.Log("��ŷ ������ �õ��մϴ�.");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);

        if (rankBro.IsSuccess() == false)
        {
            Debug.LogError("��ŷ ��� �� ������ �߻��߽��ϴ�. : " + rankBro);
            return;
        }

        Debug.Log("��ŷ ���Կ� �����߽��ϴ�. : " + rankBro);
    }

    public void RankGet()
    {
        // Step 3. ��ŷ �ҷ����� ���� �߰�
        string rankUUID = "0198a1d5-c81d-73d4-ba93-df3d82f26163";// ���� : "4088f640-693e-11ed-ad29-ad8f0c3d4c70"
        var bro = Backend.URank.User.GetRankList(rankUUID);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("��ŷ ��ȸ�� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log("��ŷ ��ȸ�� �����߽��ϴ�. : " + bro);

        Debug.Log("�� ��ŷ ��� ���� �� : " + bro.GetFlattenJSON()["totalCount"].ToString());

        foreach (LitJson.JsonData jsonData in bro.FlattenRows())
        {
            StringBuilder info = new StringBuilder();

            info.AppendLine("���� : " + jsonData["rank"].ToString());
            info.AppendLine("�г��� : " + jsonData["nickname"].ToString());
            info.AppendLine("���� : " + jsonData["score"].ToString());
            info.AppendLine("gamerInDate : " + jsonData["gamerInDate"].ToString());
            info.AppendLine("���Ĺ�ȣ : " + jsonData["index"].ToString());
            info.AppendLine();
            Debug.Log(info);
        }
    }
}
