using UnityEngine;

public class ObjectStatus : MonoBehaviour
{
    public bool OnKnockBack = true;
    public bool OnSuperArmor = false;
    public bool IsDie=false;
    public float VectorZ = 0;
    void Start()
    {
        InitializeVectorZ();
    }
    public void InitializeVectorZ()
    {
        VectorZ = 0;
    }
    public void SetVectorZ()
    {
        
    }
    void Update()
    {
        
    }
}
