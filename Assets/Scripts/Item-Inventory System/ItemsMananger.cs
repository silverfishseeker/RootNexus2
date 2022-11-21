using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class ItemsMananger : MonoBehaviour {

    public static string RegPath;

    private static IEnumerable<string> ReadIDs => File.ReadAllLines(RegPath);

    private Dictionary<string,Item> items;

    public static bool AddNewID(string id) {
        if (ReadIDs.Contains(id))
            return false;
        File.AppendAllText(RegPath, id + "\n");
        return true;
    }

    private IEnumerable<string> ChildsIds() {
        foreach (Transform child in transform)
            yield return child.GetComponent<Item>().ID;
    }
    
    static ItemsMananger() {
        RegPath = GameStaticAccess.DataFolder+"registro";
        if (!File.Exists(RegPath))
            File.Create(RegPath).Dispose();
    }

    public bool EnsureItemIDsConsistency() {
        HashSet<string> r = new HashSet<string>(ReadIDs);
        foreach (string id in ChildsIds()) {
            if (!r.Remove(id))
                return false;
        }

        return r.Count == 0;
    }

    // Usar esta función cuando se ha eliminado items de la lista y
    // se quieren eliminar sus ids del registro para así manterner la consistencia
    public void ClearMissingIDsInRegister() {
        File.WriteAllLines(RegPath, ReadIDs.Intersect(ChildsIds())); // ReadIDs - childsIds
    }

    void Start() {
        items = new Dictionary<string,Item>();
        foreach (Transform child in transform)
            items[child.GetComponent<Item>().ID] = child.GetComponent<Item>();

    }

    public Item GetItemById(string id) => items[id]; 

}
