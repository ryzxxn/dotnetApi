namespace dotnetApi
{
    public static class GreetFunction
    {
        public static string greet(string name)
        {
            return $"hello {name} how are you!";
        }

        public static int sum(int num1, int num2)
        {
            return num1 + num2;
        }
    }
}