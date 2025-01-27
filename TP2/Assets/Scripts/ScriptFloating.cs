﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptFloating : MonoBehaviour {

    // Temps que le joueur peut rester dans l'air
    [SerializeField] float FloatingTime = 3.0f;
    // Temps du recharge par rapport au FloatingTime. Plus c'est eleve, plus le temps recharge est court.
    [SerializeField] float RefillSpeed = 1f;
    [SerializeField] Image FloatingImagefill;

    float currentFloatingTime;

    // Boolean pour activer le cooldown
    bool coolDown;
    // Boolean pour verifier si le cooldown est epuise
    bool canFloat;

    // Use this for initialization
    void Start () {
        coolDown = false;
        canFloat = true;
        FloatingImagefill.fillAmount = 1;

        // Verification pour les valeurs negatives
        if (FloatingTime < 0)
            FloatingTime = 3f;

        if (RefillSpeed <= 0)
            RefillSpeed = 1f;
        currentFloatingTime = FloatingTime;
    }
	
	// Update is called once per frame
	void Update () {

        // Verifier si le joueur peut flotter
        FloatCoolDown();

        // Laisser flotter le joueur s'il appuie sur la touche pour flotter
        Float();
    }

    void Float()
    {
        if (Input.GetButton("Ability") && canFloat)
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currentFloatingTime -= Time.deltaTime;
            FloatingImagefill.GetComponent<Image>().fillAmount = currentFloatingTime / FloatingTime;
            // Consider player as floor when he is floating
            gameObject.layer = LayerMask.NameToLayer("Floor");
            coolDown = false;

            // Dseactiver la flottaison jusqu'a la fin du cooldown
            if (currentFloatingTime <= 0)
            {
                coolDown = true;
                canFloat = false;
            } 
        } else if (currentFloatingTime < FloatingTime)
        {
            coolDown = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            // Set layer back to normal
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    void FloatCoolDown()
    {
        if (coolDown)
        {
            currentFloatingTime += Time.deltaTime * RefillSpeed; 
            FloatingImagefill.GetComponent<Image>().fillAmount = currentFloatingTime / FloatingTime; 


            // Retour à la normale
            if (currentFloatingTime >= FloatingTime)
            {
                coolDown = false;
                canFloat = true;
            }
        }

    }
}
