using BackEnd;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Post
{
    public bool IsCanReceive = false;
    public string Title;
    public string Content;
    public string InDate;

    public Dictionary<string, int> PostReward = new();

    public override string ToString()
    {
        string result = string.Empty;
        result += $"title : {Title}\n";
        result += $"content : {Content}\n";
        result += $"inDate : {InDate}\n";

        if (IsCanReceive)
        {
            result += "���� ������\n";

            foreach (string itemKey in PostReward.Keys)
            {
                result += $"| {itemKey} : {PostReward[itemKey]}�� \n";
            }
        }
        else
        {
            result += "�������� �ʴ� ���� �������Դϴ�.";
        }
        return result;
    }
    
}
public class BackEndPost
{
    private static BackEndPost s_instance = null;
    public static BackEndPost Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new ();
            }
            return s_instance;
        }
    }
    private List<Post> _postList = new();
    public void SavePostToLocal(LitJson.JsonData item)
    {
        foreach (LitJson.JsonData itemJson in item)
        {
            if (itemJson["item"].ContainsKey("itemType"))
            {
                int itemId = int.Parse(itemJson["item"]["itemId"].ToString());
                string itemType = itemJson["item"]["itemType"].ToString();
                string itemName = itemJson["item"]["itemName"].ToString();
                int itemCount = int.Parse(itemJson["itemCount"].ToString());
                if (BackendGameData.UserData.Inventory.ContainsKey(itemName))
                {
                    BackendGameData.UserData.Inventory[itemName] += itemCount;
                }
                else
                {
                    BackendGameData.UserData.Inventory.Add(itemName, itemCount);
                }
            }
            else
            {
                Debug.LogError("�������� �ʴ� item�Դϴ�.");
            }
        }
    }
    public void PostListGet(PostType postType)
    {
        // Step 3. ���� �ҷ�����
        var bro = Backend.UPost.GetPostList(postType);

        string chartName = "������ ��Ʈ";
        if (bro.IsSuccess() == false)
        {
            Debug.LogError("���� �ҷ����� �� ������ �߻��߽��ϴ�.");
            return;
        }
        if (bro.GetFlattenJSON()["postList"].Count <= 0)
        {
            Debug.LogWarning("���� ������ �������� �ʽ��ϴ�!");
            return;
        }

        foreach (LitJson.JsonData postListJson in bro.GetFlattenJSON()["postList"])
        {
            Post post = new();

            post.Title = postListJson["title"].ToString();
            post.Content = postListJson["content"].ToString();
            post.InDate = postListJson["inDate"].ToString();
            if (postType == PostType.User)
            {
                if (postListJson["itemLocation"]["tableName"].ToString() == "USER_DATA")
                {
                    if (postListJson["itemLocation"]["column"].ToString() == "inventory")
                    {
                        foreach (string itemKey in postListJson["item"].Keys)
                        {
                            post.PostReward.Add(itemKey, int.Parse(postListJson["item"][itemKey].ToString()));
                        }
                    }
                    else
                    {
                        Debug.LogWarning("���� �������� �ʴ� Į�� �����Դϴ�." +
                           postListJson["itemLocation"]["column"].ToString());
                    }
                }
                else
                {
                    Debug.LogWarning("���� �������� �ʴ� ���̺� �����Դϴ�." +
                        postListJson["itemLocation"]["tableName"].ToString());
                }
            }
            else
            {
                foreach (LitJson.JsonData itemJson in postListJson["items"])
                {
                    if (itemJson["chartName"].ToString() == chartName)
                    {
                        string itemName = itemJson["item"]["itemName"].ToString();
                        int itemCount = int.Parse(itemJson["itemCount"].ToString());
                        
                        if (post.PostReward.ContainsKey(itemName))
                        {
                            post.PostReward[itemName] += itemCount;
                        }
                        else
                        {
                            post.PostReward.Add(itemName, itemCount);
                        }
                        post.IsCanReceive = true;
                    }
                    else
                    {

                    }
                }
            }
        }
    }
    public void PostReceive(PostType postType, int index)
    {
        // Step 4. ���� ���� ���� �� �����ϱ�.
    }
    public void PostReceiveAll(PostType postType)
    {
        // Step 5. ���� ��ü ���� �� �����ϱ�.
    }
}