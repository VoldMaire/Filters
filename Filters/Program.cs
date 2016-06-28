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
            Filters polosovoy = new Filters(2.0, 0.0005, 20.0, 3, 1000.0);
            Console.WriteLine("magn: " + polosovoy.CompFilter.Magnitude + " Re: " + polosovoy.CompFilter.Real + " Im: " + polosovoy.CompFilter.Imaginary);
            Complex x = Complex.Divide(new Complex(-2,1), new Complex(1,-1));
            Console.WriteLine("Divide result: " + x.Real + " + j" + x.Imaginary);
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(polosovoy.GetUout(i));
            }
            double timer = 0.0;
            string pathpolos = @"e:\temp\Polosovoy.txt";
            string pathzag = @"e:\temp\Zagraditelniy.txt";
            string pathfnch = @"e:\temp\FNch.txt";
            string pathfvch = @"e:\temp\FVch.txt";
            if (File.Exists(pathpolos))
            {
                StreamWriter swp = new StreamWriter(pathpolos);
                StreamWriter swz = new StreamWriter(pathzag);
                StreamWriter swN = new StreamWriter(pathfnch);
                StreamWriter swV = new StreamWriter(pathfvch);
                    for (int i = 0; i < 100; i++)
                    {
                        swp.WriteLine(polosovoy.GetUout(timer));
                        swz.WriteLine(polosovoy.GetUoutZag(timer));
                        swN.WriteLine(polosovoy.GetUoutFnch(timer));
                        swV.WriteLine(polosovoy.GetUoutFvch(timer));
                        timer += 0.01;
                    }  
                swp.Close();
                swz.Close();
                swV.Close();
                swN.Close();
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

        public Complex CompFNCH
        {
            get
            {
                Complex multipl = new Complex(1/Math.Pow(1+Freaquence*Resistor*Capacitor,2), Freaquence*Resistor*Capacitor/(1+Math.Pow(Freaquence*Resistor*Capacitor,2)));
                return Complex.Multiply(CompVoltage, multipl);
            }
        }

        public Complex CompFVCH
        {
            get
            {
                Complex multipl = new Complex(Math.Pow(Freaquence*Resistor*Capacitor,2)/(1+Math.Pow(Freaquence*Resistor*Capacitor,2)), Freaquence*Resistor*Capacitor/(1+Math.Pow(Freaquence*Resistor*Capacitor,2)));
                return Complex.Multiply(CompVoltage, multipl);
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

        public double GetUoutFnch(double time)
        {
            return CompFNCH.Magnitude * (Math.Sin(Freaquence * time + CompFNCH.Phase));
        }

        public double GetUoutFvch(double time)
        {
            return CompFVCH.Magnitude * (Math.Sin(Freaquence * time + CompFVCH.Phase));
        }

        public double GetUoutZag(double time)
        {
            return UenterMagnitude * (Math.Sin(Freaquence * time + UenterPhase)) * (1 - Math.Pow(Freaquence * Resistor * Capacitor, 2)) / (Math.Sqrt(Math.Pow(1 - Math.Pow(Freaquence * Resistor * Capacitor, 2), 2) + 16 * Math.Pow(Freaquence * Resistor * Capacitor, 2)));
        }
    }
}

