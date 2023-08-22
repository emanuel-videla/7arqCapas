﻿using System;
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
using WindowsFormsApp1;

namespace winform_app
{
    public partial class frmPokemons : Form
    {

        private List<Pokemon> listaPokemon;

        
        public frmPokemons()
        {
            InitializeComponent();
        }


        private void frmPokemons_Load(object sender, EventArgs e)
        {        }

        private void frmPokemons_Load_1(object sender, EventArgs e)
        {
            cargar();
        }

        private void cargar() //creamos el metodo que hara que se actualice la ventana anterior cuando se cierre la de carga
        {

            try
            {
                PokemonNegocio negocio = new PokemonNegocio();           //creo un objeto para invocarlo
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;
                dgvPokemons.Columns["UrlImagen"].Visible = false; //con esto oculto una columna.. la del url que muestra imagen
                dgvPokemons.Columns["Id"].Visible = false;
                cargarImagen(listaPokemon[0].UrlImagen);           //le cargamos por defecto la primer posicion
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        private void dgvPokemons_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //(Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

            //al data grid.. la fila actual... dame el objeto enlazado a esa fila.. 
            //devuelve un objeto.. en este caso sabemos que el tipo es pokemon..
            //entonces le indicamos explisitamente que es un pokemon

            //creamos una variable para guardar la seleccion.. tipo pokemon tmb

            Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
             cargarImagen(seleccionado.UrlImagen);
            //entonces en la pictbox va a cargar lo que este seleccionando
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

        private void btnAgregar_Click(object sender, EventArgs e)

        {
            frmAltaPokemon alta = new frmAltaPokemon();  //creamos una instancia del tipo
            alta.ShowDialog();                          //invocamos la ventana
            cargar();               //creamos el metodo que hara que se actualice la ventana anterior cuando se cierre la de carga
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem; //fila actual.. item seleccionado
            //le voy a mandar el seleccionado que va a ser el que quiero modificar..



            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado); 
            //esta es una instancia de metodo.. vamos a modificarlo para que reciba un parametro

            modificar.ShowDialog(); 
            cargar(); 
        }
    }
}
