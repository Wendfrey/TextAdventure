using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    
    class Item
    {
        public static void Ordenar(Item[] array)
        {
            for (int i = 0; i < array.Length-1; i++)
            {
                if (array[i] == null)
                {
                    for (int j = i + 1; j < array.Length; j++)
                    {
                        if (array[j] != null)
                        {
                            array[i] = array[j];
                            array[j] = null;
                            j = array.Length;
                        }
                    }
                }
            }
        }

        protected string name;
        public Item(string name)
        {
            this.name = name;
        }


        public string GetName()
        {
            return name;
        }
    }
}
