using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    public float unitSquareSize = 10.0f;

    private Camera _cam;
    private FloatingPlatformHalo _halo;

    //Bobbing
    public float bobbingSpeed = 1f;
    public float bobbingAmount = 0.25f;
    private Vector3 bobbingBasePosition;

    private void Awake()
    {
        _cam = Camera.main;
        _halo = GetComponentInChildren<FloatingPlatformHalo>();
        bobbingBasePosition = transform.localPosition;
    }

    private void Update()
    {
        CheckForPlatformControl();

        if (isDragging)
        {
            _halo.IsDragging(true);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            mousePos.x = Mathf.Clamp(mousePos.x, -unitSquareSize / 1, unitSquareSize / 1);
            mousePos.y = Mathf.Clamp(mousePos.y, -unitSquareSize / 3, unitSquareSize / 3);

            transform.position = mousePos + offset;
        }
        else
        {
            _halo.IsDragging(false);
            float yOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, bobbingBasePosition.y + yOffset, transform.localPosition.z);
        }
    }

    private void CheckForPlatformControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D[] results = new RaycastHit2D[10];
            ContactFilter2D filter = new ContactFilter2D();
            int numHits = Physics2D.Raycast(mousePos, Vector3.zero, filter, results);
            if (numHits > 0)
            {
                for (int i = 0; i < numHits; ++i)
                {
                    if (results[i].transform.CompareTag("Platform"))
                    {
                        Debug.Log("Raycast Hit Platform");
                        isDragging = true;
                        break;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            bobbingBasePosition = transform.localPosition;
        }
    }
}