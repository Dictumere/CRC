using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksCRC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите двойчные данные:");
            string inputData = Console.ReadLine(); //вводим данные

            List<byte> result = new List<byte>(); // создаем список
            byte[] CRC4 = { 1, 0, 0, 1, 1 }; //CRC-4-ITU, параметры 
            byte[] data = new byte[inputData.Length + CRC4.Length - 1]; //создаем массив, где прибавляем столько нулей, сколько чисел в порождающем многочлене минус 1
            int idx = CRC4.Length - 1; // показывает какое число после операции сносить ( в делении столбиком, после вычитания)
            for (int i = 0; i < inputData.Length + CRC4.Length - 1; i++) //введеное число мы преобразуем в массив чисел, учитывая, что при обращении к элементу строки по индексу он имеет тип char,
                                                                         //поэтому переводим его в численное значение с помощью метода GetNumericValue и затем явно преоброзовать к типу байт
            {
                if (i < inputData.Length) // i - индекс элемента, если его значение больше чем длина, то заполняем нулями, а если меньше то данными из массива
                    data[i] = (byte)char.GetNumericValue(inputData[i]);
                else
                    data[i] = 0;
            }

            byte[] Step(byte[] num, ref int index) // это все реализация шага вычитания при нахождении CRC кода
            {
                index++;
                List<byte> Res = new List<byte>();
                int counter = 0;
                if (num[0] != 0) result.Add(1);
                else result.Add(0);
                foreach (byte n in num)
                {
                    if (num[0] != 0)
                    {
                        Res.Add((byte)(n ^ CRC4[counter]));
                    }
                    else
                    {
                        Res.Add((byte)(n ^ 0));
                    }
                    counter++;
                }
                for (int i = 0; i < Res.Count; i++)
                {
                    if (i != Res.Count - 1)
                        Res[i] = Res[i + 1];
                    else
                    {
                        if (index < data.Length)
                        {
                            Res[i] = data[index];
                        }
                        else
                        {
                            Res[i] = 0;
                        }
                    }
                }
                return Res.ToArray();
            }
            byte[] startNum = new byte[CRC4.Length]; // реализует вычисления CRC кода
            List<byte> CRC = new List<byte>();
            for (int i = 0; i < CRC4.Length; i++)
            {
                startNum[i] = data[i];
            }
            while (idx <= data.Length)
            {
                startNum = Step(startNum, ref idx);
                //foreach(byte b in startNum)
                //{
                //    Console.Write(b);

                //}
                //Console.WriteLine();
                if (idx == data.Length)
                {
                    Console.Write("CRC code: ");
                    for (int i = 0; i < startNum.Length - 1; i++)
                    {
                        CRC.Add(startNum[i]);
                        Console.Write(startNum[i]);
                    }
                    Console.WriteLine();
                }
                // расшифровка переданных данных 🔽
            }
            byte[] dataWithCRC = new byte[inputData.Length + CRC4.Length - 1]; // проверка входных данных
            idx = CRC4.Length - 1;
            for (int i = 0; i < inputData.Length + CRC4.Length - 1; i++)
            {
                if (i < inputData.Length)
                    data[i] = (byte)char.GetNumericValue(inputData[i]);
                else
                    data[i] = CRC[i - inputData.Length];
            }
            byte[] startNum2 = new byte[CRC4.Length];
            for (int i = 0; i < CRC4.Length; i++)
            {
                startNum2[i] = data[i];
            }
            while (idx <= data.Length)
            {
                startNum2 = Step(startNum2, ref idx);
                if (idx == data.Length)
                {
                    Console.Write("CRC control_sum: ");
                    for (int i = 0; i < startNum2.Length - 1; i++)
                    {
                        Console.Write(startNum2[i]);
                    }
                    Console.WriteLine();
                }
            }
            Console.ReadLine();
        }
    }
}