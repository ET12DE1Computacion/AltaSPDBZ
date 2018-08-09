using System;
using System.Collections.Generic;

namespace AltaSPDBZ.Modelo
{
    class Luchador
    {
        public UInt32 IdLuchador { get; set; }
        public string Nombre { get; set; }
        public UInt32 PoderDePelea { get; set; }
        public List<Tecnica> Tecnicas { get; set; }
        public Luchador()
        {
            Tecnicas = new List<Tecnica>();
        }
        public UInt64 podertotal()
        {
            return PoderDePelea + sumaTecnicas();
        }
        private uint sumaTecnicas()
        {
            uint suma = 0;
            Tecnicas.ForEach(tecnica => suma += tecnica.Poder);
            return suma;
        }

        public override string ToString()
        {
            return string.Format("ID: {0}\t\tNombre: {1}\tTecnicas: {2}", IdLuchador, Nombre, Tecnicas.Count);
        }
    }
}
