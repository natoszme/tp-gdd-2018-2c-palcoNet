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
        private String SQLUsername { get; set; }
        private String SQLPassword { get; set; }
        private String SQLDatabase { get; set; }
        private String SQLClient { get; set; }
        public DateTime Fecha { get; private set; }
        private static ConfigReader instancia;
        public String connectionString { get; private set; }

        public static ConfigReader getInstance()
        {
            if (instancia == null)
            {
                instancia = new ConfigReader();
            }
            return instancia;
        }

        private ConfigReader()
        {
            leerYCargarParametrosArchivoConfiguracion();
        }

        private String obtenerArchivoConfig(String nombreArchivo)
        {
            return System.IO.Directory.GetParent(Application.StartupPath).Parent.Parent.FullName + "\\TP2C2018 K3522 RAGNAR 20\\src\\" + nombreArchivo + ".txt";
        }

        public void leerYCargarParametrosArchivoConfiguracion()
        {
            String ConfigFile = obtenerArchivoConfig("Config");

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
                        if (contador == 2)
                        {
                            SQLDatabase = linea;
                        }
                        else
                        {
                            if (contador == 3)
                            {
                                SQLClient = linea;
                            }
                            else
                            {
                                Fecha = DateTime.ParseExact(linea, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }
                    }
                }
                contador++;
            }

            connectionString = generarConnectionString();

            file.Close();
        }

        private String generarConnectionString()
        {
            EntityConnectionStringBuilder constructorConeccion = new EntityConnectionStringBuilder();
            constructorConeccion.Provider = "System.Data.SqlClient";
            constructorConeccion.ProviderConnectionString = @"data source=" + SQLClient + ";initial catalog=" + SQLDatabase + ";persist security info=True;user id=" + SQLUsername + ";password=" + SQLPassword + ";MultipleActiveResultSets=True;App=EntityFramework";
            constructorConeccion.Metadata = "res://*/Model.EntityModel.csdl|res://*/Model.EntityModel.ssdl|res://*/Model.EntityModel.msl"; 
       
            return constructorConeccion.ToString();
        }
    }
}
