using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float wayPointGizmoRadios = .5f;

        private void OnDrawGizmos()
        {
            for(int i =0;i <transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWayPoint(i), wayPointGizmoRadios);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(j));


            }
        }

        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if((i+1) == transform.childCount)
            {
                return 0;
            }
            else
            {
                return i+1;
            }
            
        }
    }
}

