using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    //public TextMeshProUGUI healthUI;
    public bool inHealingPad = false;
    public GameObject HP1;
    public GameObject HP2;
    public GameObject HP3;
    public GameObject HP4;
    public GameObject HP5;
    public GameObject HP6;
    public GameObject HP7;
    public GameObject HP8;
    public GameObject HP9;
    public GameObject HP10;
    //public Slider healthSlider;

    [Header("-Radiation Points-")]
    public float radiationTotal = 0;
    public float radiationStage = 0;
    public Slider radiationSlider;

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

        if (healthTotal < 1)
        {
            HP1.SetActive(false);
            HP2.SetActive(false);
            HP3.SetActive(false);
            HP4.SetActive(false);
            HP5.SetActive(false);
            HP6.SetActive(false);
            HP7.SetActive(false);
            HP8.SetActive(false);
            HP9.SetActive(false);
            HP10.SetActive(false);
        }
        else if (healthTotal < 10)
        {
            HP1.SetActive(true);
            HP2.SetActive(false);
        }
        else if (healthTotal < 20)
        {
            HP2.SetActive(true);
            HP3.SetActive(false);
        }
        else if (healthTotal < 30)
        {

            HP3.SetActive(true);
            HP4.SetActive(false);
        }
        else if (healthTotal < 40)
        {
            HP4.SetActive(true);
            HP5.SetActive(false);
        }
        else if (healthTotal < 50)
        {
            HP5.SetActive(true);
            HP6.SetActive(false);
        }
        else if (healthTotal < 60)
        {
            HP6.SetActive(true);
            HP7.SetActive(false);
        }
        else if (healthTotal < 70)
        {
            HP7.SetActive(true);
            HP8.SetActive(false);
        }
        else if (healthTotal < 80)
        {
            HP8.SetActive(true);
            HP9.SetActive(false);
        }
        else if (healthTotal < 90)
        {
            HP9.SetActive(true);
            HP10.SetActive(false);
        } else if (healthTotal > 90)
        {
            HP1.SetActive(true);
            HP2.SetActive(true);
            HP3.SetActive(true);
            HP4.SetActive(true);
            HP5.SetActive(true);
            HP6.SetActive(true);
            HP7.SetActive(true);
            HP8.SetActive(true);
            HP9.SetActive(true);
            HP10.SetActive(true);
        }

        //healthSlider.value = healthTotal;
    }

    private void UpdateRadiationUI() //REFRESHES RADIATION COUNTER UI
    {
        radiationSlider.value = radiationTotal;
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