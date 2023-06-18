using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class Individual
    {
        // Constructor for making an Individual with full data.
        public Individual(int[] chromosomes, double fitness)
        {
            this.Chromosomes = chromosomes;
            this.Fitness = fitness;
        }

        // Constructor for making an Individual with a predetermined length.
        public Individual(int length)
        {
            ChromosomeLength = length;
            Fitness = 0;
            Chromosomes = GenerateIndividual(ChromosomeLength);
        }
         
        public int[] Chromosomes { get; set; }
        public double Fitness { get; set; }
        private int ChromosomeLength { get; set; }
        private Random Rng { get; set; } = new Random();

        // Populates the chromosomes array and makes fitness 0.
        public int[] GenerateIndividual(int length)
        {
            var Chromosomes = new int[length];

            for (int i = 0; i < Chromosomes.Length; i++)
            {
                Chromosomes[i] = RandomChromosome();
            }
            return Chromosomes;
        }

        // Returns -1, 0, or 3.
        public int RandomChromosome()
        {
            return Rng.Next(-1, 2);
        }
    }
}

/*
 Inputs:
  - board.squares (board.gridSize)
  - figure.squares (4)
  - figure.shape [next figure] (4)
  - figure.shape [swap figure] (4)
 
 Outputs:
  - move("left") / move("right") (-1 / 1)
  - move("down") / move("instant") (-1 / 1)
  - rotate() (0/1)
  - swap() (0/1)
 */
