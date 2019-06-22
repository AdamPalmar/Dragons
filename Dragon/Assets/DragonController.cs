using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{

    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float flySpeed = 5f;

    public bool manualControll = true;
    public float yaw = 0.0f;
    public float pitch = 0.0f;
    // Start is called before the first frame update
    public ParticleSystem flame;

    Rigidbody rb;
        

    // Explosion 
    public float explosionForce = 50f;
    public float explosionRadius = 5f;
    private float timer;
    public GameObject flameGameobject;

    void Start()
    {
        flame.Stop();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manualControll)
        {
            ManualControl();
        }
        else
        {
            // if altitude low fly upwards
            AIControl();
        }
    }
    void AIControl()
    {

     
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        // select target
        timer += Time.deltaTime;
        if (soldiers.Length > 0)
        {
            if (timer >= 1f)
            {

                transform.LookAt(soldiers[0].transform);
                if (Vector3.Distance(transform.position, soldiers[0].transform.position) > 10f)
                {
                    rb.velocity = transform.forward * flySpeed;
                }
                else
                {
                    rb.velocity = new Vector3(0, 0, 0);
                }
                timer = 0;
            }

            AIShotFlame(soldiers[0]);
        }
        
  
    }


    void ManualControl()
    {
        Movement();

        if (Input.GetMouseButton(0))
        {
            FireRay();
            flame.Play();
        }
        if (Input.GetMouseButtonUp(0))
        {
            flame.Stop();
        }
    }

    void AIShotFlame(GameObject soldiers)
    {
        if (Vector3.Distance(transform.position, soldiers.transform.position) < 30f)
        {
            FireRay();
            flame.Play();
        }
        else
        {
            flame.Stop();
        }

    }

    void Rotate()
    {
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    void Movement()
    {
        Rotate();
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * flySpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = -transform.forward * flySpeed;
        }

        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }


    void Explosion()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                Collider[] colliders = Physics.OverlapSphere(hit.point, explosionRadius);

                foreach (Collider collider in colliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosionForce, hit.point, explosionRadius);

                    }

                    Destructible dest = collider.GetComponent<Destructible>();
                    if (dest != null)
                    {
                        dest.Destroy();
                    }
                }
            }
        }

    }

    void FireRay()
    {
        RaycastHit hit;

  
        Ray ray = new Ray(flameGameobject.transform.position, flameGameobject.transform.forward);

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if (hit.collider.tag == "environment" || hit.collider.tag == "Soldier")
            {
                Debug.Log("Collision with environment");
                Collider[] colliders = Physics.OverlapSphere(hit.point, explosionRadius);

                GameObject cubehit = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), hit.point, Quaternion.identity);
                Destroy(cubehit, 0.2f);
                foreach (Collider collider in colliders)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosionForce, hit.point, explosionRadius);
                    }

                    Destructible dest = collider.GetComponent<Destructible>();
                    if (dest != null)
                    {
                        dest.Destroy();
                    }
                }
            }
        }
        


    }
}
