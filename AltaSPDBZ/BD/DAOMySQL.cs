using AltaSPDBZ.Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AltaSPDBZ.BD
{
    class DAOMySQL:IDAO
    {
        MySqlConnection conexion;
        MySqlCommand comando;
        MySqlDataAdapter adaptador;

        public DAOMySQL(string cadenaConexion)
        {
            conexion = new MySqlConnection(cadenaConexion);            
        }

        public void altaTecnica(Tecnica tecnica)
        {
            instanciarComando("altaTecnica");

            cargarParametro("unNombre", MySqlDbType.VarChar, 45, tecnica.Nombre);
            cargarParametro("unPoder", MySqlDbType.UInt16, tecnica.Poder);

            ejecutarComando();
        }

        private void cargarParametro(string nombre, MySqlDbType tipoDb, object valor)
        {
            MySqlParameter parametro = new MySqlParameter(nombre, valor);
            parametro.MySqlDbType = tipoDb;
            comando.Parameters.Add(parametro);
        }

        private void setearParametroComoRetorno(string nombre)
        {
            comando.Parameters[nombre].Direction = ParameterDirection.ReturnValue;
        }

        private void cargarParametro(string nombre, MySqlDbType tipoDb, int longitud, object valor)
        {
            cargarParametro(nombre, tipoDb, valor);
            comando.Parameters[nombre].Size = longitud;
        }        

        public List<Tecnica> traerTecnicas()
        {
            List<Tecnica> tecnicas;
            DataTable tabla = traerTablaDeQuery("SELECT * FROM Tecnica");
            tecnicas = tablaTecnicasALista(tabla);
            return tecnicas;
        }

        private DataTable traerTablaDeQuery(string query)
        {
            //Instancio una tabla
            DataTable tabla = new DataTable();
            //Reinstancio el adaptador, pasando como parametros
            //al constructor, la query y la conexion
            adaptador = new MySqlDataAdapter(query, conexion);
            //'le pido' al adaptador, que complete la tabla
            adaptador.Fill(tabla);
            return tabla;
        }

        public List<Luchador> traerLuchadores()
        {
            List<Luchador> luchadores;
            DataTable tabla = traerTablaDeQuery("Select * from Luchador");
            luchadores = tablaLuchadorALista(tabla);
            cargarLuchadoresConTecnicas(luchadores);
            return luchadores;
        }

        private List<Tecnica> tablaTecnicasALista(DataTable tabla)
        {
            List<Tecnica> tecnicas = new List<Tecnica>();
            Tecnica tecnica;
            DataRowCollection filas = tabla.Rows;

            for (int i = 0; i < filas.Count; i++)
            {
                tecnica = obtenerTecnicaDeFila(filas[i]);
                tecnicas.Add(tecnica);
            }
            return tecnicas;
        }

        private List<Luchador> tablaLuchadorALista(DataTable tabla)
        {
            List<Luchador> luchadores = new List<Luchador>();
            Luchador luchador;
            DataRowCollection filas = tabla.Rows;
            for (int i = 0; i < filas.Count; i++)
            {
                luchador = obtenerLuchadorDeFila(filas[i]);
                luchadores.Add(luchador);
            }
            return luchadores;
        }

        private delegate T filaA<T>(DataRow fila);
        
        private List<T> tablaA<T>(DataTable tabla, filaA<T> metodoFilaAObjeto)
        {
            List<T> lista = new List<T>();
            T objeto;
            DataRowCollection filas = tabla.Rows;
            for (int i = 0; i < filas.Count; i++)
            {
                objeto = metodoFilaAObjeto(filas[i]);
                lista.Add(objeto);
            }
            return lista;
        }

        private Luchador obtenerLuchadorDeFila(DataRow fila)
        {
            Luchador luchador = new Luchador();
            luchador.Nombre = fila["nombre"].ToString();
            luchador.IdLuchador = Convert.ToUInt32(fila["idLuchador"]);
            luchador.PoderDePelea = Convert.ToUInt32(fila["poderDePelea"]);
            return luchador;
        }

        private Tecnica obtenerTecnicaDeFila(DataRow fila)
        {
            Tecnica tecnica = new Tecnica();
            tecnica.IdTecnica = Convert.ToUInt32(fila["idTecnica"]);
            tecnica.Nombre = fila["nombre"].ToString();
            tecnica.Poder = Convert.ToUInt16(fila["poder"]);
            
            return tecnica;
        }
        public void altaLuchador(Luchador luchador)
        {
            instanciarComando("altaLuchador");

            cargarParametro("unNombre", MySqlDbType.VarChar, 45, luchador.Nombre);
            cargarParametro("poderDePelea", MySqlDbType.UInt24, luchador.PoderDePelea);
            cargarParametro("unIdLuchador", MySqlDbType.UInt32, luchador.IdLuchador);
            setearComoSalida("unIdLuchador");
            ejecutarComando();
        }

        private void instanciarComando(string nombreComando)
        {
            comando = new MySqlCommand(nombreComando, conexion);
            comando.CommandType = CommandType.StoredProcedure;
        }

        private void ejecutarComando()
        {
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                throw e;
            }
            finally
            {
                conexion.Close();
            }
        }

        private void setearComoSalida(string nombreParametro)
        {
            comando.Parameters[nombreParametro].Direction = ParameterDirection.Output;
        }


        public uint poderTotalPara(Luchador luchador)
        {
            instanciarComando("poderTotal");
            cargarParametro("unIdLuchador", MySqlDbType.UInt32, luchador.IdLuchador);
            cargarParametro("retorno", MySqlDbType.UInt32, DBNull.Value);
            setearParametroComoRetorno("retorno");
            ejecutarComando();
            return Convert.ToUInt32(comando.Parameters["retorno"].Value);
        }

        private void cargarLuchadoresConTecnicas(List<Luchador> luchadores)
        {
            var tecnicas = this.traerTecnicas();
            DataTable luchadoresTecnicas =
                   traerTablaDeQuery("SELECT * FROM LuchadorTecnica");
            ensenarTecnicas(luchadoresTecnicas, luchadores, tecnicas);
        }

        private void ensenarTecnicas(DataTable luchadoresTecnicas, List<Luchador> luchadores, List<Tecnica> tecnicas)
        {
            DataRowCollection filas = luchadoresTecnicas.Rows;
            for (int i = 0; i < filas.Count; i++)
            {
                ensenarTecnicaA(filas[i], luchadores, tecnicas);
            }
        }

        private void ensenarTecnicaA(DataRow fila, List<Luchador> luchadores, List<Tecnica> tecnicas)
        {
            Luchador luchador = luchadores.First(l => l.IdLuchador == (uint)fila["idLuchador"]);
            Tecnica tecnica = tecnicas.First(t => t.IdTecnica == (uint)fila["idTecnica"]);
            luchador.Tecnicas.Add(tecnica);
        }
    }
}