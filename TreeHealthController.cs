using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TreeHealthController : MonoBehaviour
{
    public int maxHealth = 200;
    private int currentHealth;
    public Slider healthBarSlider;
    public bool treeRemoved = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(int health)
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
        maxHealth = health;

    }

    // Method to decrease tree health
    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Tree was hit");
        currentHealth -= damageAmount;
        healthBarSlider.value = currentHealth;

        // Check if the tree's health has reached zero
        if (currentHealth <= 0)
        {
            MakeObjectsInvisible();
            SceneReset();
        }
    }

    // Method to handle tree destruction
    private void MakeObjectsInvisible()
    {
        // Disable the renderers of the children objects
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        treeRemoved = true;
    }

     public void SceneReset()
    {
        StartCoroutine(DelayedReset());
    }

    private IEnumerator DelayedReset()
    {
        Debug.Log("Scene is resetting");

        yield return new WaitForSeconds(6f);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Debug.Log("Scene was reset");
    }
}

