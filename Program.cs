using System;
using System.Linq;


namespace ConvertToSingle32
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nProszę wpisać liczbę rzeczywistą:");

                decimal liczba_rzeczywista = decimal.Parse((Console.ReadLine()));


                metoda1(liczba_rzeczywista);
                metoda2(liczba_rzeczywista);

                //lub po prostu Single liczba_rzeczywista = Single.Parse((Console.ReadLine()));
            }

        }

        static void metoda1(decimal wczytana_liczba)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("================Metoda 1================");
            decimal liczba_rzeczywista = wczytana_liczba;
            int czesc_calkowita = Math.Abs((int)liczba_rzeczywista);
            Console.WriteLine("Wczytana liczba: " + liczba_rzeczywista);
            // znak 1bit
            int Sign, sign;

            if (liczba_rzeczywista >= 0) { Sign = 0; } else { Sign = 1; }
            Console.WriteLine("1 Bit- znak (0-dodatnia, 1-ujemna): " + Sign);

            if (Sign == 0) { sign = 1; } else { sign = -1; };

            //2. Wyznaczenie wykładnika
            int exponent;
            int pom;
            int n = czesc_calkowita;
            decimal z = liczba_rzeczywista;
            int fract_bit1;


            if (czesc_calkowita == 0)
            {
                fract_bit1 = 0;
                exponent = 0;
     
                while (fract_bit1 < 1)
                {
                    z *= 2;
                    fract_bit1 = (int)z;

                    exponent = exponent - 1;
                }
            }
            else
            {
                exponent = -1;

                while (n > 0)
                {
                    pom = n % 2;
                    n = n /= 2;
                    exponent = exponent + 1;
                }
            }
            
            int bias = 127;

            int Exponent = exponent + bias;

            Console.WriteLine("exponent: " + exponent);
            Console.WriteLine("(exponent + bias)(10): " + Exponent);

            int i = 0;
            n = Exponent;
            int[] Exponent_binarny = new int[8];
            while (n > 0){
                    pom = n % 2;
                    n = n /= 2;
                    Exponent_binarny[i] = pom;
                    i++;
            }
            
            Array.Reverse(Exponent_binarny);
            Console.WriteLine($"(exponent + bias)(2):  {string.Join("", Exponent_binarny)}");

            ///3.Wyznaczenie mantysy
            decimal mantissa = (Math.Abs(liczba_rzeczywista) / (decimal)Math.Pow(2, exponent)) - 1;
            Console.WriteLine("Mantysa(10): " + mantissa);


            ///2.Zamiana mantysy na binarny
            decimal multiply_2 = mantissa;
            int fract_bit;
            int[] mantissa_binarny = new int[24];
            for (i = 0; i < 24; i++)
            {
                multiply_2 *= 2;
                fract_bit = (int)multiply_2;

                if (fract_bit == 1)
                {
                    multiply_2 -= fract_bit;
                    mantissa_binarny[i] = 1;
                }
                else
                {
                    mantissa_binarny[i] = 0;
                }

                //jeśli ostatni 24-bit to 1, dodaje 1 do pozostałego 23-bitowego binarnego, aby zaokrąglić liczbę 
                if (mantissa_binarny[23] == 1)
                {
                    mantissa_binarny[22] = 1;
                }
            }
            Console.WriteLine($"Mantysa(2):  {string.Join("", mantissa_binarny.Take(23))}");

            Console.WriteLine("=======Single IEEE754 32bit========");
            Console.WriteLine($" {Sign} {string.Join("", Exponent_binarny)} { string.Join("", mantissa_binarny.Take(23))}");


            string mantisa_string = string.Join("", mantissa_binarny.Take(23));

            int k = 1;
            decimal binarny_do_dec = 0;
            foreach (char c in mantisa_string)
            {
                binarny_do_dec = binarny_do_dec + Int32.Parse(c.ToString()) * (decimal)Math.Pow(0.5, k);
                k++;
            }
            Console.WriteLine($"Mantysa(10):  {binarny_do_dec}");


            float ConvertedTosingle = sign * (1 + (float)binarny_do_dec) * (float)Math.Pow(2, exponent);
            Console.WriteLine($"Wartość Single 32bit IEEE754:  {ConvertedTosingle}");
        }
        static void metoda2(decimal wczytana_liczba) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================Metoda 2================");
            decimal liczba_rzeczywista = wczytana_liczba;
            Console.WriteLine("Wczytana liczba: " + liczba_rzeczywista);
            Single ieee754_single = decimal.ToSingle(liczba_rzeczywista);
            Console.WriteLine($"Wartość Single 32bit IEEE754:  {ieee754_single}");
        }
    }
}
