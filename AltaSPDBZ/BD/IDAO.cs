using AltaSPDBZ.Modelo;
using System.Collections.Generic;

namespace AltaSPDBZ.BD
{
    interface IDAO
    {
        void altaTecnica(Tecnica tecnica);
        List<Tecnica> traerTecnicas();
        void altaLuchador(Luchador luchador);

        List<Luchador> traerLuchadores();

        uint poderTotalPara(Luchador luchador); 
    }
}
