namespace Dojo.Mensageria.Model
{
    public class Publisher
    {
        public int getNumber()
        {
            Console.WriteLine("Digite um número:");
            String input = Console.ReadLine();
            return int.Parse(input);
        }

        public static Publisher getPublisher()
        {
            return new Publisher();
        }
    }
}