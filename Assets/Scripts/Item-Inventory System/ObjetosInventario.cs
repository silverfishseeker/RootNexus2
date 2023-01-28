using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using TMPro;
using UnityEngine;

public class ObjetosInventario : MonoBehaviour {
    private string FILE_SEP = "_";

    public GameObject slot;
    public int filas;
    public int columnas;
    public int total {get => filas*columnas;}
    public float separación;
    public string inventoryFile;
    public TextMeshProUGUI itemTitle;
    public TextMeshProUGUI itemDescrp;


    public Dictionary<int,Item> objetos; //slotPos -> item
    public Dictionary<int, int> identificarItem; // itemId -> slotPos
    private SlotManager[] slots; //slotPos -> slot

    public void CrearMalla() {
        slots = new SlotManager[filas*columnas];
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                GameObject newSlot = Instantiate(slot, transform);
                newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(j*separación, -i*separación);
                slots[i*columnas + j] = newSlot.GetComponent<SlotManager>();
                newSlot.GetComponent<SlotManager>().slotPos = i*columnas + j;
            }
        }
    }

    public void Load() {
        CrearMalla();
        objetos = new Dictionary<int,Item>();
        identificarItem = new Dictionary<int, int>();
        if (File.Exists(inventoryFile)) {
            using (StreamReader sr = new StreamReader(inventoryFile)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    string[] ss = line.Split(FILE_SEP);
                    Item it = Instantiate(Resources.Load<GameObject>(ss[1]), transform).GetComponent<Item>();
                    Add(it, int.Parse(ss[0]));
                }
            }
        }
    }

    public void PreStart(){
        inventoryFile = GameStaticAccess.DataFolder+inventoryFile;
        Load();
    }

    public void Save() {
        using (StreamWriter sw = new StreamWriter(inventoryFile)) {
            foreach (KeyValuePair<int, Item> kvp in objetos) {
                string itemName = kvp.Value.gameObject.name.Substring(0, kvp.Value.gameObject.name.Length - 7);
                sw.WriteLine(kvp.Key + FILE_SEP + itemName);                    
            }
        }
    }

    public bool Add(Item item, int slotPos = -1) {
        if (slotPos >= 0) {
            if (objetos.ContainsKey(slotPos))
                return false;
        } else
            while(objetos.ContainsKey(++slotPos)){
                if (slotPos >= total)
                    return false;
            }
        
        objetos[slotPos] = item;
        slots[slotPos].item = item.gameObject;
        identificarItem[item.id] = slotPos;
        return true;
    }

    public Item Remove(int slotPos) {
        Item item = objetos[slotPos];
        identificarItem.Remove(item.id);
        objetos.Remove(slotPos);
        slots[slotPos].DetachItem();
        return item;
    }

    public void Move(int itemId, int slotPos) {
        if (slots[slotPos].item != null)
            throw new ArgumentException("slot "+slotPos+" ya ocupado");
        int firstSlotPos = identificarItem[itemId];
        Add(Remove(firstSlotPos), slotPos);
    }

    void Update(){
        if (Input.GetKeyUp("b")) {
            //Add(ejemplo);
        }
        if (Input.GetKeyUp("n")) {
            Save();
        }
        if (Input.GetKeyUp("m")) {
            Load();
        }
    }
}
