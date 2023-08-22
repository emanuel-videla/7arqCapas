using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace WindowsFormsApp1
{
    public partial class frmAltaPokemon : Form
    {
        private Pokemon pokemon = null; //esto servira para hacer una validacion.. 
        //si "pokemon" es nulo significa que no estamos modificando estamos abriendo la ventana normal
        // cuando tenga un valor es porque recibimos un pokemon "seleccionado"

        public frmAltaPokemon() 
        {
            InitializeComponent();
        }

        public frmAltaPokemon(Pokemon pokemon) //recibe un poke seleccionado
        {
            InitializeComponent();
            this.pokemon = pokemon; //this pokemon se refiere a pokemon dentro de esta clase creado arriba
                                    // y = pokemon se refiere al pokemon recibido por parametro
                                    //entonces al de este scope le asignamos el valor recibido
            
            Text = "Modificar Pokemon"; //para cambiarle el nombre a la ventana
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Pokemon poke = new Pokemon();  antes habia un solo evento para este "agregar"
            //ahora tenemos dos instancias asi que hay que buscar otro metodo

            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                if (pokemon == null)   //si apretaste aceptar.. tal vez signifique que quieras agregar
                                       //un pokemon nuevo.. x eso creamos una nueva instancia que anulara
                                       //la anterior
                    pokemon = new Pokemon();
                

                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtUrlImagen.Text;
                pokemon.Tipo = (Elemento)cbTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cbDebilidad.SelectedItem;

                //aqui arriba al crear una instancia nueva y al haberse asignado valores a las variables 
                //ya no estan nulas entonces podemos hacer validaciones

                if(pokemon.Id != 0) //si ya tiene un valor modifico
                {
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado correctamente");
                }
                else
                {
                    negocio.agregar(pokemon);
                    MessageBox.Show("Agregado correctamente");
                }

                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();

            try
            {
                cbTipo.DataSource = elementoNegocio.listar();
                cbTipo.ValueMember = "Id"; //nombres de la propiedades de elemento id y descrip...
                cbTipo.DisplayMember = "Descripcion"; //tomamos 2 valores q querramos..

                cbDebilidad.DataSource = elementoNegocio.listar();
                cbDebilidad.ValueMember = "Id"; 
                cbDebilidad.DisplayMember = "Descripcion";

                //usamos la validacion del poke = null
                if (pokemon != null) //cargamos los valores preasignado.. extraidos de la DB
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen; //aca carga el link
                    cargarImagen(pokemon.UrlImagen);       //aca cargan la imagen

                    cbTipo.SelectedValue = pokemon.Tipo.Id;
                    cbDebilidad.SelectedValue = pokemon.Debilidad.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)  //creo un metodo para invocarlo y asi no
        {                                           //repetir
            try
            {
                pbPokemon.Load(imagen);
            }
            catch (Exception ex)
            {
                pbPokemon.Load("https://i0.wp.com/theperfectroundgolf.com/wp-content/uploads/2022/04/placeholder.png?fit=1200%2C800&ssl=1");
                //podria cargar una imagen por defecto para que en caso que la imagen de error se sustituya 
                //asi muestre una imagen que indica que no hay una real y evitar que crashee
            }
        }


    }
}
