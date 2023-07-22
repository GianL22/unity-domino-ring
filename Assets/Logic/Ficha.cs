using System;
using System.Collections.Generic;
using System.Collections;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoRedes
{
    class Ficha
    {
        private Direction[] directions;
        private Dictionary<Direction, int> values;
        private Dictionary<Direction, Ficha> adyacentes;
        public int id;

        public Ficha(int id, Dictionary<Direction, int> values, Direction[] directions)
        {
            this.values = values;
            this.directions = directions;
            this.adyacentes = new Dictionary<Direction, Ficha>();
            this.id = id;
        }

        public int? GetValue(Direction dir)
        {
            if (this.values.ContainsKey(dir))
                return this.values[dir];
            return null;
        }

        public int[] GetValues()
        {
            return this.values.Values.ToArray();
        }

        public Ficha[] GetAdyacentes()
        {
            return this.adyacentes.Values.ToArray();
        }

        public bool HasValue(int value)
        {
            return this.values.ContainsValue(value);
        }

        public Direction[] GetDirections()
        {
            return this.directions;
        }

        public Direction? WhereIsInsertable(Ficha ficha)
        {
            foreach (Direction dir in directions)
                if (!this.adyacentes.ContainsKey(dir))
                {

                    if (ficha.HasValue(this.values[dir]))
                    {
                        return dir;
                    }

                }
            return null;
        }

        public void InsertInDirection(Direction dir, Ficha ficha)
        {
            this.adyacentes.Add(dir, ficha);
        }

        public void Insert(Ficha ficha)
        {
            Direction? dir = this.WhereIsInsertable(ficha);
            if (dir == null) return ;
            this.adyacentes.Add(dir.Value, ficha);
            dir = ficha.WhereIsInsertable(this);
            ficha.InsertInDirection(dir.Value , this);
        }

        public void Imprimir( ArrayList visitados )
        {
            Console.WriteLine("[" + this.values[Direction.Lado1] + " " + this.values[Direction.Lado2] + "]");
            foreach( Direction dir in this.directions )
            {
                if (this.adyacentes.ContainsKey(dir) && !visitados.Contains(adyacentes[dir].id))
                {
                    visitados.Add(adyacentes[dir].id);
                    Console.WriteLine("[" + this.values[Direction.Lado1] + " " + this.values[Direction.Lado2] + "]" + dir);
                    adyacentes[dir].Imprimir( visitados );
                }
            }
        }

        //public void setVecino(D dir, Ficha ficha)
        //{
        //    this.vecinos.Add(dir, ficha);
        //} 

        //public Ficha getVecino(D dir, Ficha ficha)
        //{
        //    return this.vecinos[dir];
        //}
    }
}
