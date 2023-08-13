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
    public class Pokemon //deben ser publicas para que se puedan usar en winform
    {
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public string UrlImagen { get; set; }

        //  no xq tengamos estos atributos son iguales a los de la db en este caso coincide pero
        // son internos.. se usan localmente en este caso..

        // esta clase define el modelo.. creamos otra para establecer la conexion.. pokemonNegocio

        public Elemento Tipo { get; set; }
        //aca creamos un constructor tipo de la clase elemento.. ojo que no esta instanciado!

        public Elemento Debilidad { get; set; }



    }
}
