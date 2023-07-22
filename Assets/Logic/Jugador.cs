using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoRedes
{
    class Jugador
    {
        private int id;
        private ArrayList fichas;
        private Equipo equipo;
        public Jugador(int id)
        {
            this.id = id;
        }

        public void AgregarFichas(ArrayList fichas)
        {
            this.fichas = fichas;
        }

        public void JugarFicha(Ficha ficha)
        {
            this.fichas.Remove(ficha);
        }

        public void AsignarEquipo(Equipo equipo)
        {
            this.equipo = equipo;
        }
       

    }
}
