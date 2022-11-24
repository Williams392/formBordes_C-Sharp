using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Examen_T3
{
    // Módulo 4: Clases
    internal class Juego
    {
        public string usuario;
        public string juego;
        public string dia;
        public string partida;
        public string puntos;

        public Juego() { }
        
        public Juego(string usuario, string juego, string dia, string partida, string puntos)
        {
            // Asignando los valores;
            this.usuario = usuario;
            this.juego = juego;
            this.dia = dia;
            this.partida = partida;
            this.puntos = puntos;
            // De esta manera tenemos nuestra CLASS -> Juego construida;
        }

        // Una funcion Para mostrar los DATOS;
        public string[] getData()
        {
            string[] data = new string[5]; // Definiendo el vector de tipo STRINGque estamos declarando de 3 valores;
            data[0] = usuario;
            data[1] = juego;
            data[2] = dia;
            data[3] = partida;
            data[4] = puntos;
            return data;
        }
    }
}
