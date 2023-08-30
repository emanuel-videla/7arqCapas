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


        //private void frmPokemons_Load(object sender, EventArgs e)
        //{aca se cargan las funciones metodos al momento de abrir la apk.. es la carga de arranque}

        private void frmPokemons_Load_1(object sender, EventArgs e)
        {
            cargar();

            //filtro avanzado.. el segundo cbo dependera de la primer eleccion
            cbCampo.Items.Add("Número");
            cbCampo.Items.Add("Nombre");
            cbCampo.Items.Add("Descripción");
        }

        private void cargar() //creamos el metodo que hara que se actualice la ventana anterior cuando se cierre la de carga
        {

            try
            {
                PokemonNegocio negocio = new PokemonNegocio();           //creo un objeto para invocarlo
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;
                ocultarColumnas();
                cargarImagen(listaPokemon[0].UrlImagen);           //le cargamos por defecto la primer posicion
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false; //con esto oculto una columna.. la del url que muestra imagen
            dgvPokemons.Columns["Id"].Visible = false;
        }
        private void dgvPokemons_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            //(Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

            //al data grid.. la fila actual... dame el objeto enlazado a esa fila.. 
            //devuelve un objeto.. en este caso sabemos que el tipo es pokemon..
            //entonces le indicamos explisitamente que es un pokemon

            //creamos una variable para guardar la seleccion.. tipo pokemon tmb
            if (dgvPokemons.CurrentRow != null) //si hay una fila seleccionada va a entrar y cargar
            { //para evitar exception
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
                //entonces en la pictbox va a cargar lo que este seleccionando
            }
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

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminacionLogica_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }


        private void eliminar(bool logico = false) //si pongo esta condicion estoy diciendo que el
        {                                     //parametro a recibir es opcional.. si saco la igualdad
                                              //me diria en el metodo que necesito enviar un valor bool
            PokemonNegocio negocio = new PokemonNegocio();
            Pokemon seleccionado;
            try
            {
                //pedir confirmacion.. metodo sobrecargado
                //guardamos el resultado en una variable 
                DialogResult respuesta = MessageBox.Show("Seguro desea eliminar? No se puede recuperar este registro", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


                if (respuesta == DialogResult.Yes) 
                {
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                    
                    if (logico)
                        negocio.eliminarLogico(seleccionado.Id);
                    else
                        negocio.eliminar(seleccionado.Id);
                    
                    cargar();
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length > 2)
            {
                //expresion lamda es como un for each.. en cada verificacion si es positiva la comparacion
                //lo guarda en una nueva lista.. ahora variable listaFiltrada..
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper() ) || x.Tipo.Descripcion.ToUpper().Contains(filtro.ToUpper() ));
            } //constains buscara resultado parciales .. mas condiciones con O ||...
            else
            {
                listaFiltrada = listaPokemon;
            }

            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbCampo.SelectedItem.ToString();
            if (opcion == "Número")
            {
                cbCriterio.Items.Clear(); //siempre primero limpiamos.. luego asignamos despleglables
                cbCriterio.Items.Add("Menor a");
                cbCriterio.Items.Add("Igual a");
                cbCriterio.Items.Add("Mayor a");
            }else{
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Comienza con");
                cbCriterio.Items.Add("Contiene");
                cbCriterio.Items.Add("Finaliza con"); //se podria usar el filtro rapido..
                //pero la practica es hacer una consulta sobre la base de datos.. no recomendado 
            }
        }

        private bool validarFiltro()
        {
            if (cbCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor complete el campo para filtrar...");
                return true;
            }
            if (cbCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor complete el criterio para filtrar...");
                return true;
            }
            if (cbCampo.SelectedItem.ToString() == "Número") 
            { 
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes ingresar números para referenciar..");
                    return true;
                }
            
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Solo se filtran números en este campo..");
                    return true;
                }
            }
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter))) //si algun caracter es numero.. y esta negado
                    return false;             //osea.. sino es numero returna falso y corta..
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            //if (txtFiltroAvanzado.Text.Length > 0)
            //{
                try
                {
                if (validarFiltro()) //si entra al if.. y es positivo.. significa que no se ha
                    return;          //seleccioonado ninguna opcion .. entonces corta la 
                                     //accion.. es como un break del switch..


                string campo = cbCampo.SelectedItem.ToString();
                    string criterio = cbCriterio.SelectedItem.ToString();
                    string filtro = txtFiltroAvanzado.Text;

                    dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);
                }

                catch (Exception ex)

                {
                    MessageBox.Show(ex.ToString());

                }
           //}
        }


    } //pr
} //pr
