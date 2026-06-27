Explanation of the concept
Genetic algorithms are a useful tool for machine learning. One simple way to find a solution to a problem that would typically be too difficult to brute force is through algorithms such as these.

For example, say our problem is, given the list [1,2,3,4,5,6,7,8,9,10], find a way to partition the list into two lists such that the sum of one list is 38 and the product of the other list is 210. 
You could of course brute force it, using the fact that you can find the divisors of 210. 
But say you have the list of numbers from 1 to 50! 
That makes it a lot more difficult, and if we are not looking for a sum or product that is actually possible, just close to a given number, it's even harder to do by brute force.

**Genetic algorithms** are based on the idea that you can create a chromosome that represents a potential solution to your problem. 
One way of representing such a **chromosome** is with a binary string of digits. 
In our example, we could represent one chromosome as ***0010010111***, and decide that 0 means that the corresponding number is in the "sum" pile 
and 1 means it is in the "product" pile, so this chromosome gives us 1+2+4+5+7=19 and 3*6*8*9*10=12960. 
Not very close. But the point is that you can easily represent any potential solution as a binary string!

Step 1: Generate a population of random chromosomes

Now, **the key to a genetic algorithm is to generate many chromosomes**, a large population, if you will. 
The **"genetic"** part of the name comes from the fact that we will, in a sense, **let evolution bring our chromosomes as close as possible to our desired solution**. 

So what we do is generate a population of random chromosomes. 
Then we calculate the fitness (значение функции приспособленности) of those chromosomes in whatever way fits the problem. 

In our case, we want to minimize the absolute difference of the sum from the ideal sum and likewise for the product, 
*so one way we can calculate a "score" of how good our estimate is is*: 

sqrt((chromosome sum - ideal sum)^2 + (chromosome product - ideal product)^2). 

We want to, of course, maximize fitness (значение функции приспособленности), so the closer our score is to 0 (that is, the closer the chromosome is to ideal), the bigger the fitness is. So what we can do is 

**let our fitness be 1/(score + 1); **

*a fitness of 0 means it's nowhere near what we want, and a fitness of 1 means we have exactly the right answer!*

Step 2: Evolve the population

**The evolution step involves taking a look at our population and selecting by fitness.** This is done in a few steps:

1. Select two chromosomes from our original population. This is not done purely randomly. 
   This is done using what is called **"roulette wheel selection"**, where the **chances of picking a chromosome are proportional to its fitness!** 
   This means we are more likely to pick out chromosomes that are closer to our answer. 
   *Duplicate these chromosomes*.
  
2. With a probability p_c, a crossover occurs between these two new chromosomes. 
    
    MINE:
    There are actually two separate random decisions in a genetic algorithm.

    Decide whether to perform crossover.
    If crossover happens, choose the crossover point.

    The probability p
    c
	    ​

     refers only to the first decision.

    A typical algorithm looks like this:

    Select two parent chromosomes.

    Generate a random number r in [0, 1).

    If r < p_c:
        Choose a random crossover point.
        Create two new children by swapping the tails.
    Else:
        Copy the parents unchanged.

   That means at some random bit along the length of the chromosome, we cut off the rest of the chromosome and switch it with the cut off part of the other one. 
   In other words, say we have 01011010 and 11110110 and we crossover at the 3rd bit. 
   This results in 010 10110 and 111 11010.
  
3. With a probability p_m, a mutation can occur at every bit along each new chromosome. 
   The mutation rate is typically very small.

4. Add these two new chromosomes into our new population and 
   repeat steps 1-3 until you have a new population the same size as the original one. 
   For obvious reasons, this is easier if you start off with an even sized original population.

Our goal here is to run the evolution process many times. Eventually, all the chromosomes in our population will have a fitness close to 1! When we feel we have done enough runs, that is the time to cut it off, find the chromosome with the highest fitness, and return that as the result.

**Roulette Wheel Selection**

Roulette wheel selection means that every chromosome has a chance to be selected, but fitter chromosomes get a larger chance.

Think of a roulette wheel where each chromosome occupies a slice whose size is proportional to its fitness.

Example population:

Chromosome	            Fitness
0010010111	            0.5
0110110011	            0.3
1110110011	            0.15
0110110011	            0.05

Total fitness: 0.50 + 0.30 + 0.15 + 0.05 = 1.00

So the selection probabilities are:

Chromosome	            Probability

0010010111	            50%
0110110011	            30%
1110110011	            15%
0110110011	            5%

Visually:

|------A------|---B---|-C-|D|
0            0.5     0.8 .95 1.0

To select one chromosome:

Generate a random number between 0 and the total fitness.
Find which interval contains that number.
Return the corresponding chromosome.

For example:

Random = 0.72 falls into B's interval:

A: [0.00, 0.50)
B: [0.50, 0.80)  <- selected
C: [0.80, 0.95)
D: [0.95, 1.00)


**Your task**

We'll keep this task fairly simple. You will be given an outline of a GeneticAlgorithm class with a few methods. 

**The score of a chromosome is the number of bits that differ from the goal string**.

The fitness is calculated as double fitness = 1/(score + 1);

1. The crossover method is self-explanatory: it takes in two chromosomes and return a crossed-over pair. 
2. The mutate method: it takes in one chromosome and a probability and returns a mutated chromosome. 

Conceptually, mutation introduces random changes into a chromosome to maintain diversity in the population and prevent the algorithm from getting stuck in local optima.

If the mutation probability is p, then each gene (bit) in the chromosome has an independent probability p of being flipped:

0 → 1 with probability p
1 → 0 with probability p
unchanged with probability 1 − p

For example, given the chromosome 0010010111 and a mutation probability of 0.05 (5%), each bit has a 5% chance of being flipped.

**Calculate for each bit if it should [not] flip:**

For each bit, generate a uniform random number between 0 and 1 and compare it to the mutation probability p.

Rule: flip the bit if random < p; otherwise leave it unchanged.

Example with p = 0.05 (5% mutation rate):

Bit					Random r				Action
0					0.72					keep
0					0.03					flip → 1
1					0.44					keep
1					0.01					flip → 0

C# Example of a mutation function in C#:
```csharp	

char[] genes = chromosome.ToCharArray();

for (int i = 0; i < genes.Length; i++)
{
    if (random.NextDouble() < mutationProbability)
    {
        genes[i] = genes[i] == '0' ? '1' : '0';
    }
}

string mutatedChromosome = new string(genes);

3. The generate method generates a random chromosome of a given length (use this in your run method to create a population). 

4. The select method will take a population and a corresponding list of fitnesses and return two chromosomes selected with the roulette wheel method. 

5. The run method will take a fitness function that accepts a chromosome and returns the fitness of that chromosome, 
the length of the chromosomes to generate (should be the same length as the goal chromosome),
the crossover and mutation probabilities, and an optional number of iterations (default to 100). 


Make the population size whatever you want; 100 is a good number but anywhere between 50 and 1000 will work just fine (although the bigger, the slower). 

After the iterations are finished, the method returns the chromosome it deemed to be fittest. This fitness function will be preloaded (Helper.Fitness in C#)! 

What the test will do is generate a random binary string of 35 digits (a random Integer with 35 bits for Ruby), and your algorithm must discover that string! 

The fitness will be calculated in a way similar to above, where **the score of a chromosome is the number of bits that differ from the goal string**.

The crossover probability we will use is 0.6 and the mutation probability we will use is 0.002. 

Now, since the chromosome length is small, 100 iterations should be enough to get the correct answer every time. 

The test fixture will run the algorithm 10 times on 10 different goal strings. If not all of them work, then you can try again and you'll probably be fine.

Good luck and have fun!
