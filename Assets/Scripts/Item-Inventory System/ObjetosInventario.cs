using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class ObjetosInventario : MonoBehaviour {
    private string FILE_SEP = "_";

    public GameObject slot;
    public int filas;
    public int columnas;
    public int total {get => filas*columnas;}
    public float separación;
    public string inventoryFile;

    public Dictionary<int,Item> objetos; //slotPos -> item
    public Dictionary<string, int> identificarItem; // itemid -> slotPos
    private SlotManager[] slots; //slotPos -> slot
    private ItemsMananger im;

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
        identificarItem = new Dictionary<string, int>();
        if (File.Exists(inventoryFile)) {
            using (StreamReader sr = new StreamReader(inventoryFile)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    string[] ss = line.Split(FILE_SEP);
                    Add(im.GetItemById(ss[1]).GetMe(), int.Parse(ss[0]));
                }
            }
        }
    }

    public void PreStart(){
        im = GameStateEngine.gse.im;
        inventoryFile = GameStaticAccess.DataFolder+inventoryFile;
        Load();
    }

    public void Save() {
        using (StreamWriter sw = new StreamWriter(inventoryFile)) {
            foreach (KeyValuePair<int, Item> kvp in objetos) {
                sw.WriteLine(kvp.Key + FILE_SEP + kvp.Value.ID);                    
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
        identificarItem[item.ID] = slotPos;
        return true;
    }

    public Item Remove(int slotPos) {
        Item item = objetos[slotPos];
        identificarItem.Remove(item.ID);
        objetos.Remove(slotPos);
        slots[slotPos].DetachItem();
        return item;
    }

    public void Move(String itemID, int slotPos) {
        if (slots[slotPos].item != null)
            throw new ArgumentException("slot "+slotPos+" ya ocupado");
        int firstSlotPos = identificarItem[itemID];
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