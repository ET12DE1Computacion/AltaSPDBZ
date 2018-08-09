using System;
using AltaSPDBZ.BD;
using AltaSPDBZ.Modelo;
using System.Collections.Generic;

namespace AltaSPDBZ
{
    class Program
    {
        static void Main(string[] args)
        {
            IDAO dao = ingresoBDMySQL();
            menuConsola(dao);
        }

        static void menuConsola(IDAO dao)
        {
            Opciones opcion;
            do
            {
                mostrarMenu();
                opcion = leerOpcion();
                Console.Clear();
                delegarSubMenu(dao, opcion);
            } while (opcion != Opciones.Salir);
            Console.WriteLine("Adios, que la fuerza te acompañe");
            Console.ReadLine();
        }

        private static void mostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("1) Alta Luchador");
            Console.WriteLine("2) Alta Tecnica");
            Console.WriteLine("3) Ver Tecnicas");
            Console.WriteLine("4) Ver poder total (calculo por DAO)");
            Console.WriteLine("5) Salir");
            Console.Write("\nSeleccionar opción: ");
        }

        private static void delegarSubMenu(IDAO dao, Opciones opcion)
        {
            switch (opcion)
            {
                case Opciones.AltaLuchador:
                    dao.altaLuchador(promptLuchador());
                    break;
                case Opciones.AltaTecnica:
                    dao.altaTecnica(promptTecnica());
                    break;
                case Opciones.VerTecnicas:
                    imprimirTecnicas(dao.traerTecnicas());
                    break;
                case Opciones.VerPoderTotal:
                    promptLuchadorPoderTotal(dao);
                    break;
                case Opciones.Salir:
                    break;
                default:
                    break;
            }
        }

        private static Opciones leerOpcion()
        {
            return (Opciones)(Convert.ToByte(Console.ReadLine()));
        }

        static DAOMySQL ingresoBDMySQL()
        {
            string host = prompt("Ingrese Host: ");
            string db = "agbd5to_dbz";
            string user = prompt("Ingrese Usuario: ");
            string pass = prompt("Ingrese contraseña: ");
            string conexion = string.Format("Server={0};Database={1};Uid={2};Pwd={3};"
                                                   , host, db, user, pass);
            return new DAOMySQL(conexion);
        }

        static string prompt(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine();
        }

        static Tecnica promptTecnica()
        {
            Tecnica tecnica = new Tecnica();
            tecnica.Nombre = prompt("Ingrese nombre tecnica: ");
            tecnica.Poder = Convert.ToUInt16(prompt("Ingrese poder Tecnica: "));
            return tecnica;
        }

        static Luchador promptLuchador()
        {
            Luchador luchador = new Luchador();
            luchador.Nombre = prompt("Ingrese nombre luchador: ");
            luchador.PoderDePelea = Convert.ToUInt32(prompt("Ingrese Poder de Pelea: "));
            return luchador;
        }

        static void imprimirTecnicas(List<Tecnica> tecnicas)
        {
            Console.WriteLine("Tecnicas");
            tecnicas.ForEach(tecnica=>Console.WriteLine(tecnica));
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }
        static void imprimirLuchadores(List<Luchador> luchadores)
        {
            Console.WriteLine("Luchadores");
            luchadores.ForEach(luchador => Console.WriteLine(luchador));
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }

        static void imprimirListaDe<T>(List<T> lista)
        {
            lista.ForEach(elemento => Console.WriteLine(elemento));
        }        

        static void promptLuchadorPoderTotal(IDAO dao)
        {
            Console.WriteLine("Seleccione un luchador: ");
            var luchadores = dao.traerLuchadores();
            imprimirListaDe<Luchador>(luchadores);
            int indiceLuchador = Convert.ToInt32(prompt("Ingrese orden luchador: "));
            UInt32 poderTotal = dao.poderTotalPara(luchadores[indiceLuchador]);
            Console.WriteLine();
            Console.WriteLine("{0}\t\tpoder total: {1}", luchadores[indiceLuchador], poderTotal);
            Console.ReadKey();
        }
    }
}

public enum Opciones
{
    AltaLuchador = 1,
    AltaTecnica,
    VerTecnicas,
    VerPoderTotal,
    Salir
}
