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
    private class SelIns{
        public bool sel;
        public IExclSelectable ins;
        public SelIns(bool sel, IExclSelectable ins){
            this.sel = sel;
            this.ins = ins;
        }
    }

    private class Category{
        public int nextId = 0;
        public Dictionary<int,SelIns> dict = new Dictionary<int,SelIns>();
    }
    private static Dictionary<Type,Category> categorias = new Dictionary<Type,Category>();

    public static T Current<T> () where T : IExclSelectable {
        Dictionary<int,SelIns> dict = categorias[typeof(T)].dict;
        foreach (KeyValuePair<int,SelIns> kvp in dict)
            if(kvp.Value.sel)
                return (T)kvp.Value.ins;
        return default(T);
    }


    private Category categoria;

    private int myId;

    public bool isSelected => categoria.dict[myId].sel;

    public ExclusivityManager(IExclSelectable ide){
        Type tipo = ide.GetType();
        if(!categorias.ContainsKey(tipo))
            categorias[tipo] = new Category();
        categoria = categorias[tipo];

        myId = categoria.nextId++;
        categoria.dict[myId] = new SelIns(false, ide);
    }

    public void Select(){
        categoria.dict[myId].sel = true;
        categoria.dict[myId].ins.Select();
        foreach (KeyValuePair<int,SelIns> kvp in categoria.dict)
            if(kvp.Key != myId && kvp.Value.sel == true){
                categoria.dict[kvp.Key].sel = false;
                categoria.dict[kvp.Key].ins.Deselect();
            }
    }

    public void Destroy(){
        categoria.dict.Remove(myId);
    }

}
