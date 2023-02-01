using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Type

/* Esta clase está pensanda para usarse de forma muy concreta:
1. Para que un objeto use esta clase debe de implementar IExclSelectable
2. Te creas un atributo y lo inicializas pasándole como parámetro uno mismo
3. Llamas Select() de tu atributo para seleccionarse y usa isSelected para saber si uno mismo lo está o no

Esto es así porque c# no te deja herencia múltiple y hace falta variables estáticas por cada tipo que use esto.
Tal vez usar genéricos ayudaría a simplificar esto, yo que sé.
*/
public class ExclusivityManager {

    private class Category{
        public int nextId = 0;
        public List<bool> seleccionados = new List<bool>();
        public List<IExclSelectable> instancias = new List<IExclSelectable>();
    }
    private static Dictionary<Type,Category> categorias = new Dictionary<Type,Category>();

    public static IExclSelectable Current<T>() {
        List<bool> sels = categorias[typeof(T)].seleccionados;
        for (int i = 0; i < sels.Count; i++)
            if(sels[i])
                return categorias[typeof(T)].instancias[i];
        return null;
    }


    private Category categoria;
    private List<bool> selecs => categoria.seleccionados;
    private List<IExclSelectable> insts => categoria.instancias;

    private int myId;

    public bool isSelected => selecs[myId];

    public ExclusivityManager(IExclSelectable ide){
        Type tipo = ide.GetType();
        if(!categorias.ContainsKey(tipo))
            categorias[tipo] = new Category();
        categoria = categorias[tipo];

        myId = categoria.nextId++;
        selecs.Add(false);
        insts.Add(ide);
    }

    public void Select(){
        selecs[myId] = true;
        insts[myId].Select();
        for (int i = 0; i < selecs.Count; i++)
            if( i != myId && selecs[i] == true){
                selecs[i] = false;
                insts[i].Deselect();
            }
    }

}
