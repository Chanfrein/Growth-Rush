using UnityEngine;

namespace Control.AIControl
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                const float waypointGizmosRadius = 0.5f;

                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmosRadius);

                Vector3 lineStart = GetWaypoint(i);
                Vector3 lineEnd = GetWaypoint(GetNextIndex(i));

                Gizmos.DrawLine(lineStart, lineEnd);
            }

        }

        public int GetNextIndex(int i)
        {
            return i + 1 >= transform.childCount ? 0 : i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
