using UnityEngine;

namespace PathCreation.Examples {
    // Example of creating a path at runtime from a set of points.

    [RequireComponent(typeof(PathCreator))]
    public class GeneratePathExample : MonoBehaviour
    {
        private const int DISTANCE = 20;

        public Transform[] waypoints;

        private GameObject go;

        private BezierPath bezierPath;

        void Start () {
            bezierPath = new BezierPath (waypoints, false, PathSpace.xz);
            GetComponent<PathCreator> ().bezierPath = bezierPath;
            go = GameObject.FindGameObjectWithTag("Target");
            InvokeRepeating("FastUpdate", 0.01f, 0.01f);
        }

        void FastUpdate()
        {
            if (bezierPath.GetPoint(bezierPath.NumPoints - 1).z - go.transform.position.z < 1) {
                MovePoints();
            }
        }

        void SetPoints()
        {                
            bezierPath.SetPoint(bezierPath.NumPoints - 1, new Vector3(
                bezierPath.GetPoint(bezierPath.NumPoints - 1).x,
                bezierPath.GetPoint(bezierPath.NumPoints - 1).y,
                bezierPath.GetPoint(bezierPath.NumPoints - 1).z + DISTANCE
            ));
        }

        void MovePoints()
        {
            for (int i = 1; i < bezierPath.NumPoints; i++)
            {
                bezierPath.MovePoint(i, new Vector3(
                    bezierPath.GetPoint(i).x,
                    bezierPath.GetPoint(i).y,
                    bezierPath.GetPoint(i).z + DISTANCE
                ));
            }
        }
    }
}