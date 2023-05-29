using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Settings")]
    public float bobFrequency;
    public float bobAmount = 0.05f;

    private float timer = 0f;
    private float midpoint = 0f;
    [SerializeField] private Transform playerTransform;
    private CharacterController characterController;

    private void Start()
    {
        playerTransform = transform.parent;
        characterController = playerTransform.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (characterController != null)
        {
            midpoint = (characterController.height) - 0.4f;
        }
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        float speed = characterController.velocity.magnitude;
        
        if (speed > 0 && speed < 5)
        {
            bobFrequency = 20f;
        }
        else if (speed > 5 && speed < 11)
        {
            bobFrequency = 25f;
        }
        else if (speed >= 11)
        {
            bobFrequency = 30f;
        }
        

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            float waveSlice = Mathf.Sin(timer);
            timer += bobFrequency * Time.deltaTime;

            if (timer > Mathf.PI * 2)
                timer -= Mathf.PI * 2;

            if (waveSlice != 0)
            {
                float translateChange = waveSlice * bobAmount;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.1f, 1f);

                translateChange *= totalAxes;
                transform.localPosition = new Vector3(transform.localPosition.x, midpoint + translateChange, transform.localPosition.z);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, midpoint, transform.localPosition.z);
            }
        }
        else
        {
            timer = 0f;
            transform.localPosition = new Vector3(transform.localPosition.x, midpoint, transform.localPosition.z);
        }
    }
}