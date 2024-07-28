using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private float inputVertical;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("qqqq");
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            GetComponent<Rigidbody>().useGravity = false; // 중력 해제
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            GetComponent<Rigidbody>().useGravity = true; // 중력 적용
        }
    }

    private void Update()
    {
        if (isClimbing)
        {
            inputVertical = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * inputVertical * climbSpeed * Time.deltaTime);
        }
    }
}
