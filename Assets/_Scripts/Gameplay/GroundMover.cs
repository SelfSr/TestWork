using UnityEngine;

public class GroundMover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private RectTransform groundPart;

    private float backgroundPosX;
    private bool isMoving;

    public void Move() => isMoving = true;
    public void Stop() => isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            backgroundPosX -= speed * Time.deltaTime;
            backgroundPosX = Mathf.Repeat(backgroundPosX, groundPart.rect.width);
            groundPart.localPosition = new Vector2(backgroundPosX, groundPart.localPosition.y);
        }
    }
}