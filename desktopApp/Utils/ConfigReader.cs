using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PalcoNet.Utils
{
    public class ConfigReader
    {
        public static String SQLUsername { get; private set; }
        public static String SQLPassword { get; private set; }
        public static String SQLDatabase { get; private set; }
        public static String ConfigFile { get; set; }

        private static void obtenerArchivoConfig()
        {
            ConfigFile = System.IO.Directory.GetParent(Application.StartupPath).Parent.Parent.FullName + "\\TP2C2018 K3522 RAGNAR 20\\src\\ConfigDB.txt";
        }

        public static void leerYCargarParametros()
        {
            obtenerArchivoConfig();

            int contador = 0;
            String linea;

            System.IO.StreamReader file = new System.IO.StreamReader(ConfigFile);

            while ((linea = file.ReadLine()) != null)
            {
                if (contador == 0)
                {
                    SQLUsername = linea;
                }
                else
                {
                    if (contador == 1)
                    {
                        SQLPassword = linea;
                    }
                    else
                    {
                        SQLDatabase = linea;
                    }
                }
                contador++;
            }

            file.Close();

            Console.WriteLine(SQLUsername);
            Console.WriteLine(SQLPassword);
            Console.WriteLine(SQLDatabase);
        }

        public static String connectionString()
        {
            leerYCargarParametros();

            EntityConnectionStringBuilder constructorConeccion = new EntityConnectionStringBuilder();
            constructorConeccion.Provider = "System.Data.SqlClient";
            constructorConeccion.ProviderConnectionString = @"data source=localhost\SQLSERVER2012;initial catalog=" + SQLDatabase + ";persist security info=True;user id=" + SQLUsername + ";password=" + SQLPassword + ";MultipleActiveResultSets=True;App=EntityFramework";
            constructorConeccion.Metadata = "res://*/Model.EntityModel.csdl|res://*/Model.EntityModel.ssdl|res://*/Model.EntityModel.msl"; 
       
            return constructorConeccion.ToString();
        }
    }
}
