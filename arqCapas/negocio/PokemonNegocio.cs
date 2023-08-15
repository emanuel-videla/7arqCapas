using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;



namespace negocio //depende de negocio ahora
{
    public class PokemonNegocio //lo hacemos publico
    {
        public List<Pokemon> listar()           //creamos un metodo que nos devolvera una lista de la db

        {
            List<Pokemon> lista = new List<Pokemon>();                      // creamos lista

            SqlConnection conexion = new SqlConnection();                   //nueva conexion invocada

            SqlCommand comando = new SqlCommand();                          //comandos

            SqlDataReader lector;                                           //lector data reader.. 

            try
            {
                conexion.ConnectionString = "server=DESKTOP-9JO9LGS\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                //variable                                  (local)\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                //variable con punto                              .\\SQLEXPRESS; database=POKEDEX_DB;
                //integrated sec false y se agrega con credencial user="..."; password="...";


                comando.CommandType = System.Data.CommandType.Text; //texto comando.. tipo ejecucion son comportamientos funciones en la nube

                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad";
                //ejemplo de comando con texto.. probar en sql primero

                comando.Connection = conexion;   //el comando se ejecuta en la ejecion indicada arriba ip..

                conexion.Open();

                lector = comando.ExecuteReader();   //lo que devuelve el comando se guarda en la variable lector.. que es del tipo sqldatareader ut supra


                while (lector.Read())    //si tiene una linea mas va a devolver un true.. corta cuando ya no hay mas objetos
                {
                    Pokemon aux = new Pokemon();
                    aux.Numero = lector.GetInt32(0);   // indice 0 porque el numero es el primer valor del vector 0, 1, 2, 3.. se 
                                                       //puede usar con los string pero se muestra otra forma de hacerlo

                    aux.Nombre = (string)lector["Nombre"];              //le aclaro.. fuerzo que entienda.. que devuelve un string..
                    aux.Descripcion = (string)lector["Descripcion"];

                    //para evitar que crashee podemos validar.. consultando si es nulo el UrlImagen... 2 soluciones
                    // 1---
                    //     if (!(lector.IsDBNull(lector.GetOrdinal("UrlImagen"))))
                    //     aux.UrlImagen = (string)lector["UrlImagen"];
                    //aca se consulta si la columna UrlImagen es nula... entonces la negamos porque necesitamos que si es 
                    //nula no la lea asi evitamos que crashee..
                    // 2--

                    if (!(lector["UrlImagen"] is DBNull))               //si la ubicacion del lector "Url" es nula.. negado
                        aux.UrlImagen = (string)lector["UrlImagen"];    //entonces afirmativo.. no lee y el string queda vacio

                    //tipo elemento no esta instanciado
                    aux.Tipo = new Elemento();
                    aux.Tipo.Descripcion = (string)lector["Tipo"];

                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];

                    lista.Add(aux); //agrego a la lista.. en cada vuelva posicion de vector... hasta q de falso y termina
                }


                conexion.Close(); // se puede colocar en finaly por si falla el try

                return lista;       //si la conexion sale bien retorna una lista sino throw exception

            } //fin try
            catch (Exception ex)
            {
                throw ex;                           //aca tira ex.. algo no funciono
            }

        }
        public void agregar(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos(); //creo una instancia

            try
            {
                datos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad)values(" + nuevo.Numero + " , '" + nuevo.Nombre +  "', ' " + nuevo.Descripcion + "', 1, @idTipo, @idDebilidad)");
                
                //parametros.. @idTipo, @idDebilidad.. se crea como una referencia en la consulta.. entonces
                //realiza la consulta y se encuentra con una referencia a un parametro.. baja consulta y completa con el dato 
                //tomado.. 

                datos.setearParametro("@idTipo", nuevo.Tipo.Id); //se recibio en la funcion el nuevo pokemon..
                                                                 //tomado de la ventana. AGREGAR POKEMON..
                                                                 //ahi se tomo de la lista el tipo y la debilidad..
                datos.setearParametro("@idDebilidad" , nuevo.Debilidad.Id);

                datos.ejecutarAccion(); //cargada la solicitud/consulta y los parametros.. ejecutamos..
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Pokemon modificar)
        {

        }
    }
}
