using UnityEngine;
using BackEnd;
public class BackEndInit : MonoBehaviour
{
    void Start()
    {
        var bro = Backend.Initialize(); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }
        Test();
    }
    void Test()
    {
        //BackEndLogin.Instance.CustomSignUp("testID","testPW");
        BackEndLogin.Instance.CustomLogin("testID", "testPW");
        //BackEndLogin.Instance.UpdateNickname("맹엉뚱");
        //BackendGameData.Instance.GameDataInsert();

        //BackendGameData.Instance.GameDataGet(); // 데이터 삽입 함수

        //// [추가] 서버에 불러온 데이터가 존재하지 않을 경우, 데이터를 새로 생성하여 삽입
        //if (BackendGameData.userData == null)
        //{
        //    BackendGameData.Instance.GameDataInsert();
        //}

        //BackendGameData.Instance.LevelUp(); // [추가] 로컬에 저장된 데이터를 변경

        //BackendGameData.Instance.GameDataUpdate(); //[추가] 서버에 저장된 데이터를 덮어쓰기(변경된 부분만)

        //BackEndRanking.Instance.RankInsert(100);
        //BackEndRanking.Instance.RankGet();
        BackEndChart.Instance.ChartGet("198951");
    }
}
