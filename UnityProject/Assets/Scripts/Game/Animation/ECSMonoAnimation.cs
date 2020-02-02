using UnityEngine;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

public class ECSMonoAnimation : MonoBehaviour
{
    public static ECSMonoAnimation Instance { get; private set; }
    
    private Dictionary<int, Animator> m_AnimatorDictionary = new Dictionary<int, Animator>();
    private Dictionary<int, Transform> m_TransformDictionary = new Dictionary<int, Transform>();

    private void Awake()
    {
        Instance = this;
    }

    public int AddAnimator(Animator animator)
    {
        int instanceId = animator.GetInstanceID();
        m_AnimatorDictionary.Add(instanceId, animator);
        return instanceId;
    }

    public void PlayAnimation(int id, int animation)
    {
        m_AnimatorDictionary[id].Play(animation);
    }

    public int AddTransform(Transform t)
    {
        int instanceId = t.GetInstanceID();
        m_TransformDictionary.Add(instanceId, t);
        return instanceId;
    }

    public void SyncTransform(int id, float3 position)//, float3 scale)
    {
        m_TransformDictionary[id].position = position;
        //m_TransformDictionary[id].localScale = scale;
    }
}
