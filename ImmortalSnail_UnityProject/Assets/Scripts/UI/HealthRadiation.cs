using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


//Player Health, Energy Pickups, Projectile Shoot
public class HealthRadiation : MonoBehaviour
{
    //[Header("-Player References-")]
    //public Animator anim;

    //[Header("-Audio-")]
    //public AudioSource playerDamage;
    //public AudioSource healingPadSound;
    //public AudioSource deathSound;

    [Header("-Health Points-")]
    public float healthTotal = 100;
    public TextMeshProUGUI healthUI;
    string healthText;
    public bool inHealingPad = false;

    [Header("-Radiation Points-")]
    public float radiationTotal = 0;
    public TextMeshProUGUI radiationUI;
    string radiationText;
    public float radiationStage = 0;

    [Header("-Stations-")]
    public int numStationsOn = 0;

    void Start()
    {
        //this.anim.SetBool("Dead", false);

        UpdateHealthUI();
        UpdateRadiationUI();
        
        StartCoroutine(RadiationMeter());
        StartCoroutine(HealthMeter());
    }

    void Update()
    {
        if (healthTotal <= 0 || radiationTotal >= 100) //KILLS PLAYER IF HEALTH = 0 OR RADIATION = 100
        {
            UpdateHealthUI();
            UpdateRadiationUI();
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealingPad")) //ENTER HEALING PAD
        {
            //healingPadSound.Play();
            Debug.Log("Healing Pad entered");
            inHealingPad = true;
        }

        if (other.gameObject.CompareTag("SnailHitbox") && healthTotal > 0) //HIT BY SNAIL, SUBTRACTS HEALTH
        {
            Debug.Log("Hit by snail");
            //this.anim.SetTrigger("Damage");
            //playerDamage.Play();
            healthTotal -= 10;
            UpdateHealthUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HealingPad")) //EXIT HEALING PAD
        {
            Debug.Log("Healing Pad exited");
            inHealingPad = false;
        }
    }

    private void UpdateHealthUI() //REFRESHES HEALTH COUNTER UI
    {
        healthText = healthTotal.ToString();
        healthUI.SetText("Health: " + healthText);
    }

    private void UpdateRadiationUI() //REFRESHES RADIATION COUNTER UI
    {
        radiationText = radiationTotal.ToString();
        radiationUI.SetText("Radiation: " + radiationText);
    }

    private void Die()
    {
        healthTotal = 0;
        //this.anim.SetBool("Dead", true);
        //this.anim.SetTrigger("Death");
        //deathSound.Play();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator HealthMeter()
    {
        while (healthTotal > 0)
        {
            switch (radiationStage) // SUBTRACTS HEALTH BASED ON RADIATION STAGE
            {
                case 1:
                    if (inHealingPad == false) healthTotal--;
                    else healthTotal++;
                    UpdateHealthUI();
                    break;

                case 2:
                    if (inHealingPad == false) healthTotal -= 2;
                    else healthTotal++;
                    UpdateHealthUI();
                    break;

                case 3:
                    if (inHealingPad == false) healthTotal -= 3;
                    else healthTotal++;
                    UpdateHealthUI();
                    break;

                case 4:
                    if (inHealingPad == false) healthTotal -= 4;
                    else healthTotal++;
                    UpdateHealthUI();
                    break;
            }
            yield return new WaitForSeconds(1f);
        }

        while (inHealingPad) // ADDS HEALTH WHILE IN HEALING PAD
        {
            if(healthTotal <= 100) healthTotal++;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator RadiationMeter()
    {
        while (radiationTotal < 100)
        {
            radiationTotal += numStationsOn; // ADDS RADIATION BASED ON THE NUMBER OF STATIONS ON

            if (radiationTotal > 75) radiationStage = 4; // ASSIGN RADIATION STAGE BASED ON RADIATION TOTAL
            else if (radiationTotal >= 50) radiationStage = 3;
            else if (radiationTotal >= 25) radiationStage = 2;
            else if (radiationTotal >= 1) radiationStage = 1;

            UpdateRadiationUI();
            yield return new WaitForSeconds(1f);
        }
    }
}