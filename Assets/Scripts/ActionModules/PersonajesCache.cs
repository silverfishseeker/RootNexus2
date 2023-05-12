using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonajesCache: MonoBehaviour {

    public Dictionary<int, List<string>> caracteristicasCache;

    void OnEnable(){// como hace falta para cargar la escena, es necesario que se ejecute con un OnEnable que va antes que un OnSceneLoaded. Start va después
        caracteristicasCache = new Dictionary<int, List<string>>();
    }

    public void SaveToCache(){
        foreach(Personaje p in FindObjectsOfType<Personaje>())
            caracteristicasCache[p.id] = p.caracteristicas;
    }

    public void LoadToScene(){
        foreach(Personaje p in FindObjectsOfType<Personaje>())
            if(caracteristicasCache.ContainsKey(p.id))
                p.caracteristicas = caracteristicasCache[p.id];
    }
    
}



