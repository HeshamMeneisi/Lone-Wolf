using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoneWolf
{
    class Node
    {
        private Vector3 value;        
        private List<Node> neighbours = new List<Node>();
        public Node(Vector3 vector3)
        {
            this.value = vector3;
        }

        public Vector3 Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public Node Parent { get; set; }

        internal List<Node> Neighbours
        {
            get
            {
                return neighbours;
            }

            set
            {
                neighbours = value;
            }
        }
    }
}
