using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[CustomEditor(typeof(AttackData))]
public class AttackDataCustomInspector : Editor
{
    private bool _showBasic = true;
    private bool _showCost = true;
    private bool _showEffect = true;
    private bool _showCamera = true;
    #region PropertyField
    private SerializedProperty _attackType; // 스킬타입 대전제
    private SerializedProperty _targetLayer; // 스킬 타겟레이어
    
    // ** 스킬 기본 정보 ** // 
    //- 스킬 이름
    private SerializedProperty _attackName;
     
    //- 공격자 무기타입
    
    private SerializedProperty _weaponType;
    
    //- 스킬 설명
    private SerializedProperty _attackDescription;
    
    //- 스킬 아이콘
    private SerializedProperty _attackIcon;
    
    //- 쿨타임
    private SerializedProperty _coolTime;
    
    //- 데미지
    private SerializedProperty _damage;
    
    //- 히트수
    private SerializedProperty _hitCount;
    
    // 다중, 단일타겟
    private SerializedProperty _attackHitType;
    
    // 공격 속성
    private SerializedProperty _attackElement;
     
     // 스킬 특수 능력
    private SerializedProperty _specialAbility;
   
   // 버프 타입
    private SerializedProperty _setBuffType;
    
    // 디버프 타입
    private SerializedProperty _setDebuffType;
     
     // 스킬 범위
    private SerializedProperty _startAttackPoint;
    private SerializedProperty _attackRange;
    private SerializedProperty _rangeDownOffsetZ;
    private SerializedProperty _rangeUpOffsetZ;
     
    // ----------------------------------------------
    
    // ** 스킬 레벨시스템 디자인 ** //
    
    //- 스킬 레벨 시스템 사용 여부
    private SerializedProperty _usingLevelSystem;
    
    //- 스킬 현재레벨
    private SerializedProperty _attackCurLevel;
    
     //- 스킬 최대레벨
    private SerializedProperty _attackMaxLevel;
   
     //- 스킬 레벨당 데미지 상승치
    private SerializedProperty _attackIncreaseValue;

    // ----------------------------------------------

    // ** 소모코스트  
    private SerializedProperty _hpCost;
    private SerializedProperty _mpCost;

    // ** 스킬 사용중 이동여부
    private SerializedProperty _canMove;

    // ** 슈퍼아머
    private SerializedProperty _onSuperArmor;

    // ** 넉백
    private SerializedProperty _useKnockBack;
    private SerializedProperty _knockBackForce;

    // ** 스킬 캐스팅
    private SerializedProperty _doAttackCasting;
    private SerializedProperty _attackCastingTime;

    // ** 투사체
    private SerializedProperty _projectilePrefabName;
    private SerializedProperty _projectileForce;

    // ** 스킬 연출 ** // 
    
    //- 스킬 시전 사운드
    private SerializedProperty _attackSFX;
    private SerializedProperty _voiceSFX;

    //- 스킬 이펙트
    private SerializedProperty _attackEffectName;
    private SerializedProperty _attackEffectPos;

    //- 타격 이펙트
    private SerializedProperty _hitEffectName;
    private SerializedProperty _hitEffectPos;

    // ----------------------------------------------

    // ** 카메라 연출 ** //

    //- 카메라 연출 사용여부
    private SerializedProperty _useCameraShake;

    //- 카메라 쉐이크 힘
    private SerializedProperty _shakeCameraForce;

    //- 카메라 쉐이크 방향
    private SerializedProperty _shakeCameraDir;

    //- 카메라 쉐이크 유지시간
    private SerializedProperty _shakeCameraDuration;

