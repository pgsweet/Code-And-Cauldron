using UnityEngine;
using UnityEngine.SceneManagement;

public class Title_script : MonoBehaviour
{
    public Sprite[] images;
    public SpriteRenderer spriteRenderer;
    private int currentImageIndex = 0;
    private float timer = 0f;
    private float interval = .7f; // .7 seconds

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    } 

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            currentImageIndex = (currentImageIndex + 1) % images.Length;
            spriteRenderer.sprite = images[currentImageIndex];
        }

        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Main_Game");
        }
    }
}