using UnityEngine;

namespace Supertactic.Mukani
{
    public class FlockUnit : MonoBehaviour
    {
        private FlockSpawner flockSpawner;

        //private bool isMoving = false;
        //private bool isTurning = false;

        private Vector3 waypoint;
        private Vector3 lastWaypoint = Vector3.zero;

        private Animator anim;
        private float speed;

        private Collider col;
        private RaycastHit hit;

        private bool hasTarget;
        private GameObject rayOrigin;
        private float scaleFactor = 1.5f;

        // Start is called before the first frame update
        private void Start()
        {
            // Get the FlockSpawner from its parent
            flockSpawner = transform.parent.GetComponentInParent<FlockSpawner>();
            anim = GetComponentInChildren<Animator>();
            //SetupNPC();
        }

        private void Update()
        {
            if (!hasTarget)
            {
                hasTarget = CanFindTarget(0.5f, 2f);
            }
            else
            {
                // Make sure we rotate the NPC to face its waypoint
                RotateNPC(waypoint, speed);
                // Move the NPC in a straight line toward the waypoint
                transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);

                // Check if collided - if yes then lose the target and look for new waypoint
                CollidedNPC();
            }

            // If NPC reaches waypoint reset target
            if (transform.position == waypoint)
            {
                hasTarget = false;
            }
        }

        // Method for changing direction if NPC collided with something
        private void CollidedNPC()
        {
            if (rayOrigin == null)
            {
                return;
            }

            rayOrigin = GameObject.Find("RayOrigin");

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin.transform.position, rayOrigin.transform.forward, out hit, transform.localScale.z))
            {
                // If collider has hit a waypoint or registers itself ignore raycast hit
                if (hit.collider == col | hit.collider.CompareTag("Waypoint"))
                {
                    return;
                }

                //otherwise have a random chance that NPC will change direction
                int randomNum = UnityEngine.Random.Range(1, 100);
                if (randomNum < 40)
                {
                    hasTarget = false;
                }

                // Debug just to show that it works
                //     Debug.Log(hit.collider.transform.parent.name + " " + hit.collider.transform.parent.position);
            }

            Debug.DrawRay(rayOrigin.transform.position, rayOrigin.transform.forward * 2f, Color.yellow);
        }

        // Get the waypoint
        private Vector3 GetWaypoint(bool isRandom)
        {
            // If isRandom is true than get a random position location
            if (isRandom)
            {
                return flockSpawner.RandomPosition();
            }
            else
            {
                return flockSpawner.RandomWaypoint();
            }
        }

        private bool CanFindTarget(float start = 1f, float end = 7f)
        {
            waypoint = flockSpawner.RandomWaypoint();

            // Make sure we dont set the same waypoint
            if (lastWaypoint == waypoint)
            {
                // Get a new waypoint
                waypoint = GetWaypoint(true);
                return false;
            }
            else
            {
                // Set the new waypoint as the last waypoint
                lastWaypoint = waypoint;

                // Get random speed for movement and animation for a short period
                speed = UnityEngine.Random.Range(start, end);
                anim.speed = speed;

                // Return true to say we found a waypoint
                return true;
            }
        }

        // Method for rotate the NPC to face new waypoints
        private void RotateNPC(Vector3 waypoint, float speed)
        {
            // Get random speed up for the turn
            float turnSpeed = speed * UnityEngine.Random.Range(1f, 5f);

            // Get new direction to look at the target
            Vector3 lookAt = waypoint - this.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt), turnSpeed * Time.deltaTime);
        }

        public void SetNPCScale(float scaleFactor)
        {
            this.scaleFactor = scaleFactor;
        }

        public void SetupNPC()
        {
            // Randomly scale each NPC
            float scale = Random.Range(0f, 4f);
            transform.localScale += new Vector3(scale * scaleFactor, scale, scale);

            if (transform.GetComponent<Collider>() != null && transform.GetComponent<Collider>().enabled == true)
            {
                col = transform.GetComponent<Collider>();
            }
            else if (transform.GetComponentInChildren<Collider>() != null && transform.GetComponentInChildren<Collider>().enabled == true)
            {
                col = transform.GetComponentInChildren<Collider>();
            }
        }
    }
}
