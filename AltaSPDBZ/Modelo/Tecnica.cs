using System;

namespace AltaSPDBZ.Modelo
{
    public class Tecnica
    {
        public UInt32? IdTecnica { get; set; }
        public String Nombre { get; set; }
        public UInt16 Poder { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}\tNombre: {1}\tPoder: {2}",
                                  IdTecnica, Nombre, Poder);
        }
    }
}
