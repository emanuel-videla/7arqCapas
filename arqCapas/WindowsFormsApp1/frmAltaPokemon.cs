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
        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Pokemon poke = new Pokemon();
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                poke.Numero = int.Parse(txtNumero.Text);
                poke.Nombre = txtNombre.Text;
                poke.Descripcion = txtDescripcion.Text;
                poke.UrlImagen = txtUrlImagen.Text;
                poke.Tipo = (Elemento)cbTipo.SelectedItem;
                poke.Debilidad = (Elemento)cbDebilidad.SelectedItem;


                negocio.agregar(poke);
                MessageBox.Show("Agregado correctamente");
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
                cbDebilidad.DataSource = elementoNegocio.listar();
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
