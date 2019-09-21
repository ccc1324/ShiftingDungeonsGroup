using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialObject : MonoBehaviour
{
    public GameObject KeyAnimationSingle;
    public GameObject KeyAnimmationDouble;
    public TextMeshProUGUI KeyTextSingle;
    public TextMeshProUGUI KeyTextA;
    public TextMeshProUGUI KeyTextB;
    public string Tutorial;
    public GameObject NextTutorial;
    public GameObject InventoryPointerA;
    public GameObject InventoryPointerB;
    public UIItem SelectedItem;
    public UIItem PrimaryWeapon;
    public UIItem SecondaryWeapon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (Tutorial)
            {
                case "Walk":
                    KeyAnimmationDouble.SetActive(true);
                    KeyTextA.text = "A";
                    KeyTextB.text = "D";
                    break;
                case "Jump":
                    KeyAnimationSingle.SetActive(true);
                    KeyTextSingle.text = ";";
                    break;
                case "Pickup":
                    KeyAnimationSingle.SetActive(true);
                    KeyTextSingle.text = "J";
                    break;
                case "Inventory":
                    KeyAnimationSingle.SetActive(true);
                    KeyTextSingle.text = "I";
                    StartCoroutine(InventoryTutorial());
                    break;
                case "Attack":
                    KeyAnimmationDouble.SetActive(true);
                    KeyTextA.text = "K";
                    KeyTextB.text = "L";
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            KeyAnimationSingle.SetActive(false);
            KeyAnimmationDouble.SetActive(false);
            StopAllCoroutines();
            switch (Tutorial)
            {
                case "Walk":
                    Destroy(gameObject);
                    break;
                case "Jump":
                    Destroy(gameObject);
                    break;
                case "Pickup":
                    if (Input.GetKeyDown("j"))
                        NextTutorial.SetActive(true);
                    break;
                case "Inventory":
                    break;
                case "Attack":
                    Destroy(gameObject);
                    break;
            }
        }
    }

    IEnumerator PickupTutorial()
    {
        while (true)
        {
            if (Input.GetKeyDown("j"))
                NextTutorial.SetActive(true);
            yield return null;
        }
    }

    IEnumerator InventoryTutorial()
    {
        while (true)
        {
            if (SelectedItem.Item != null)
            {
                InventoryPointerA.SetActive(false);
                InventoryPointerB.SetActive(true);
            }
            else if (PrimaryWeapon.Item != null || SecondaryWeapon.Item != null)
            {
                InventoryPointerA.SetActive(false);
                InventoryPointerB.SetActive(false);
                if (Input.GetKeyDown("i"))
                {
                    NextTutorial.SetActive(true);
                    Destroy(gameObject);
                }
            }
            else
            {
                InventoryPointerA.SetActive(true);
                InventoryPointerB.SetActive(false);
            }
            yield return null;
        }
    }
}
