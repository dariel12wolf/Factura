using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Factura
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cmbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cod;
            string nom;
            float precio;
            string aten;

            cod = cmbProducto.SelectedIndex;
            nom = cmbProducto.SelectedItem.ToString();
            precio = cmbProducto.SelectedIndex;
            aten = cmbProducto.SelectedItem.ToString();

            switch (cod)
            {
                case 0: lblCodigo.Text = "0011";break;
                case 1: lblCodigo.Text = "0022";break;
                case 2: lblCodigo.Text = "0033"; break;
                case 3: lblCodigo.Text = "0044"; break;
                default: lblCodigo.Text = "0055";break;
            }

            switch (nom)
            {
                case "Ciclo-Sep/dic": lblNombre.Text = "Ciclo-Sep/Dic"; break;
                case "Ciclo-Adaptacion": lblNombre.Text = "Ciclo-Adaptacion"; break;
                case "Ciclo-Familia": lblNombre.Text = "Ciclo-Adaptacion"; break;
                case "Ciclo-Verano": lblNombre.Text = "Ciclo-Verano"; break;
                default: lblNombre.Text = "Ciclo-Verano"; break;
            }

            switch (precio)
            {
                case 0: lblPrecio.Text = "499";break;
                case 1: lblPrecio.Text = "99";break;
                case 2: lblPrecio.Text = "399"; break;
                case 3: lblPrecio.Text = "350"; break;
                default: lblPrecio.Text = "350";break;
            }
            switch (aten)
            {
                case "Omar": lblAtencion.Text = "Omar"; break;
                case "Martin": lblAtencion.Text = "Martin"; break;
                case "Dante": lblAtencion.Text = "Dante"; break;
                case "Zuleira": lblAtencion.Text = "Zuleira"; break;
                case "Juan": lblAtencion.Text = "Juan"; break;
                case "Ariel": lblAtencion.Text = "Ariel"; break;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DataGridViewRow file = new DataGridViewRow();
            file.CreateCells(dgvLista);

            file.Cells[0].Value = lblCodigo.Text;
            file.Cells[1].Value = lblNombre.Text;
            file.Cells[2].Value = lblPrecio.Text;
            file.Cells[3].Value = txtCantidad.Text;
            file.Cells[4].Value = (float.Parse(lblPrecio.Text) * float.Parse(txtCantidad.Text)).ToString();
            dgvLista.Rows.Add(file);

            lblCodigo.Text = lblNombre.Text = lblPrecio.Text = txtCantidad.Text = "";

            obtenerTotal();


        }

        public void obtenerTotal()
        {
            float costot = 0;
            int contador = 0;

            contador = dgvLista.RowCount;

            for (int i = 0; i < contador; i++)
           {
                costot += float.Parse(dgvLista.Rows[i].Cells[4].Value.ToString());
            }

            lblTotatlPagar.Text = costot.ToString();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try {
                DialogResult rppta = MessageBox.Show("¿Desea eliminar producto?",
                    "Eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (rppta == DialogResult.Yes)
                {
                    dgvLista.Rows.Remove(dgvLista.CurrentRow);
                }
            }
            catch { }
            obtenerTotal();
        }

        private void txtEfectivo_TextChanged(object sender, EventArgs e)
        {
            try {
                lblDevolucion.Text = (float.Parse(txtEfectivo.Text) - float.Parse(lblTotatlPagar.Text)).ToString();

                
            }
            catch { }

            if (txtEfectivo.Text == "")
            {
                lblDevolucion.Text = "";
            }

        }

        private void btnVender_Click(object sender, EventArgs e)
        {
            
            clsFactura.CreaTicket Ticket1 = new clsFactura.CreaTicket();
            Ticket1.TextoCentro("**********************************");
            Ticket1.TextoCentro("NEWTON EN RED"); 
            Ticket1.TextoCentro("**********************************");

            Ticket1.TextoIzquierda("");
            Ticket1.TextoCentro("Factura de Venta"); 
            Ticket1.TextoIzquierda("No Fac:   ");
            Ticket1.TextoIzquierda("Fecha:" + DateTime.Now.ToShortDateString() + " Hora:" + DateTime.Now.ToShortTimeString());
            Ticket1.TextoIzquierda("Le Atendio: ");
            Ticket1.TextoIzquierda("");
            clsFactura.CreaTicket.LineasGuion();

            clsFactura.CreaTicket.EncabezadoVenta();
           clsFactura.CreaTicket.LineasGuion();
            foreach (DataGridViewRow r in dgvLista.Rows)
            {
                // PROD                     //PrECIO                                    CANT                         TOTAL
                Ticket1.AgregaArticulo(r.Cells[1].Value.ToString(), double.Parse(r.Cells[2].Value.ToString()), int.Parse(r.Cells[3].Value.ToString()), double.Parse(r.Cells[4].Value.ToString())); //imprime una linea de descripcion
            }


            clsFactura.CreaTicket.LineasGuion();
            Ticket1.TextoIzquierda(" ");
            Ticket1.AgregaTotales("Total", double.Parse(lblTotatlPagar.Text)); // imprime linea con total
            Ticket1.TextoIzquierda(" ");
            Ticket1.AgregaTotales("Efectivo Entregado:", double.Parse(txtEfectivo.Text));
            Ticket1.AgregaTotales("Efectivo Devuelto:", double.Parse(lblDevolucion.Text));


            // Ticket1.LineasTotales(); // imprime linea 

            Ticket1.TextoIzquierda(" ");
            Ticket1.TextoCentro("**********************************");
            Ticket1.TextoCentro("*     Gracias por preferirnos    *");
            Ticket1.TextoCentro("**********************************");
            Ticket1.TextoIzquierda(" ");
            
            string impresora = "Microsoft Print to PDF";
            Ticket1.ImprimirTiket(impresora);
            MessageBox.Show("Gracias por preferirnos");
            this.Close();
        }

        private void lblDevolucion_TextChanged(object sender, EventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
