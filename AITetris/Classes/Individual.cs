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
        public Individual(int[] chromosomes, double fitness)
        {
            this.chromosomes = chromosomes;
            this.fitness = fitness;
        }

        public Individual(int length)
        {
            chromosomeLength = length;
            GenerateIndividual();
        }
         
        public int[] chromosomes;
        public double fitness;
        private int chromosomeLength;
        private Random rng = new Random();

        public void GenerateIndividual()
        {
            Debug.WriteLine("New Individual");
            fitness = 0;
            chromosomes = new int[chromosomeLength];

            for (int i = 0; i < chromosomes.Length; i++)
            {
                chromosomes[i] = RandomChromosome();
                //Debug.WriteLine(" - Chromosome: " + chromosomes[i]);
            }
        }

        private int RandomChromosome()
        {
            return rng.Next(-1, 2);
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
