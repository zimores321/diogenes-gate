﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class inventoryManager : MonoBehaviour
{
    // Start is acalled before the first frame update
    public int window;
    public int selected;
    public int item;
    GameObject partyWindow;
    GameObject inventoryWindow;

    void Start()
    {
        selected = 0;
        window = 0;
        partyWindow = GameObject.Find("party");
        inventoryWindow = GameObject.Find("inventory");
        setParty();      
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            GameController curr = GameController.getInstance();
            curr.worldDetails.currentScene = curr.worldDetails.lastScene;
            curr.worldDetails.lastScene = curr.worldDetails.currentScene;
            SceneManager.LoadScene(curr.worldDetails.currentScene);
           
        }
    }

    public void setParty()
    {
        GameController curr = GameController.getInstance();
        int j = 0;
        int i = curr.playerDetails.partySize();
        GameObject originalGameObject = GameObject.Find("CharList");
        while (j < 6)
        {
            pcObject temp = curr.playerDetails.partyMem(j);
            GameObject child = originalGameObject.transform.GetChild(j).gameObject;

            if (temp != null)
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = temp.pcName;
            }
            else
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }

            j++;
        }
        if(selected!=-1)
        {
            originalGameObject = GameObject.Find("charDesc");
            GameObject child = originalGameObject.transform.Find("Dropdown").gameObject;
            child.GetComponentInChildren<TMP_Dropdown>().ClearOptions();
            int f = 0;
            List<string> L = new List<string>();
            while (f < 6)
            {
                pcObject pc = curr.playerDetails.partyMem(f);
                if (pc != null)
                {
                    string cha = pc.pcName;
                    L.Add(cha);
                }
                f++;
            }
            child.GetComponentInChildren<TMP_Dropdown>().AddOptions(L);

            pcObject temp = curr.playerDetails.partyMem(selected);
            if (temp != null)
            {
                string output = "Class:" + "\nName: " + temp.pcName + "\nDefense: " + temp.dodge + "\nHealth: " + +temp.healthLeft + "/"+temp.health + "\nMana:" +temp.manaLeft+"/"+ temp.mana;
                originalGameObject.GetComponentInChildren<TextMeshProUGUI>().text = output;

                child = originalGameObject.transform.Find("helm").gameObject;
                if (temp.armor[1] != null)
                {
                    child.GetComponentInChildren<TextMeshProUGUI>().text = temp.armor[1].itemName;
                }
                else
                {
                    child.GetComponentInChildren<TextMeshProUGUI>().text = "nothing";
                }

                child = originalGameObject.transform.Find("weapon").gameObject;
                if (temp.armor[0] != null)
                {
                    child.GetComponentInChildren<TextMeshProUGUI>().text = temp.armor[0].itemName;
                }
                else
                {
                    child.GetComponentInChildren<TextMeshProUGUI>().text = "nothing";
                }

                child = originalGameObject.transform.Find("armor").gameObject;
                if (temp.armor[2] != null)
                {
                    child.GetComponentInChildren<TextMeshProUGUI>().text = temp.armor[2].itemName;
                }
                else
                {
                    child.GetComponentInChildren<TextMeshProUGUI>().text = "nothing";
                }
            }
        }
    }


    public void setInven()
    {
        GameController curr = GameController.getInstance();

        GameObject originalGameObject = GameObject.Find("inventory");
        int j = 0;
        while (j < 42)
        {
            itemObject temp = curr.playerDetails.invMem(j);
            GameObject child = originalGameObject.transform.Find("slots").transform.GetChild(j).gameObject;

            if (temp != null)
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = temp.itemName;
            }
            else
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }

            j++;
        }

    }

    public void onNameClick(int a)
    {
        selected = a;
        setParty();
    }

    public void onSwapClick(int a)
    {
        GameController curr = GameController.getInstance();

        if (a == 1)
        {
           window = a;
           partyWindow.SetActive(false);
           inventoryWindow.SetActive(true);
           setInven();
        }
        else if(a == 0)
        {
            window = a;
            partyWindow.SetActive(true);
            inventoryWindow.SetActive(false);
            setParty();
        }
        else if (a == 2)
        {
            string potion = JsonUtility.ToJson(curr.playerDetails);
            System.IO.File.WriteAllText("Assets/Scripts/Data/json/loadParty.txt", potion);
            potion = JsonUtility.ToJson(curr.worldDetails);
            System.IO.File.WriteAllText("Assets/Scripts/Data/json/loadWorld.txt", potion);
        }
        else
        {
            SceneManager.LoadScene("Main_Menu");
        }
 
    }

    public void onArmorClick(int a)
    {
        GameController curr = GameController.getInstance();
        GameObject originalGameObject = GameObject.Find("charDesc");
        pcObject temp = curr.playerDetails.partyMem(selected);
        if (temp != null)
        {
            itemObject remove = temp.armor[a];
            temp.armor[a] = null;
            curr.playerDetails.addItem(remove);
        }
        setParty();
    }

    public void onInvClick(int a)
    {
        GameController curr = GameController.getInstance();
        item = a;
        itemObject temp = curr.playerDetails.invMem(a);

        GameObject originalGameObject = GameObject.Find("inventory");
        GameObject child = originalGameObject.transform.Find("useMenu").transform.Find("Dropdown").gameObject;
        child.GetComponentInChildren<TMP_Dropdown>().ClearOptions();
        int j = 0;
        List<string> L= new List<string>();
        while (j < 6)
        {
            pcObject pc = curr.playerDetails.partyMem(j);
            if (pc!=null)
            {
                string cha = pc.pcName;
                L.Add(cha);
            }
            j++;
        }
        child.GetComponentInChildren<TMP_Dropdown>().AddOptions(L);



        child = originalGameObject.transform.Find("useMenu").transform.Find("description").gameObject;
        if (temp != null)
        {
            string s;
            if(temp.type==4)//heal
            {
                s ="\nHeals: " + temp.value;
            }
            else if (temp.type == 3)//mana
            {
                s = "\nMana gained: " + temp.value;
            }
            else if(temp.type ==1 || temp.type == 2)//defense
            {
                s = "\nDefense: " + temp.value;
            }
            else
            {
                s = "\nDamage: " + temp.value;
            }
            string output = "Name: " + temp.itemName + "\nConsumable: " + temp.destroyOnUse + s + "\ncarried: " + temp.number;
            child.GetComponentInChildren<TextMeshProUGUI>().text = output;
            child = originalGameObject.transform.Find("useMenu").gameObject;
            child.transform.position = new Vector3(987, 530, 0);
        }
        else
        {
            child.GetComponentInChildren<TextMeshProUGUI>().text = "";
            child = originalGameObject.transform.Find("useMenu").gameObject;
            child.transform.position = new Vector3(-429, 200, 0);
            setInven();
        }



    }

    public void onUseClick(int a)
    {
        GameObject originalGameObject = GameObject.Find("inventory");
        GameObject child = originalGameObject.transform.Find("useMenu").transform.Find("Dropdown").transform.Find("Label").gameObject;
        GameController curr = GameController.getInstance();
        itemObject currItem = curr.playerDetails.invMem(item);
        pcObject selectedPC = curr.playerDetails.partyMem(child.GetComponentInChildren<TextMeshProUGUI>().text);
        if (a==0)
        {
            Debug.Log(currItem.number);
            if (currItem.type == 0)     //damage
            {
                if (selectedPC.armor[0]!=null)
                {
                    curr.playerDetails.addItem(selectedPC.armor[0]);
                    Debug.Log(currItem.number);
                }
                selectedPC.armor[0] = currItem;
                curr.playerDetails.removeItem(currItem);
                Debug.Log(currItem.number);
            }
            else if (currItem.type == 1)//head
            {
                if (selectedPC.armor[1] != null)
                {
                    curr.playerDetails.addItem(selectedPC.armor[1]);
                }
                selectedPC.armor[1] = currItem;
                curr.playerDetails.removeItem(currItem);
            }
            else if (currItem.type == 2)//chest
            {
                if (selectedPC.armor[2] != null)
                {
                    curr.playerDetails.addItem(selectedPC.armor[2]);
                }
                selectedPC.armor[2] = currItem;
                curr.playerDetails.removeItem(currItem);
            }
            else if (currItem.type == 3)//mana
            {
                selectedPC.manaLeft += currItem.value;
                if(selectedPC.manaLeft> selectedPC.mana)
                {
                    selectedPC.manaLeft = selectedPC.mana;
                }
                curr.playerDetails.removeItem(currItem);
            }
            else //heal
            {
                selectedPC.healthLeft += currItem.value;
                if (selectedPC.healthLeft > selectedPC.health)
                {
                    selectedPC.healthLeft = selectedPC.health;
                }
                curr.playerDetails.removeItem(currItem);
            }
        }

        child = originalGameObject.transform.Find("useMenu").gameObject;
        child.transform.position = new Vector3(-429, 200, 0);
        setInven();
    }

    public void onCharSwapClick()
    {
        GameObject originalGameObject = GameObject.Find("party").transform.Find("charDesc").gameObject;
        GameObject child = originalGameObject.transform.Find("Dropdown").transform.Find("Label").gameObject;

        GameController curr = GameController.getInstance();
        pcObject selectedPC = curr.playerDetails.partyMem(child.GetComponentInChildren<TextMeshProUGUI>().text);
        int i;
        i = curr.playerDetails.pcIndex(selectedPC.pcName);
        curr.playerDetails.swapParty(i, selected);
        setParty();
    }
}
