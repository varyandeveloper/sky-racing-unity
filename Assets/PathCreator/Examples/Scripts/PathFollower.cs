using UnityEngine;
using System.Collections;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float rotationSpeed = 10;
        public float distanceTravelled;

        void Start() {
            if (pathCreator != null)
            {
                UseRandomSpeed();
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        public void UseRandomSpeed()
        {
              speed = Random.Range(10, 15);
        }

        public float GetDistanceUntilLastPoint(float followerX) {
            return pathCreator.bezierPath.GetPoint(pathCreator.bezierPath.NumPoints - 1).x - followerX;
        }

        public void UsePath(PathCreator pathCreator) {
            this.pathCreator = pathCreator;
        }

        public IEnumerator TakePathForTime(float time) {
            PathCreator current = pathCreator;
            pathCreator = null;
            yield return new WaitForSeconds(time);
            pathCreator = current;
        }

        public float GetDistanceTravelled() {
            return distanceTravelled;
        }

        public void UpdateDistanceTravelled() {
            distanceTravelled += speed * Time.deltaTime;
        }

        void Update()
        {
            if (pathCreator != null)
                {
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.rotation = Quaternion.Slerp(
                        GetComponentInParent<Transform>().rotation,
                        Quaternion.LookRotation(pathCreator.path.GetDirection(GetDistanceTravelled() / pathCreator.path.length)),
                        rotationSpeed
                    );
                } else {
                    Vector3 possition = transform.position;
                    possition.z += speed * Time.deltaTime;
                    transform.position = possition;
                }
                UpdateDistanceTravelled();
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}