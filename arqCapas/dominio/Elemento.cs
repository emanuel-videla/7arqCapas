using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
//namespace winform_app  el namespace quedo winform porque era la ubicacion que tenia
//antes de copiarlo a esta nueva clase...ahora depende de DOMINIO
//quedara referenciar a la clase dominio con la aplicacion principal.. add referencia



{
    public class Elemento //deben ser publicas para que se puedan usar en winform
    {
        //esta clase nos va a servir para modelar los elementos.. que es otra clase aparte del tipo pokemon..

        public int Id { get; set; }

        public string Descripcion { get; set; }

        public override string ToString()       //
        {
            return Descripcion;
        }

    }
}
