﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetApp.Codigo
{
    internal class PasandoParametros
    {
        public async Task CodigoDelCurso(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args).Build();

            var configuración = host.Services.GetService<IConfiguration>();

            var cadenaConexión = configuración.GetConnectionString("cadenaDeConexion");

            Console.Write("Escriba el nombre de la persona a ingresar a la BD: ");
            var nombre = Console.ReadLine();

            try
            {
                using (SqlConnection conexión = new SqlConnection(cadenaConexión))
                {
                    // Abrimos la conexión
                    conexión.Open();
                    Console.WriteLine("Conexión abierta");
                    var query = @"INSERT INTO Personas (Nombre)
                        VALUES(@nombre)";

                    using (SqlCommand comando = new SqlCommand(query, conexión))
                    {
                        comando.Parameters.Add(new SqlParameter("@nombre", nombre));

                        var filasAfectadas = await comando.ExecuteNonQueryAsync();
                        Console.WriteLine("Filas afectadas: " + filasAfectadas);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("No pudimos abrir la conexión");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Fin");

            await host.RunAsync();
        }
    }
}
