namespace Simplex
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = new System.IO.StreamReader(args.Length > 0 ? args[0] : "01.txt").ReadToEnd();
            int rowsLength = input.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries).Length;
            double[] values = System.Array.ConvertAll(
                input.Split(new char[] { '\r', '\n', '\t' }, System.StringSplitOptions.RemoveEmptyEntries),
                item => System.Convert.ToDouble(item)
            );

            double[,] table = new double[rowsLength, values.Length / rowsLength];
            for (int i = 0; i < table.GetLength(0); i++)
                for (int j = 0; j < table.GetLength(1); j++)
                    table[i, j] = values[i * table.GetLength(1) + j];

            double[,] table_result = new Simplex(table).Calculate(out double[] result, out bool unsolvable);
            if (unsolvable)
                System.Console.WriteLine("Нет допустимых решений.");
            else
            {
                System.Console.WriteLine("Решённая симплекс-таблица:");
                for (int i = 0; i < table_result.GetLength(0); i++)
                    for (int j = 0; j < table_result.GetLength(1); j++)
                        System.Console.Write(table_result[i, j] + (j < table_result.GetLength(1) - 1 ? "\t" : "\r\n"));

                System.Console.Write("\r\nРешение: ");
                for (int i = 1; i < table.GetLength(1); i++)
                    System.Console.Write("X(" + i + ") = " + result[i - 1] + (i < table.GetLength(1) - 1 ? ", " : "."));
            }
            System.Console.WriteLine("\r\nДля завершения нажмите любую клавишу...");
            System.Console.ReadKey();
        }
    }
}
