/// <summary>
/// Mover missile.
/// Looking for a Target by TargetTag and moving to it
/// </summary>

using UnityEngine;
using System.Collections;


public class MoverMissile : MonoBehaviour
{
    public GameObject target;
    public string TargetTag;
    public float damping = 3;
    public float Speed = 500;
    public float SpeedMax = 1000;
    public float SpeedMult = 1;
    public Vector3 Noise = new Vector3(20, 20, 20);
    public int distanceLock = 70;
    public int DurationLock = 40;
    public float targetlockdirection = 0.5f;
    public bool Seeker;
    public float LifeTime = 5.0f;
	
	private int timetorock;
	private bool locked;
    

    private void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void Update()
    {
		// Find a closed target and follow
        if (Seeker)
        {
            if (timetorock > DurationLock)
            {
                if (!locked && !target)
                {
                    float distance = int.MaxValue;
                    if (GameObject.FindGameObjectsWithTag(TargetTag).Length > 0)
                    {
                        var objs = GameObject.FindGameObjectsWithTag(TargetTag);
						foreach(var obj in objs)
                        {
                            if(obj)
                            {
                                float dis = Vector3.Distance(obj.transform.position,gameObject.transform.position);
                                if(distanceLock > dis)
                                {
                                    if(distance > dis)
                                    {
                                        distance = dis;
                                        target = obj;
                                    }
                                    locked = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                timetorock += 1;
            }

            if (target)
            {
                damping += 0.9f;
                Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*damping);
                Vector3 dir = (target.transform.position - transform.position).normalized;
                float direction = Vector3.Dot(dir, transform.forward);
                if (direction < targetlockdirection)
                {
                    target = null;
                }
            }
            else
            {
                locked = false;
            }
        }
		
		Speed	+= SpeedMult;
		if(Speed > SpeedMax)
			Speed	= SpeedMax;

		GetComponent<Rigidbody>().velocity	= Speed * Time.deltaTime * gameObject.transform.forward;
        GetComponent<Rigidbody>().velocity	+= new Vector3(Random.Range(-Noise.x, Noise.x),Random.Range(-Noise.y, Noise.y),
                                          Random.Range(-Noise.z, Noise.z));

    }
}
