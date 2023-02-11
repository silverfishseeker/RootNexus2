using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour {

    public const char NEGATION = '!';

    public static bool IsNegation(string caracteristica, out string outCaract){
        if (caracteristica[0] == NEGATION){
            outCaract = caracteristica.Substring(1);
            return true;
        }else{
            outCaract = caracteristica;
            return false;
        }
    }

    // Es pública para el isnpector de unity, no editar directamente
    public List<string> caracteristicas;

    public bool Condition(List<string> lista){
        string caract;
        foreach (string s in lista){
            if (IsNegation(s, out caract)){
                if (caracteristicas.Contains(caract))
                    return false;
            } else if(!caracteristicas.Contains(caract))
                return false;
        }

        return true;
    }

    public bool Remove(string caracteristica){
        return caracteristicas.Remove(caracteristica);
    }

    public bool Add(string caracteristica){
        if (caracteristicas.Contains(caracteristica))
            return false;
        caracteristicas.Add(caracteristica);
        return true;
    }
}
