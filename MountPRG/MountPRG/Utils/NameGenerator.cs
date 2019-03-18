using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class NameGenerator
    {

        private List<string> maleNames;
        private List<string> femaleNames;

        private static NameGenerator INSTANCE;

        public static NameGenerator GetInstance
        {
            get
            {
                if (INSTANCE == null)
                    INSTANCE = new NameGenerator();

                return INSTANCE;
            }
        }

        private NameGenerator()
        {
            maleNames = new List<string>();
            maleNames.Add("Brin");
            maleNames.Add("Nokkir");
            maleNames.Add("Jeko");
            maleNames.Add("Juar");
            maleNames.Add("Brak");
            maleNames.Add("Izgin");
            maleNames.Add("Dhad");
            maleNames.Add("Akrun");
            maleNames.Add("Jalun");
            maleNames.Add("Ruge");

            femaleNames = new List<string>();
            femaleNames.Add("Jesta");
            femaleNames.Add("Jadi");
            femaleNames.Add("Misi");
            femaleNames.Add("Trene");
            femaleNames.Add("Ohra");
            femaleNames.Add("Eza");
            femaleNames.Add("Teendi");
            femaleNames.Add("Aira");
            femaleNames.Add("Izil");
            femaleNames.Add("Avres");
        }

        public string GenerateMaleName()
        {
            return maleNames[MyRandom.Range(0, maleNames.Count - 1)];
        }

        public string GeneraterFemaleName()
        {
            return femaleNames[MyRandom.Range(0, femaleNames.Count - 1)];
        }

    }
}
