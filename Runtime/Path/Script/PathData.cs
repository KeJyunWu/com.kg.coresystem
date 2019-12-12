using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PathData : MonoBehaviour {

    [SerializeField]
    bool m_drawPath = false;

    [SerializeField]
    Color m_pathColor = Color.red;

    [SerializeField]
    Color m_crossColor = Color.red;
    [SerializeField]
    float m_crossLength = 1;

    [SerializeField]
    public List<GameObject> m_pathPoint = new List<GameObject>();
    List<Vector3> m_pointPos = new List<Vector3>();

    public Vector3[] GetPath()
    {
        var m_pointPos = from _item in m_pathPoint
                         where _item != null
                         select _item.transform.position;
        return m_pointPos.ToList().ToArray();
    }

    public void AddNewPoint()
    {
        GameObject _point = new GameObject();
        _point.transform.parent = transform;
        _point.transform.position = m_pathPoint.Count == 0 ? Vector3.zero : m_pathPoint[m_pathPoint.Count-1].transform.position;
        _point.transform.name = "Pont";
        _point.hideFlags = HideFlags.HideInHierarchy;
        m_pathPoint.Add(_point);
    }

    public void RemovePoint(int _index)
    {
        GameObject _point = m_pathPoint[_index];
        DestroyImmediate(_point);
        m_pathPoint.RemoveAt(_index);
    }

    private void OnDrawGizmos()
    {
        if (!m_drawPath)
            return;

        if (m_pathPoint.Count != 0)
        {
            bool _b = m_pathPoint.Any(_item => _item == null);
            m_pointPos.Clear();
            if (!_b)
            {
                var m_pointPos = from _item in m_pathPoint
                                 where _item != null
                                 select _item.transform.position;
                List<Vector3> m_point = m_pointPos.ToList();

                Gizmos.color = m_crossColor;
                for (var i=0;i< m_point.Count;i++)
                {
                    Vector3 y = m_point[i];
#if UNITY_EDITOR
                    Handles.Label(m_point[i],"Point : " +i.ToString());
#endif
                    Gizmos.DrawLine(m_point[i] + Vector3.right* m_crossLength, m_point[i] - Vector3.right* m_crossLength);
                    Gizmos.DrawLine(m_point[i] + Vector3.up * m_crossLength, m_point[i] - Vector3.up * m_crossLength);
                    Gizmos.DrawLine(m_point[i] + Vector3.forward * m_crossLength, m_point[i] - Vector3.forward * m_crossLength);
                }

                if (m_point.Count > 1)
                    iTween.DrawPath(m_point.ToArray(), m_pathColor);
            }
            else
                Debug.LogWarning("Path Data Error");
        }
    }
}
