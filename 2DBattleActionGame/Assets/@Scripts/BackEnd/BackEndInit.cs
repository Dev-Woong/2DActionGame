using UnityEngine;
using BackEnd;
public class BackEndInit : MonoBehaviour
{
    void Start()
    {
        var bro = Backend.Initialize(); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻�
        }
        Test();
    }
    void Test()
    {
        //BackEndLogin.Instance.CustomSignUp("testID","testPW");
        BackEndLogin.Instance.CustomLogin("testID", "testPW");
        //BackEndLogin.Instance.UpdateNickname("�;���");
        //BackendGameData.Instance.GameDataInsert();

        //BackendGameData.Instance.GameDataGet(); // ������ ���� �Լ�

        //// [�߰�] ������ �ҷ��� �����Ͱ� �������� ���� ���, �����͸� ���� �����Ͽ� ����
        //if (BackendGameData.userData == null)
        //{
        //    BackendGameData.Instance.GameDataInsert();
        //}

        //BackendGameData.Instance.LevelUp(); // [�߰�] ���ÿ� ����� �����͸� ����

        //BackendGameData.Instance.GameDataUpdate(); //[�߰�] ������ ����� �����͸� �����(����� �κи�)

        //BackEndRanking.Instance.RankInsert(100);
        //BackEndRanking.Instance.RankGet();
        BackEndChart.Instance.ChartGet("198951");
    }
}
