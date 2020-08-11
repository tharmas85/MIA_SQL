using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace MIA_SQL
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Ejecutar()
        {
            try
            {
                MessageBox.Show("Getting Connection ...");

                string connString = "Data Source=10.204.128.45;Network Library = DBMSSOCN; Initial Catalog = rev_ass_uy;Integrated Security=SSPI;";

                //create instance of database connection
                SqlConnection conn = new SqlConnection(connString);

                MessageBox.Show("Openning Connection ...");

                //open connection
                conn.Open();

                MessageBox.Show("Connection successful!");

                //set stored procedure name
                string spName = @"dbo.[WS_TEST]";

                //define the SqlCommand object
                SqlCommand cmd = new SqlCommand(spName, conn);

                //Set SqlParameter - the employee id parameter value will be set from the command line
                SqlParameter fcInicio = new SqlParameter();
                fcInicio.ParameterName = "@FCInicio";
                fcInicio.SqlDbType = SqlDbType.VarChar;
                fcInicio.Value = dtp_fcinicio.SelectedDate.Value.ToString();

                //Set SqlParameter - the location id parameter value will be set from the command line
                SqlParameter fcFin = new SqlParameter();
                fcFin.ParameterName = "@FCFin";
                fcFin.SqlDbType = SqlDbType.VarChar;
                fcFin.Value = dtp_fcfin.SelectedDate.Value.ToString();

                //add the parameter to the SqlCommand object
                cmd.Parameters.Add(fcInicio);
                cmd.Parameters.Add(fcFin);

                //set the SQLCommand type to StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter result = cmd.Parameters.Add("@RESULTADO", SqlDbType.VarChar);
                result.Size = 8000;
                result.Direction = ParameterDirection.Output;

                //execute the stored procedure                   
                cmd.ExecuteNonQuery();

                string papapa = result.ToString();
                lbl_resultado.Content = (string)cmd.Parameters["@RESULTADO"].Value;

                MessageBox.Show("Stored Procedure successfully executed!");

                //close connection
                conn.Close();
            }
            catch (Exception ex)
            {
                // Configure the message box to be displayed
                string titulo = "Error de conexión";
                string textoError = "Error: " + ex.Message + "\n" + "¿Reintentar conexión?";
                MessageBoxButton msgBoton = MessageBoxButton.YesNo;
                MessageBoxImage icono = MessageBoxImage.Warning;

                // Display message box
                MessageBoxResult respuesta = MessageBox.Show(textoError, titulo, msgBoton, icono);

                // Process message box results
                switch (respuesta)
                {
                    case MessageBoxResult.Yes:
                        // User pressed Yes button
                        Ejecutar();
                        break;
                    case MessageBoxResult.No:
                        // User pressed No button
                        break;
                }
            }
        }

        private void btn_ejecutar_Click(object sender, RoutedEventArgs e)
        {
            Ejecutar();
        }
    }
}
