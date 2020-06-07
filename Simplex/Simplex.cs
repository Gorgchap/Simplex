namespace Simplex
{
    public class Simplex
    {
        double[,] table; // симплекс-таблица
        readonly int m, n;
        readonly System.Collections.ArrayList basis; // список базисных переменных
        
        public Simplex(double[,] source) // source – симплекс-таблица без базисных переменных
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            table = new double[m, n + m - 1];
            basis = new System.Collections.ArrayList();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                    table[i, j] = (j < n) ? source[i, j] : 0;
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1; // коэффициент 1 перед базисной переменной в строке
                    basis.Add(n + i);
                }
            }
            n = table.GetLength(1);
        }

        public double[,] Calculate(out double[] result, out bool unsolvable)
        {
            int mainCol, mainRow; // ведущие столбец и строка
            while (IsNotEnd())
            {
                mainCol = MainCol();
                mainRow = MainRow(mainCol);
                basis[mainRow] = mainCol;

                double[,] new_table = new double[m, n];
                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;
                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                }
                table = new_table;
            }

            result = new double[n];
            unsolvable = false;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (basis.IndexOf(i + 1) > -1) ? table[basis.IndexOf(i + 1), 0] : 0;
                if (unsolvable)
                    continue;
                else
                    unsolvable = result[i] < 0;
            }
            return table;
        }

        private bool IsNotEnd()
        {
            for (int j = 1; j < n; j++)
                if (table[m - 1, j] < 0)
                    return true;
            return false;
        }

        private int MainCol()
        {
            int mainCol = 1;
            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;
            return mainCol;
        }

        private int MainRow(int mainCol)
        {
            int mainRow = 0;
            for (int i = 0; i < m - 1; i++)
                if (table[i, mainCol] > 0)
                {
                    mainRow = i;
                    break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
                if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                    mainRow = i;
            return mainRow;
        }
    }
}