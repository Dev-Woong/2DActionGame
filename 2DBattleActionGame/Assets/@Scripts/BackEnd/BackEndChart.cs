using BackEnd;
using System.Text;
using UnityEngine;

public class BackEndChart
{
    private static BackEndChart s_instance;
    public static BackEndChart Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new();
            }
            return s_instance;  
        }
    }
    public void ChartGet(string chartId)
    {
        // Step 3. ��Ʈ ���� �������� ���� �߰�
        Debug.Log($"{chartId}�� ��Ʈ �ҷ����⸦ ��û�մϴ�.");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}�� ��Ʈ�� �ҷ����� ��, ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log("��Ʈ �ҷ����⿡ �����߽��ϴ�. : " + bro);
        foreach (LitJson.JsonData gameData in bro.FlattenRows())
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("itemID : " + int.Parse(gameData["itemId"].ToString()));
            content.AppendLine("itemName : " + gameData["itemName"].ToString());
            content.AppendLine("itemType : " + gameData["itemType"].ToString());
            content.AppendLine("itemID : " + long.Parse(gameData["itemPower"].ToString()));
            content.AppendLine("itemInfo : " + gameData["itemInfo"].ToString());

            Debug.Log(content.ToString());
        }
    }
}
