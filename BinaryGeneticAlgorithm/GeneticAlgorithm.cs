using System.Drawing;
using System.Text;

public class GeneticAlgorithm
{
    private Random random = new Random();

/// <summary>
/// Generates a random chromosome of a given length. A chromosome is a string of 0s and 1s.
/// </summary>
/// <param name="length"></param>
/// <returns></returns>
    public string Generate(int length)
    {
        var chromosome = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            chromosome.Append(random.Next(0, 2));
        }

        return chromosome.ToString();
    }

    /// <summary>
    /// The select method will take a population and a corresponding list of fitnesses and return chromosome selected with the roulette wheel method.
    /// </summary>
    /// <param name="population"></param>
    /// <param name="fitnesses"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    public string Select(IEnumerable<string> population, IEnumerable<double> fitnesses, double sum = 0)
    {
        double r = Random.Shared.NextDouble() * sum;

        double cumulative = 0;

        for (int i = 0; i < population.Count(); i++)
        {
            cumulative += fitnesses.ToArray()[i];

            if (r <= cumulative)
                return population.ToArray()[i];
        }

        return population.ToArray()[^1];
    }

/// <summary>
/// The Mutate method takes in one chromosome and a probability and return a mutated chromosome. 
/// </summary>
/// <param name="chromosome"></param>
/// <param name="probability"></param>
/// <returns></returns>
    public string Mutate(string chromosome, double probability)
    {

        char[] genes = chromosome.ToCharArray();

        for (int i = 0; i < genes.Length; i++)
        {
            if (random.NextDouble() < probability)          // Random.NextDouble() returns a double in the range 0.0 <= value < 1.0
            {
                genes[i] = genes[i] == '0' ? '1' : '0';
            }
        }

        return new string(genes);
    }

    /// <summary>
    /// The Crossover method takes in two chromosomes and return a crossed-over pair. 
    /// Crossover means: at some random bit along the length of the chromosome, cut off the rest of the chromosome and switch it with the cut off part of the other one. 
    /// Example: we have 01011010 and 11110110 and we crossover at the 3rd bit. This results in 010 10110 and 111 11010.
    /// </summary>
    /// <param name="chromosome1"></param>
    /// <param name="chromosome2"></param>
    /// <returns></returns>
    /* Crossover means: at some random bit along the length of the chromosome, cut off the rest of the chromosome and switch it with the cut off part of the other one. 
       In other words, say we have 01011010 and 11110110 and we crossover at the 3rd bit. This results in 010 10110 and 111 11010.*/
    public IEnumerable<string> Crossover(string chromosome1, string chromosome2, double probability)
    {
        // Assume that the two chromosomes should be of the same length.
        if (chromosome1.Length != chromosome2.Length)
        {
            throw new ArgumentException("Chromosomes must be of the same length");
        }

        /*Generate a random number r in [0, 1).
        If r < probability:
            Choose a random crossover point.
            Create two new children by swapping the tails.
        Else:
            Copy the parents unchanged.    
        */

        if (random.NextDouble() < probability)
        { 
            int crossoverPoint = random.Next(1, chromosome1.Length - 1);

            string cross1 = chromosome1.Substring(0, crossoverPoint);
            string cross2 = chromosome2.Substring(0, crossoverPoint);
            string cross1End = chromosome1.Substring(crossoverPoint);
            string cross2End = chromosome2.Substring(crossoverPoint);

            string crossover1 = cross1 + cross2End;
            string crossover2 = cross2 + cross1End;

            return new string[] { crossover1, crossover2 };
        }
        
        return new string[] { chromosome1, chromosome2 };
    }

    /// <summary>
    /// The Run method takes in a fitness function, the length of the chromosome, the probability of crossover and mutation, and the number of iterations to run. 
    /// It returns the best chromosome found after running the genetic algorithm for the specified number of iterations.
    /// </summary>
    /// <param name="fitness">fitness function that accepts a chromosome and returns the fitness of that chromosome</param>
    /// <param name="length">the length of the chromosomes to generate</param>
    /// <param name="p_c"></param>
    /// <param name="p_m"></param>
    /// <param name="iterations"></param>
    /// <returns></returns>
    public string Run(Func<string, double> fitness, int length, double p_c, double p_m, int iterations = 100)
    {
        string target = "abracadabra";
        int populationSize = 100;
        var population = new List<string>();

        // Step 1: Generate initial population

        for (int i = 0; i < populationSize; i++)
        {
            population.Add(Generate(target.Length));
        }

        // Step 2: Run the genetic algorithm
        // Select two chromosomes from our original population.
        // Duplicate these chromosomes: "duplicate" here is more of a conceptual operation ("copy the selected parents into the next generation workflow") than a special C# string-copying operation.

        //var evolutedPopulation = new List<string>();
        //double totalFitness = fitness.Sum();

        //do
        //{
        //    var chromosome1 = Select(population, fitness, totalFitness);
        //    var chromosome2 = Select(population, fitness, totalFitness);

        //    var crossover = Crossover(chromosome1, chromosome2).ToList();

        //    var mutation1 = Mutate(crossover[0], p_m);
        //    var mutation2 = Mutate(crossover[1], p_m);

        //    evolutedPopulation.AddRange(new List<string> { mutation1, mutation2 });
        //} while (evolutedPopulation.Count < populationSize);
        return string.Empty;
    }

    public double Fitness(string chromosome, string target)
    {
        //string bits = string.Concat(chromosome.Zip(target, (c1, c2) => c1 == c2 ? '0' : '1'));

        // The score of a chromosome is the number of bits that differ from the goal string. Zip produces a sequence of bools, Count(different => different) counts the true (1) values.
        double score = chromosome.Zip(target, (chromosome, target) => chromosome != target).Count(different => different);
        double fitness = 1/(score + 1);

        return fitness;
    }
}