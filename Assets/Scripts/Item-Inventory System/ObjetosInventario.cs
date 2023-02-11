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

    private Item LoadNew(string name, Transform trans) => Instantiate(Resources.Load<GameObject>(name), trans).GetComponent<Item>();

    public void Load() {
        CrearMalla();
        objetos = new Dictionary<int,Item>();
        identificarItem = new Dictionary<int, int>();
        if (File.Exists(inventoryFile)) {
            using (StreamReader sr = new StreamReader(inventoryFile)) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    string[] ss = line.Split(FILE_SEP);
                    Add(LoadNew(ss[1], transform), int.Parse(ss[0]));
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
            foreach (KeyValuePair<int, Item> kvp in objetos)
                sw.WriteLine(kvp.Key + FILE_SEP + kvp.Value.itemName);
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
        identificarItem[item.id] = slotPos;

        SlotManager slot = slots[slotPos];
        GameObject itemGO = item.gameObject;
        itemGO.transform.SetParent(slot.transform);
        itemGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        itemGO.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        itemGO.transform.SetParent(GameStateEngine.gse.oi.transform, true);
        itemGO.GetComponent<Item>().myslot = slot;
        return true;
    }

    public Item Remove(int slotPos) {
        Item item = objetos[slotPos];
        identificarItem.Remove(item.id);
        objetos.Remove(slotPos);
        item.transform.SetParent(null);
        return item;
    }

    public Item Remove(Item item){
        foreach(KeyValuePair<int,Item> kvp in objetos)
            if(item.Equals(kvp.Value))
                return Remove(kvp.Key);
        return null;
    }

    public bool Move(int itemId, int slotPos) {
        if (objetos.ContainsKey(slotPos))
            return false;
        int firstSlotPos = identificarItem[itemId];
        Add(Remove(firstSlotPos), slotPos);
        return true;
    }

    void Update(){
        if (Input.GetKeyUp("n")) {
            Save();
        }
        if (Input.GetKeyUp("m")) {
            Load();
        }
    }
}
