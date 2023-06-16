using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITetris.Classes
{
    public class AI : Character
    {
        // Constructor for making an entirely new AI.
        public AI(string name, int populationSize, int inputSize) : base(name)
        {
            this.populationSize = populationSize;
            Random rng = new Random();
            seed = rng.Next();

            GeneratePopulation(inputSize);
        }

        // Constructor for making an AI from database information.
        public AI(int id, string name, int seed, Individual[] population, int generationNumber) : base(name)
        {
            this.generationID = id;
            this.seed = seed;
            this.population = population;
            this.generationNumber = generationNumber;
        }

        public int generationID = 0;

        public int generationNumber;
        public Individual[] population;
        public int seed;

        private int populationSize;

        // Generates a full population for the AI..
        public void GeneratePopulation(int inputSize)
        {
            population = new Individual[populationSize];
            generationNumber = 0;

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new Individual(inputSize);

            }
        }

    }
}