    // ----------------------------------------------
    #endregion
    private void OnEnable()
    {
        _attackType = serializedObject.FindProperty("AttackType");
        _weaponType = serializedObject.FindProperty("WeaponType");
        _attackDescription = serializedObject.FindProperty("AttackDescription");
        _attackIcon = serializedObject.FindProperty("AttackIcon");
        _targetLayer = serializedObject.FindProperty("TargetLayer");
        _attackName = serializedObject.FindProperty("AttackName");
        _coolTime = serializedObject.FindProperty("CoolTime");
        _damage = serializedObject.FindProperty("Damage");
        _hitCount = serializedObject.FindProperty("HitCount");
        _attackHitType = serializedObject.FindProperty("AttackHitType");
        _attackElement = serializedObject.FindProperty("AttackElement");
        _specialAbility = serializedObject.FindProperty("SpecialAbility");
        _setBuffType = serializedObject.FindProperty("SetBuffType");
        _setDebuffType = serializedObject.FindProperty("SetDebuffType");
        _startAttackPoint = serializedObject.FindProperty("StartAttackPoint");
        _attackRange = serializedObject.FindProperty("AttackRange");
        _rangeDownOffsetZ = serializedObject.FindProperty("RangeDownOffsetZ");
        _rangeUpOffsetZ = serializedObject.FindProperty("RangeUpOffsetZ");

        _usingLevelSystem = serializedObject.FindProperty("UsingLevelSystem");
        _attackCurLevel = serializedObject.FindProperty("AttackCurLevel");
        _attackMaxLevel = serializedObject.FindProperty("AttackMaxLevel");
        _attackIncreaseValue = serializedObject.FindProperty("LevelIncreaseValue");

        _hpCost = serializedObject.FindProperty("Hp");
        _mpCost = serializedObject.FindProperty("Mp");
        _canMove = serializedObject.FindProperty("CanMove");
        _onSuperArmor = serializedObject.FindProperty("SuperArmor");

        _useKnockBack = serializedObject.FindProperty("UseKnockBack");
        _knockBackForce = serializedObject.FindProperty("KnockBackForce");

        _doAttackCasting = serializedObject.FindProperty("AttackCasting");
        _attackCastingTime = serializedObject.FindProperty("CastingTime");

        _projectilePrefabName = serializedObject.FindProperty("ProjectilePrefabName");
        _projectileForce = serializedObject.FindProperty("ProjectileForce");

        _attackSFX = serializedObject.FindProperty("AttackSFX");
        _voiceSFX = serializedObject.FindProperty("VoiceSFX");
        _attackEffectName = serializedObject.FindProperty("AttackEffectPrefabName");
        _attackEffectPos = serializedObject.FindProperty("EffectPos");
        _hitEffectName = serializedObject.FindProperty("HitEffectPrefabName");
        _hitEffectPos = serializedObject.FindProperty("HitEffectPos");

        _useCameraShake = serializedObject.FindProperty("UseCameraShake");
        _shakeCameraForce = serializedObject.FindProperty("ShakePower");
        _shakeCameraDir = serializedObject.FindProperty("ShakeDir");
        _shakeCameraDuration = serializedObject.FindProperty("ShakeDuration");
    }
    public override void OnInspectorGUI()
    {
        if (target == null)
        {
            Debug.LogError("target is null!");
            return;
        }

        AttackData aData = (AttackData)target;
        if (aData == null)
        {
            Debug.LogError("attackData is null!");
            return;
        }
        serializedObject.Update();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(_attackType);
        switch (aData.AttackType)
        {
            //근접 공격 타입 인스펙터
            #region MeleeAttackType
            case AttackType.Melee:
                _showBasic = EditorGUILayout.Foldout(_showBasic, "근접타입 기본 정보", true);
                if (_showBasic)
                {
                    EditorGUILayout.PropertyField(_targetLayer);
                    EditorGUILayout.PropertyField(_weaponType);
                    EditorGUILayout.PropertyField(_attackIcon);
                    EditorGUILayout.PropertyField(_attackName);
                    EditorGUILayout.PropertyField(_attackDescription);
                    EditorGUILayout.PropertyField(_coolTime);
                    EditorGUILayout.PropertyField(_damage);
                    EditorGUILayout.PropertyField(_hitCount);
                    EditorGUILayout.PropertyField(_attackHitType);
                    EditorGUILayout.PropertyField(_attackElement);
                    EditorGUILayout.PropertyField(_specialAbility);
                    if (aData.SpecialAbility == AttackSpecialAbility.Buff)
                    {
                        EditorGUILayout.PropertyField(_setBuffType);
                    }
                    else if (aData.SpecialAbility == AttackSpecialAbility.DeBuff)
                    {
                        EditorGUILayout.PropertyField(_setDebuffType);
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("공격 범위", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_startAttackPoint);
                    EditorGUILayout.PropertyField(_attackRange);
                    EditorGUILayout.PropertyField(_rangeDownOffsetZ);
                    EditorGUILayout.PropertyField(_rangeUpOffsetZ);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_usingLevelSystem);
                    if (aData.UsingLevelSystem)
                    {
                        EditorGUILayout.PropertyField(_attackCurLevel);
                        EditorGUILayout.PropertyField(_attackMaxLevel);
                        EditorGUILayout.LabelField("공격데이터 레벨당 데미지 증가값", EditorStyles.miniBoldLabel);
                        EditorGUILayout.PropertyField(_attackIncreaseValue);
                    }
                }
                EditorGUILayout.Space();
                _showCost = EditorGUILayout.Foldout(_showCost, "코스트 및 부가 효과", true);
                if (_showCost)
                {
                    EditorGUILayout.PropertyField(_hpCost);
                    EditorGUILayout.PropertyField(_mpCost);
                    EditorGUILayout.PropertyField(_canMove);
                    EditorGUILayout.PropertyField(_onSuperArmor);
                    EditorGUILayout.PropertyField(_useKnockBack);
                    if (aData.UseKnockBack)
                    {
                        EditorGUILayout.PropertyField(_knockBackForce);
                    }
                }
                EditorGUILayout.Space();
                _showEffect = EditorGUILayout.Foldout(_showEffect, "스킬 연출", true);
                if (_showEffect)
                {
                    EditorGUILayout.PropertyField(_doAttackCasting);
                    if (aData.AttackCasting)
                    {
                        EditorGUILayout.PropertyField(_attackCastingTime);
                    }

                    EditorGUILayout.PropertyField(_attackEffectName);
                    EditorGUILayout.PropertyField(_attackEffectPos);
                    EditorGUILayout.PropertyField(_hitEffectName);
                    EditorGUILayout.PropertyField(_hitEffectPos);
                    EditorGUILayout.PropertyField(_attackSFX);
                    EditorGUILayout.PropertyField(_voiceSFX);
                    if (_attackSFX != null)
                    {
                        if (GUILayout.Button("효과음 재생"))
                        {
                            if (Application.isPlaying == true)
                            {
                                Debug.LogWarning("플레이모드에선 재생할 수 없습니다.");
                                return;
                            }
                            if (Application.isPlaying == false && aData.AttackSFX == null)
                            {
                                Debug.LogWarning($"attackSFX가 {aData.AttackSFX} 입니다.");
                                return;
                            }
                            OnEditorModePlayClip(aData.AttackSFX);
                            Debug.Log($"효과음 재생 {aData.AttackSFX}");
                        }
                    }
                }

                EditorGUILayout.Space();
                _showCamera = EditorGUILayout.Foldout(_showCamera, "카메라 연출", true);
                if (_showCamera)
                {
                    EditorGUILayout.PropertyField(_useCameraShake);
                    if (aData.UseCameraShake)
                    {
                        EditorGUILayout.PropertyField(_shakeCameraForce);
                        EditorGUILayout.PropertyField(_shakeCameraDir);
                        EditorGUILayout.PropertyField(_shakeCameraDuration);
                    }
                }
                break;

            #endregion 
                
            //원거리 공격 타입 인스펙터
            #region RangeAttackCase
            case AttackType.Range:
                _showBasic = EditorGUILayout.Foldout(_showBasic, "투사체타입 기본 정보", true);
                if (_showBasic)
                {
                    EditorGUILayout.PropertyField(_targetLayer);
                    EditorGUILayout.PropertyField(_weaponType);
                    EditorGUILayout.PropertyField(_attackIcon);
                    EditorGUILayout.PropertyField(_attackName);
                    EditorGUILayout.PropertyField(_attackDescription);
                    EditorGUILayout.PropertyField(_coolTime);
                    EditorGUILayout.PropertyField(_damage);
                    EditorGUILayout.PropertyField(_hitCount);
                    EditorGUILayout.PropertyField(_attackHitType);
                    EditorGUILayout.PropertyField(_attackElement);
                    EditorGUILayout.PropertyField(_specialAbility);
                    if (aData.SpecialAbility == AttackSpecialAbility.Buff)
                    {
                        EditorGUILayout.PropertyField(_setBuffType);
                    }
                    else if (aData.SpecialAbility == AttackSpecialAbility.DeBuff)
                    {
                        EditorGUILayout.PropertyField(_setDebuffType);
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("투사체 프리팹 정보", EditorStyles.miniBoldLabel);
                    EditorGUILayout.PropertyField(_projectilePrefabName);
                    EditorGUILayout.PropertyField(_projectileForce);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("투사체 생성 지점", EditorStyles.miniBoldLabel);
                    EditorGUILayout.PropertyField(_startAttackPoint);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_rangeDownOffsetZ);
                    EditorGUILayout.PropertyField(_rangeUpOffsetZ);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_usingLevelSystem);
                    if (aData.UsingLevelSystem)
                    {
                        EditorGUILayout.PropertyField(_attackCurLevel);
                        EditorGUILayout.PropertyField(_attackMaxLevel);
                        EditorGUILayout.LabelField("스킬 레벨당 데미지 증가값", EditorStyles.miniBoldLabel);
                        EditorGUILayout.PropertyField(_attackIncreaseValue);
                    }
                }
                
                
                EditorGUILayout.Space();
                _showCost = EditorGUILayout.Foldout(_showCost, "코스트 및 부가 효과", true);
                if (_showCost)
                {
                    EditorGUILayout.PropertyField(_hpCost);
                    EditorGUILayout.PropertyField(_mpCost);
                    EditorGUILayout.PropertyField(_onSuperArmor);
                    EditorGUILayout.PropertyField(_useKnockBack);
                    if (aData.UseKnockBack)
                    {
                        EditorGUILayout.PropertyField(_knockBackForce);
                    }
                }

                EditorGUILayout.Space();
                _showEffect = EditorGUILayout.Foldout(_showEffect, "스킬 연출", true);
                if (_showEffect)
                {
                    EditorGUILayout.PropertyField(_doAttackCasting);
                    if (aData.AttackCasting)
                    {
                        EditorGUILayout.PropertyField(_attackCastingTime);
                    }

                    EditorGUILayout.PropertyField(_canMove);
                    EditorGUILayout.PropertyField(_attackEffectName);
                    EditorGUILayout.PropertyField(_attackEffectPos);
                    EditorGUILayout.PropertyField(_hitEffectName);
                    EditorGUILayout.PropertyField(_hitEffectPos);
                    EditorGUILayout.PropertyField(_attackSFX);
                    EditorGUILayout.PropertyField(_voiceSFX);
                    if (_attackSFX != null)
                    {
                        if (GUILayout.Button("효과음 재생"))
                        {
                            if (Application.isPlaying == true)
                            {
                                Debug.LogWarning("플레이모드에선 재생할 수 없습니다.");
                                return;
                            }
                            if (Application.isPlaying == false && aData.AttackSFX == null)
                            {
                                Debug.LogWarning($"Attack SFX가 {aData.AttackSFX} 입니다.");
                                return;
                            }
                            OnEditorModePlayClip(aData.AttackSFX);
                            Debug.Log($"효과음 재생 {aData.AttackSFX}");
                        }
                    }
                }

                EditorGUILayout.Space();
                _showCamera = EditorGUILayout.Foldout(_showCamera, "카메라 연출", true);
                if (_showCamera)
                {
                    EditorGUILayout.PropertyField(_useCameraShake);
                    if (aData.UseCameraShake)
                    {
                        EditorGUILayout.PropertyField(_shakeCameraForce);
                        EditorGUILayout.PropertyField(_shakeCameraDir);
                        EditorGUILayout.PropertyField(_shakeCameraDuration);
                    }
                }
                break;

            #endregion

            //범위 공격 타입 인스펙터
            #region AOEAttackType
            case AttackType.AOE:
                _showBasic = EditorGUILayout.Foldout(_showBasic, "AOE타입 기본 정보", true);
                if (_showBasic)
                {
                    EditorGUILayout.PropertyField(_targetLayer);
                    EditorGUILayout.PropertyField(_weaponType);
                    EditorGUILayout.PropertyField(_attackIcon);
                    EditorGUILayout.PropertyField(_attackName);
                    EditorGUILayout.PropertyField(_attackDescription);
                    EditorGUILayout.PropertyField(_coolTime);
                    EditorGUILayout.PropertyField(_damage);
                    EditorGUILayout.PropertyField(_hitCount);
                    EditorGUILayout.PropertyField(_attackHitType);
                    EditorGUILayout.PropertyField(_attackElement);
                    EditorGUILayout.PropertyField(_specialAbility);
                    if (aData.SpecialAbility == AttackSpecialAbility.Buff)
                    {
                        EditorGUILayout.PropertyField(_setBuffType);
                    }
                    else if (aData.SpecialAbility == AttackSpecialAbility.DeBuff)
                    {
                        EditorGUILayout.PropertyField(_setDebuffType);
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("스킬 범위", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(_startAttackPoint);
                    EditorGUILayout.PropertyField(_attackRange);
                    EditorGUILayout.PropertyField(_rangeDownOffsetZ);
                    EditorGUILayout.PropertyField(_rangeUpOffsetZ);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(_usingLevelSystem);
                    if (aData.UsingLevelSystem)
                    {
                        EditorGUILayout.PropertyField(_attackCurLevel);
                        EditorGUILayout.PropertyField(_attackMaxLevel);
                        EditorGUILayout.LabelField("스킬 레벨당 데미지 증가값", EditorStyles.miniBoldLabel);
                        EditorGUILayout.PropertyField(_attackIncreaseValue);
                    }
                }



                EditorGUILayout.Space();
                _showCost = EditorGUILayout.Foldout(_showCost, "코스트 및 부가 효과", true);
                if (_showCost)
                {
                    EditorGUILayout.PropertyField(_hpCost);
                    EditorGUILayout.PropertyField(_mpCost);
                    EditorGUILayout.PropertyField(_canMove);
                    EditorGUILayout.PropertyField(_onSuperArmor);
                    EditorGUILayout.PropertyField(_useKnockBack);
                    if (aData.UseKnockBack)
                    {
                        EditorGUILayout.PropertyField(_knockBackForce);
                    }
                }
                EditorGUILayout.Space();
                _showEffect = EditorGUILayout.Foldout(_showEffect, "스킬 연출", true);
                if (_showEffect)
                {
                    EditorGUILayout.PropertyField(_doAttackCasting);
                    if (aData.AttackCasting)
                    {
                        EditorGUILayout.PropertyField(_attackCastingTime);
                    }

                    EditorGUILayout.PropertyField(_attackEffectName);
                    EditorGUILayout.PropertyField(_attackEffectPos);
                    EditorGUILayout.PropertyField(_hitEffectName);
                    EditorGUILayout.PropertyField(_hitEffectPos);
                    EditorGUILayout.PropertyField(_attackSFX);
                    EditorGUILayout.PropertyField(_voiceSFX);
                    if (_attackSFX != null)
                    {
                        if (GUILayout.Button("효과음 재생"))
                        {
                            if (Application.isPlaying == true)
                            {
                                Debug.LogWarning("플레이모드에선 재생할 수 없습니다.");
                                return;
                            }
                            if (Application.isPlaying == false && aData.AttackSFX == null)
                            {
                                Debug.LogWarning($"skillSFX가 {aData.AttackSFX} 입니다.");
                                return;
                            }
                            OnEditorModePlayClip(aData.AttackSFX);
                            Debug.Log($"효과음 재생 {aData.AttackSFX}");
                        }
                    }
                }

                EditorGUILayout.Space();
                _showCamera = EditorGUILayout.Foldout(_showCamera, "카메라 연출", true);
                if (_showCamera)
                {
                    EditorGUILayout.PropertyField(_useCameraShake);
                    if (aData.UseCameraShake)
                    {
                        EditorGUILayout.PropertyField(_shakeCameraForce);
                        EditorGUILayout.PropertyField(_shakeCameraDir);
                        EditorGUILayout.PropertyField(_shakeCameraDuration);
                    }
                }
                break;
                #endregion
        }
        serializedObject.ApplyModifiedProperties();
    }

    private static MethodInfo playClipMethod;
    // 에디터모드에서 효과음 재생
    private static void OnEditorModePlayClip(AudioClip clip)
    {
        #if UNITY_EDITOR
        if (clip == null) return;

        if (playClipMethod == null)
        {
            var audioUtil = typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");
            playClipMethod = audioUtil.GetMethod("PlayClip", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(AudioClip) }, null);
        }

        playClipMethod?.Invoke(null, new object[] { clip });
        #endif
    }
}
