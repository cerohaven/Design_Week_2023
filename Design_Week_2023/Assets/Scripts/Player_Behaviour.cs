using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Behaviour : MonoBehaviour
{
    public Slider monsterTimeSlider; // Reference to the slider in the UI
    public bool playerTransformed = false;

    #region floats
    public float humanHealth = 1f;
    public float monsterHealth = 100f;
    public float transformDelay = 1f;
    public float monsterTime = 10f;
    #endregion

    #region SpriteRender
    private SpriteRenderer spriteRenderer;
    private Color humanColor = Color.blue;
    private Color monsterColor = Color.red;
    private float timeSinceTransform = 0f;
    #endregion

    private bool fillingSlider = false; // Flag for filling the slider
    private bool canTransform = true; // Flag for whether the player can transform or not

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = humanColor;
        monsterTime = monsterTimeSlider.maxValue; // Initialize monsterTime to maxValue
        monsterTimeSlider.value = monsterTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if enough time has passed since last transform
        if (timeSinceTransform < transformDelay)
        {
            timeSinceTransform += Time.deltaTime;
        }

        // Check for 'T' key press to cycle between human and monster forms
        if (Input.GetKeyDown(KeyCode.T) && timeSinceTransform >= transformDelay && canTransform)
        {
            playerTransformed = !playerTransformed;
            if (playerTransformed)
            {
                Debug.Log(spriteRenderer.color + " Player Transformed!");
                spriteRenderer.color = monsterColor;
                humanHealth = monsterHealth;
                fillingSlider = false; // Stop filling the slider if it's not empty
            }
            else
            {
                Debug.Log(spriteRenderer.color + " Player Transformed Back!");
                spriteRenderer.color = humanColor;
                humanHealth = 1f;
                fillingSlider = true; // Start filling the slider
            }

            timeSinceTransform = 0f; // Reset transform delay timer
            StartCoroutine(CountdownMonsterTime());
        }

        // Fill the slider back from 0 to 1 over time while the player is in human form
        if (!playerTransformed && fillingSlider && monsterTime < monsterTimeSlider.maxValue)
        {
            monsterTime += Time.deltaTime * (monsterTimeSlider.maxValue / 10f); // Fill the slider at 10% per second
            monsterTime = Mathf.Clamp(monsterTime, 0f, monsterTimeSlider.maxValue);
            monsterTimeSlider.value = monsterTime;
        }
    }

    IEnumerator CountdownMonsterTime()
    {
        while (monsterTime > 0)
        {
            if (playerTransformed)
            {
                yield return new WaitForSeconds(1f);
                monsterTime -= 1f;
                monsterTimeSlider.value = monsterTime;
            }
            else
            {
                // Reset fillingSlider flag
                fillingSlider = true;
                yield return new WaitForEndOfFrame();
            }
        }

        // Reset to human form and transform delay
        playerTransformed = false;
        spriteRenderer.color = humanColor;
        humanHealth = 1f;
        Debug.Log(spriteRenderer.color + " Player Transformed Back! MonsterTime expired.");
        StartCoroutine(TimeoutTransform());
    }

    IEnumerator TimeoutTransform()
    {
        timeSinceTransform = transformDelay;
        Debug.Log("Cannot transform for 15 seconds.");
        yield return new WaitForSeconds(15);

        Debug.Log("Can transform again.");
        timeSinceTransform = 0f; // Reset transform delay timer
    }

    private void LateUpdate()
    {
        // Set camera position to follow player
        Vector3 newPosition = transform.position;
        newPosition.z = -10;
        Camera.main.transform.position = newPosition;

        // Zoom out if player is transformed
        if (playerTransformed)
        {
            Camera.main.orthographicSize = 7;
        }
        else
        {
            Camera.main.orthographicSize = 5;
        }
    }
}



