using System.Threading.Channels;

namespace BinaryGeneticAlgorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string target = "101010101010101010101010";
            int populationSize = 100;
            int numOfIterations = 100;

            double crossoverProbability = 0.6;
            double mutationProbability = 0.002;

            var population = new List<string>();
            var fitness = new List<double>();
            var gen = new GeneticAlgorithm();

            // Step 1: Generate initial population and fitness values for each chromosome in the population.

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(gen.Generate(target.Length));
            }

            for (int i = 0; i < populationSize; i++)
            {
                fitness.Add(gen.Fitness(population[i], target));
            }

            // Step 2: Run the genetic algorithm number of iteration times (default 100) or until we find a perfect match (fitness = 1.0)
            // Select two chromosomes from our original population.
            // Duplicate these chromosomes: "duplicate" here is more of a conceptual operation ("copy the selected parents into the next generation workflow") than a special C# string-copying operation.

            for (int i = 0; i < numOfIterations; i++)
            {
                var evolutedPopulation = new List<string>();
                var evolutedFitness = new List<double>();

                do
                {
                    var chromosome1 = gen.Select(population, fitness, fitness.Sum());
                    var chromosome2 = gen.Select(population, fitness, fitness.Sum());

                    var crossover = gen.Crossover(chromosome1, chromosome2, crossoverProbability).ToList();

                    var mutation1 = gen.Mutate(crossover[0], mutationProbability);
                    var mutation2 = gen.Mutate(crossover[1], mutationProbability);

                    evolutedPopulation.AddRange(new List<string> { mutation1, mutation2 });
                    evolutedFitness.AddRange(new List<double> { gen.Fitness(mutation1, target), gen.Fitness(mutation2, target) });

                } while (evolutedPopulation.Count < populationSize);

                population = evolutedPopulation;
                fitness = evolutedFitness;
            }

            //string evolution = gen.Run(chromosome => gen.Fitness(chromosome, target), target.Length, crossoverProbability, mutationProbability); // ???
            //string evolution = gen.Select(population, fitness, totalFitness); // ???
            
            // The chromosome with highest fitness should be selected!
            string evolution = population[fitness.IndexOf(fitness.Max())];

            Console.WriteLine($"Target: {target}");
            Console.WriteLine($"Evolution: {evolution}");
            Console.ReadLine();
        }
    }
}
