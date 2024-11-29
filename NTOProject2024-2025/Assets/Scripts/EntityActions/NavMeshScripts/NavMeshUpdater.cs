using System;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
public class NavMeshUpdater : MonoBehaviour
{    
    static bool s_NeedsNavMeshUpdate;
    NavMeshSurface m_Surface;

    public static void RequestNavMeshUpdate()
    {
        s_NeedsNavMeshUpdate = true;
    }

    void Start()
    {
        m_Surface = GetComponent<NavMeshSurface>();
        m_Surface.BuildNavMesh();
    }

    void Update()
    {
        if (s_NeedsNavMeshUpdate)
        {
            m_Surface.UpdateNavMesh(m_Surface.navMeshData);
            s_NeedsNavMeshUpdate = false;
        }
    }
}
