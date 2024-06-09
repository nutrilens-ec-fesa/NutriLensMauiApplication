using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriLensClassLibrary.Models
{
    public class AnvisaLimits
    {
        public double Carboidratos { get; set; }

        public double Proteinas { get; set; }

        public double GordurasTotais { get; set; }

        public double GordurasSaturadas { get; set; }

        public double FibraAlimentar { get; set; }

        public double Sodio {  get; set; }

        public double Colesterol { get; set; }

        public double Calcio { get; set; }

        public double Ferro { get; set; }

        public static AnvisaLimits GetAnvisaLimits()
        {
            var limits = new AnvisaLimits();

            limits.Carboidratos = 300;      //g
            limits.Proteinas = 75;          //g
            limits.GordurasTotais = 55;     //g
            limits.GordurasSaturadas = 22;  //g
            limits.FibraAlimentar = 25;     //g
            limits.Sodio = 2400;            //mg
            limits.Colesterol = 300;        //mg
            limits.Calcio = 1000;           //mg
            limits.Ferro = 14;              //mg

            return limits;

        }
    }
}
