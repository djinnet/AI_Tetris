using AITetris.Classes;

namespace AITetris.Extensions;
public static class IndividualExtensions
{
    public static Individual[] GeneratePopulation(this Individual[] population, int inputSize)
    {
        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new Individual(inputSize);
        }
        return population;
    }
}
