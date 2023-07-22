using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoRedes
{
    class Equipo
    {

        private int puntos;
        private int id;
        private ArrayList jugadores;
        public Equipo(int id)
        {
            this.id = id;
            this.puntos = 0;
            this.jugadores = new ArrayList();
        }

        public void SumarPuntos(int puntos)
        {
            this.puntos += puntos;
        }

        public void AgregarJugador(Jugador jugador)
        {
            this.jugadores.Add(jugador);
            jugador.AsignarEquipo(this);
        }

    }
}
