using System.Linq;
using UnityEditor;
using UnityEngine;

public class AttackEditorWindow : EditorWindow
{
    private Vector2 _listScrollPos;
    private Vector2 _inspectorScrollPos;

    private string _searchText = "";
    private AttackData[] _allAttackDatas;
    private AttackData _selectedAttackData;

    private AttackType _selectedFilter = AttackType.All; // 필터 상태

    [MenuItem("Window/AttackData Tool")]
    public static void ShowWindow()
    {
        GetWindow<AttackEditorWindow>("AttackData Tool");
    }

    private void OnEnable()
    {
        LoadAllSkills();
    }

    private void LoadAllSkills()
    {
        string[] guids = AssetDatabase.FindAssets("t:AttackData");
        _allAttackDatas = guids.Select(g => AssetDatabase.LoadAssetAtPath<AttackData>(AssetDatabase.GUIDToAssetPath(g))).ToArray();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        // ===== 스킬 목록 =====
        GUILayout.BeginVertical(GUILayout.Width(300));

        // 상단 버튼
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("새 공격 데이터 만들기", GUILayout.Height(25)))
            CreateNewAttackData();

        if (GUILayout.Button("새로고침", GUILayout.Width(80), GUILayout.Height(25)))
            LoadAllSkills();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // 카테고리 필터
        GUILayout.BeginHorizontal();
        foreach (AttackType filterType in System.Enum.GetValues(typeof(AttackType)))
        {
            if (GUILayout.Toggle(_selectedFilter == filterType, filterType.ToString(), "Button"))
                _selectedFilter = filterType;
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        _searchText = EditorGUILayout.TextField("검색", _searchText);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("공격 데이터 목록", EditorStyles.boldLabel);

        // 스킬 리스트
        _listScrollPos = GUILayout.BeginScrollView(_listScrollPos);
        foreach (var skill in _allAttackDatas.Where(s => IsSkillMatchFilter(s) &&
                                                   (string.IsNullOrEmpty(_searchText) || s.name.ToLower().Contains(_searchText.ToLower()))))
        {
            GUILayout.BeginHorizontal();

            // 선택 버튼
            if (GUILayout.Button(skill.name, (_selectedAttackData == skill) ? EditorStyles.toolbarButton : EditorStyles.miniButton))
            {
                _selectedAttackData = skill;
                EditorGUIUtility.PingObject(skill);
            }

            // 삭제 버튼
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("데이터 삭제", $"해당 ({skill.name}) 데이터를 삭제하시겠습니까?", "삭제", "취소"))
                {
                    string path = AssetDatabase.GetAssetPath(skill);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                    LoadAllSkills();

                    if (_selectedAttackData == skill) _selectedAttackData = null;
                }
            }
            GUI.color = Color.white;

            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        GUILayout.EndVertical();

        // ===== 우측: 인스펙터 =====
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("선택된 공격 데이터", EditorStyles.boldLabel);
        _inspectorScrollPos = GUILayout.BeginScrollView(_inspectorScrollPos);

        if (_selectedAttackData != null)
        {
            Editor editor = Editor.CreateEditor(_selectedAttackData);
            if (editor != null) editor.OnInspectorGUI();
        }
        else
        {
            EditorGUILayout.HelpBox("공격 데이터를 선택하면 여기에서 편집할 수 있습니다.", MessageType.Info);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    private bool IsSkillMatchFilter(AttackData skill)
    {
        if (_selectedFilter == AttackType.All) return true;
        return skill.AttackType == _selectedFilter;
    }

    private void CreateNewAttackData()
    {
        AttackData newSkill = ScriptableObject.CreateInstance<AttackData>();
        string path = EditorUtility.SaveFilePanelInProject("새 공격 데이터 생성", "NewAttackData", "asset", "공격 데이터를 저장할 위치를 선택하세요");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(newSkill, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LoadAllSkills();
        }
    }
}
