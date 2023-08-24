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

                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1";
                //ejemplo de comando con texto.. probar en sql primero

                comando.Connection = conexion;   //el comando se ejecuta en la ejecion indicada arriba ip..

                conexion.Open();

                lector = comando.ExecuteReader();   //lo que devuelve el comando se guarda en la variable lector.. que es del tipo sqldatareader ut supra


                while (lector.Read())    //si tiene una linea mas va a devolver un true.. corta cuando ya no hay mas objetos
                {
                    Pokemon aux = new Pokemon();

                    aux.Id = (int)lector["Id"]; //la propiedad ID de Pokemon se le asigna el valor recibido
                    //por el lector ID

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
                    aux.Tipo.Id = (int)lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"];

                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)lector["IdDebilidad"];
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
                datos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + " , '" + nuevo.Nombre +  "', ' " + nuevo.Descripcion + "', 1, @idTipo, @idDebilidad, @urlImagen)");
                
                //parametros.. @idTipo, @idDebilidad.. se crea como una referencia en la consulta.. entonces
                //realiza la consulta y se encuentra con una referencia a un parametro.. baja consulta y completa con el dato 
                //tomado.. 

                datos.setearParametro("@idTipo", nuevo.Tipo.Id); //se recibio en la funcion el nuevo pokemon..
                                                                 //tomado de la ventana. AGREGAR POKEMON..
                                                                 //ahi se tomo de la lista el tipo y la debilidad..
               
                datos.setearParametro("@idDebilidad" , nuevo.Debilidad.Id); //"ID" xq es una referencia numerica que nos
                                                                            //devuelve texto en la comparacion
                
                datos.setearParametro("@urlImagen", nuevo.UrlImagen);
                
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

        public void modificar(Pokemon poke)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @descripcion, UrlImagen = @imagen, IdTipo = @idTipo, IdDebilidad = @idDebilidad where id = @id");
                datos.setearParametro("@numero", poke.Numero);
                datos.setearParametro("@nombre", poke.Nombre);
                datos.setearParametro("@descripcion", poke.Descripcion);
                datos.setearParametro("@imagen", poke.UrlImagen);
                datos.setearParametro("@idTipo", poke.Tipo.Id);
                datos.setearParametro("@idDebilidad", poke.Debilidad.Id);
                datos.setearParametro("@id", poke.Id);

                datos.ejecutarAccion();
                
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

        public void eliminar(int id)
        {
            
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from pokemons where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void eliminarLogico(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update pokemons set activo = 0 where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1 And ";

                if (filtro != "") //error salvado.. consulta numero < "" (vacio).. crashea.. 
                {
                    switch (campo)
                    {
                        case "Número":
                            switch (criterio)
                            {
                                case "Menor a":
                                    consulta += "Numero < " + filtro;
                                    break;
                                case "Mayor a":
                                    consulta += "Numero > " + filtro;
                                    break;
                                case "Igual a":
                                    consulta += "Numero = " + filtro;
                                    break;
                            }//Numero
                            break;

                        case "Nombre":
                            switch (criterio)
                            {
                                case "Comienza con":        //se usa % para resultado parcial
                                    consulta += "Nombre like '" + filtro + "%'";
                                    break;          //comienza con es xxx + % (atencion a comillas dobles y simples)
                                case "Finaliza con":
                                    consulta += "Nombre like '%" + filtro + "'";
                                    break;         //finaliza con es % + xxx
                                case "Contiene":
                                    consulta += "Nombre like '%" + filtro + "%'";
                                    break;
                            }//Nombre
                            break;

                        case "Descripción":
                            switch (criterio)
                            {
                                case "Comienza con":
                                    consulta += "P.Descripcion like '" + filtro + "%'";
                                    break;
                                case "Finaliza con":
                                    consulta += "P.Descripcion like '%" + filtro + "'";
                                    break;
                                case "Contiene":
                                    consulta += "P.Descripcion like '%" + filtro + "%'";
                                    break;
                            }//Descripcion
                            break;
                    } //campo
                } else
                    consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1 ";



                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())     //si hay lectura.. bool yes.. 
                {
                    Pokemon aux = new Pokemon();

                    aux.Id = (int)datos.Lector["Id"]; 

                    aux.Numero = datos.Lector.GetInt32(0);  

                    aux.Nombre = (string)datos.Lector["Nombre"];     
                    aux.Descripcion = (string)datos.Lector["Descripcion"];


                    if (!(datos.Lector["UrlImagen"] is DBNull))         
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];    

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];

                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(aux);  
                }


                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
