using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class Movement : NetworkBehaviour
{
    public bool flyhack = false;
    public float speed;
    public float jumpForce;
    public float downPush = 3;

    public float WaterPush = 2;
    public float BallWaterPush = 4;

    public float maxSpeed;

    [HideInInspector] public Rigidbody R;


    public gamemanager GM;
    public GameObject CAMERA;
    public Downpush DownPush;
    public TextMesh TM;

    public string Area = "spawn";

    float StartingMaxSpeed;

    int balljumpphase = 0;
    [SerializeField] bool Grounded = false;
    [SerializeField] bool InWater = false;

    Vector3 StartingPos;
    NetworkIdentity NI;

    [HideInInspector] public int SPIKEDDISKMOBHITS = 0;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<gamemanager>();
        CAMERA = GameObject.Find("Rotational");
        transform.position = new Vector3(13.11443f, 165.4611f, -11.11584f);
        NI = GetComponent<NetworkIdentity>();
        R = GetComponent<Rigidbody>();
        StartingMaxSpeed = maxSpeed;

        StartingPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!NI.hasAuthority || !GM.Started) return;
        if (flyhack)
        {
            R.isKinematic = true;
        }
        else
        {
            R.isKinematic = false;
            if (Grounded)
            {
                if (GM.race == 0 && Input.GetKey(KeyCode.Space) && balljumpphase == 0)
                {
                    R.velocity += new Vector3(0, jumpForce, 0);
                    balljumpphase = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    R.velocity += new Vector3(0, jumpForce, 0);
                }

            }

            if (!Grounded && balljumpphase == 1)
            {
                balljumpphase = 0;
            }

            if (!Grounded && !InWater && GM.race == 0)
            {
                maxSpeed = StartingMaxSpeed + 10;
            }
            else
            {
                maxSpeed = StartingMaxSpeed;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                transform.position = StartingPos;
                R.velocity = new Vector3(0, 0, 0);
                R.angularVelocity = new Vector3(0, 0, 0);
            }
        }
    }
    void FixedUpdate()
    {
        if (flyhack)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position += new Vector3(0, jumpForce, 0);
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += CAMERA.transform.forward * speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += -CAMERA.transform.right * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += -CAMERA.transform.forward * speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += CAMERA.transform.right * speed;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                transform.position += new Vector3(0, -jumpForce, 0);
            }
        }
        else
        {
            Vector3 Clamped = Vector3.ClampMagnitude(R.velocity, maxSpeed);
            R.velocity = new Vector3(Clamped.x, R.velocity.y, Clamped.z);
            if (InWater)
            {
                if (GM.race == 0)
                {
                    R.velocity += new Vector3(0, BallWaterPush, 0);
                }
                else
                {
                    R.velocity += new Vector3(0, WaterPush, 0);
                }
            }
            if (DownPush.Push)
            {
                R.velocity += new Vector3(0, -downPush, 0);
            }
            if (Input.GetKey(KeyCode.W))
            {
                R.velocity += CAMERA.transform.forward * speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                R.velocity += -CAMERA.transform.right * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                R.velocity += -CAMERA.transform.forward * speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                R.velocity += CAMERA.transform.right * speed;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Grounded = true;
        if (collision.gameObject.name.Contains("SpikedDiskMOB")) {
            SPIKEDDISKMOBHITS += 1;
        }

        if (collision.gameObject.name == "ToiletPaperLazarrrr")
        {
            GM.health -= 420;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        Grounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        Grounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Water") {
            InWater = true;
        }

        if (other.gameObject.name == "SpawnHITBOX-NoPVP-YesMobs") {
            Area = "spawn";
        }
        // ToiletPaperLazarrrr
        if (other.gameObject.name == "ToiletPaperHitbox")
        {
            GM.health -= 42;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Water")
        {
            InWater = true;
        }

        if (other.gameObject.name == "SpawnHITBOX-NoPVP-YesMobs")
        {
            Area = "spawn";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Water")
        {
            InWater = false;
        }

        if (other.gameObject.name == "SpawnHITBOX-NoPVP-YesMobs")
        {
            Area = "other";
        }
    } 

}
