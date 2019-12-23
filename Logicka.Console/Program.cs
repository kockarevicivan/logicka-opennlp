using Logicka.Core.Entities;
using System;
using System.Configuration;

namespace Logicka.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            LHippocampus logicka = new LHippocampus();

            var statement1 = logicka.Submit("My name is Ivan and my friend's name is Luka.");
            var statement2 = logicka.Submit("My name is Ivan and my friend's name is Luka.");

            var begin = DateTime.Now.Ticks;

            for (int i = 0; i < 100000; i++)
            {
                var result = statement1.Equals(statement2);
            }

            var end = DateTime.Now.Ticks;
            var diff = end - begin;

            //logicka.SaveToFile(ConfigurationManager.AppSettings["LODBLocation"] + "english.lodb");
        }
    }
}
