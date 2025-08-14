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
            result += "우편 아이템\n";

            foreach (string itemKey in PostReward.Keys)
            {
                result += $"| {itemKey} : {PostReward[itemKey]}개 \n";
            }
        }
        else
        {
            result += "지원하지 않는 우편 아이템입니다.";
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
                Debug.LogError("지원하지 않는 item입니다.");
            }
        }
    }
    public void PostListGet(PostType postType)
    {
        // Step 3. 우편 불러오기
        var bro = Backend.UPost.GetPostList(postType);

        string chartName = "아이템 차트";
        if (bro.IsSuccess() == false)
        {
            Debug.LogError("우편 불러오기 중 에러가 발생했습니다.");
            return;
        }
        if (bro.GetFlattenJSON()["postList"].Count <= 0)
        {
            Debug.LogWarning("받을 우편이 존재하지 않습니다!");
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
                        Debug.LogWarning("아직 지원되지 않는 칼럼 정보입니다." +
                           postListJson["itemLocation"]["column"].ToString());
                    }
                }
                else
                {
                    Debug.LogWarning("아직 지원되지 않는 테이블 정보입니다." +
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
        // Step 4. 우편 개별 수령 및 저장하기.
    }
    public void PostReceiveAll(PostType postType)
    {
        // Step 5. 우편 전체 수령 및 저장하기.
    }
}