using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar;
    public EnemyAI EA;
    public GameMaster GM;
    public int health;
    public TextMeshProUGUI healthDisplay;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        healthBar.SetMaxHealth(health);
        GM.playerDied = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthDisplay != null)
            healthDisplay.SetText(health + "");
    }

    //...RECREATE THE ENEMY DAMAGE SCRIPT IN HERE FOR PLAYER HEALTH...//

    public void takeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        Debug.Log("Player Hurt");
        if (health <= 0)
        {
            Debug.Log("YOU Got Clapped!!!");
            GM.playerDied = true;
            Invoke(nameof(Dead), 0.07f);
        }
    }

    private void Dead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
            takeDamage(EA.damage);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 10)
            takeDamage(health);
    }
}
