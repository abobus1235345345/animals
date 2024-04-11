using System;
using System.Threading;

class Animal
{
    public int ID { get; set; }
    public int HungerLevel { get; set; }
}

class Field
{
    private Semaphore semaphore;
    private Animal[] animals;
    private int animalsOnField;

    public Field(int maxCapacity)
    {
        semaphore = new Semaphore(maxCapacity, maxCapacity);
        animals = new Animal[8];
    }

    public void Start()
    {
        for (int i = 1; i < animals.Length; i++)
        {
            Animal animal = new Animal { ID = i, HungerLevel = 0 };
            animals[i] = animal;
            Thread t = new Thread(() => FeedAnimal(animal));
            t.Start();
        }
    }

    private void FeedAnimal(Animal animal)
    {
        semaphore.WaitOne();
        Console.WriteLine($"Животное {animal.ID} ест (Уровень сытости: {animal.HungerLevel})");
        Thread.Sleep(2000);
        animal.HungerLevel++;
        Console.WriteLine($"Животное {animal.ID} закончило трапезу (Уровень сытости: {animal.HungerLevel})");
        semaphore.Release();
    }
}

class Program
{
    static void Main()
    {
        Field field = new Field(4);
        field.Start();
        Thread.Sleep(4000);
        Console.WriteLine("Все животные наелись");
        Console.ReadLine();
    }
}
