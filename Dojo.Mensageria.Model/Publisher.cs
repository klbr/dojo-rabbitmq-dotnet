namespace Dojo.Mensageria.Model
{
    public class Publisher
    {
        public (int, int) getNumbers()
        {
            Console.WriteLine("Digite um número inicial:");
            string input = Console.ReadLine();
            Console.WriteLine("Digite um número final:");
            string input2 = Console.ReadLine();
            return (int.Parse(input), int.Parse(input2));
        }

        public static Publisher getPublisher()
        {
            return new Publisher();
        }
    }
}