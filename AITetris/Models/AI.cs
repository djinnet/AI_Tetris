using AITetris.Extensions;
using System;

namespace AITetris.Classes
{
    public class AI : Character
    {
        /// <summary>
        /// Constructor for making an entirely new AI.
        /// </summary>
        /// <param name="name"></param>
        public AI(string name) : base(name) { }

        /// <summary>
        /// Constructor for making an AI from database information.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="seed"></param>
        /// <param name="population"></param>
        /// <param name="generationNumber"></param>
        public AI(int id, string name, int seed, Individual[] population, int generationNumber) : base(name)
        {
            this.GenerationID = id;
            this.Seed = seed;
            this.Population = population;
            this.GenerationNumber = generationNumber;
        }

        /// <summary>
        /// The ID of the generation
        /// </summary>
        public int GenerationID { get; set; } = 0;

        /// <summary>
        /// The generation number
        /// </summary>
        public int GenerationNumber { get; set; }

        /// <summary>
        /// The array of Individual for population
        /// </summary>
        public Individual[]? Population { get; set; } //nullable population

        /// <summary>
        /// The seed of the AI
        /// </summary>
        public int Seed { get; set; }

        /// <summary>
        /// Population size
        /// </summary>
        public int PopulationSize { get; set; }

        /// <summary>
        /// Create an AI with a generated population
        /// </summary>
        /// <param name="name"></param>
        /// <param name="populationSize"></param>
        /// <param name="inputSize"></param>
        /// <returns></returns>
        public static AI Create(string name, int populationSize, int inputSize)
        {
            AI AI = new(name)
            {
                PopulationSize = populationSize
            };
            Random rng = new();
            AI.Seed = rng.Next();

            // Generates a full population for the AI..
            AI.Population = new Individual[populationSize];
            AI.GenerationNumber = 0;
            AI.Population.GeneratePopulation(inputSize);
            return AI;
        }
    }
}
