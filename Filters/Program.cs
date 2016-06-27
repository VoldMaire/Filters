using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace Filters
{
    class Program
    {
        static void Main(string[] args)
        {
            //Тестування Робота із класом для комплексних чисел
            //Complex num = new Complex(Convert.ToDouble(Console.ReadLine()),Convert.ToDouble(Console.ReadLine()));
            //Console.WriteLine(num);
            //Console.WriteLine(num.Magnitude);
            //Console.WriteLine(num.Phase);
            Console.WriteLine(Math.Sin(30*Math.PI/180));
            Filters polosovoy = new Filters(5.0, 0.005, 20.0, 0.8, 600.0);
            Console.WriteLine("magn: " + polosovoy.CompFilter.Magnitude + " Re: " + polosovoy.CompFilter.Real + " Im: " + polosovoy.CompFilter.Imaginary);
            Complex x = Complex.Divide(new Complex(-2,1), new Complex(1,-1));
            Console.WriteLine("Divide result: " + x.Real + " + j" + x.Imaginary);
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(polosovoy.GetUout(i));
            }
            double timer = 0.0;
            string path = @"e:\temp\MyTest.txt";
            if (File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path);
                    for (int i = 0; i < 100; i++)
                    {
                        sw.WriteLine(polosovoy.GetUout(timer));
                        timer += 0.01;
                    }  
                sw.Close();
            }

            Console.ReadKey();
            //Кінець роботи із комплексним класом


        }
    }

    public class Filters
    {
        public double UenterMagnitude;///Максимальне значення вхідної напруги(Umax в формулі)
        public double UenterPhase;///Частота струму входу, вводити в радіанах!!!(фі в формулі)
        public double Resistor;///Опір резисторів
        public double Capacitor;///Ємність конденсаторів
        public double Freaquence;//Частота струму(омега в формулі)

        public Complex CompVoltage//Комплексна напруга в алгебраїчній формі
        {
            get
            {
                Complex CompVoltage = new Complex(UenterMagnitude * Math.Sin(UenterPhase), UenterMagnitude * Math.Cos(UenterPhase));
                return CompVoltage;
            }
        }
        
        public Complex CompFilter//Комплексна напруга виходу
        {
            get
            {
                Complex divisor = new Complex(3, Freaquence*Resistor*Capacitor - 1/(Freaquence*Resistor*Capacitor));
                return  Complex.Divide(CompVoltage,divisor);
            }
        }        

        public Filters(double res, double cap, double uMagn, double uPhase, double frequence)
        {
            Resistor = res;
            Capacitor = cap;
            UenterMagnitude = uMagn;
            UenterPhase = uPhase;
            Freaquence = frequence;
        }

        public double GetUout(double time)//функція для виводу миттєвого значення
        {
            return CompFilter.Magnitude * (Math.Sin(Freaquence * time + CompFilter.Phase));
        }
    }
}

