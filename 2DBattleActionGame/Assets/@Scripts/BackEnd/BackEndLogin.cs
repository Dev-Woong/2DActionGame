using UnityEngine;
using System;
using BackEnd;
using Unity.VisualScripting;
public class BackEndLogin 
{
    private static BackEndLogin s_instance = null;

    public static BackEndLogin Instance
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

    public void CustomSignUp(string id, string pw)
    {
        // Step 2. ȸ������ �����ϱ� ����
        Debug.Log("ȸ�������� ��û�մϴ�.");

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("ȸ�����Կ� �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("ȸ�����Կ� �����߽��ϴ�. : " + bro);
        }
    }

    public void CustomLogin(string id, string pw)
    {
        // Step 3. �α��� �����ϱ� ����
        Debug.Log("�α����� ��û�մϴ�.");

        var bro = Backend.BMember.CustomLogin(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("�α����� �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("�α����� �����߽��ϴ�. : " + bro);
        }
    }

    public void UpdateNickname(string nickname)
    {
        // Step 4. �г��� ���� �����ϱ� ����
        Debug.Log("�г��� ������ ��û�մϴ�.");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess())
        {
            Debug.Log("�г��� ���濡 �����߽��ϴ� : " + bro);
        }
        else
        {
            Debug.LogError("�г��� ���濡 �����߽��ϴ� : " + bro);
        }
    }
}
