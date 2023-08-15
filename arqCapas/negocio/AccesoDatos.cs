using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace negocio
{
    //para centralizar.. invocar el metodo y no estar escribiendo toda la declaracion de conexiones.. url..
    //comandos.. lector.. editor.. etc

    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; } //solo para leer la propiedad lector.. contenido
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection("server=DESKTOP-9JO9LGS\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true");
            //metodo sobrecargado.. antes se instancio luego se referencio.. aca se crea y se usa la sobrecarga...

            comando = new SqlCommand();
        }
            
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }


        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();

            conexion.Close();
        }

    }
}
