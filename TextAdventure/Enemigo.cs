using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Enemigo : CombatClass
    {
        public static DatoEnemigo[] eneList = { new DatoEnemigo("Esqueleto", 230, 350, 0, 1f, 200,0,0), new DatoEnemigo("Goblin", 350, 300, 200, 0.7f, 300,0,0), new DatoEnemigo("Zombie", 420, 245, 500, 0.7f, 150,0,0), new DatoEnemigo("Slime",512, 170, 255, 1f,100,0,0)};
        string nombre;
        int level;
        public Enemigo(DatoEnemigo datoEnemigo, int level)
        {
            this.level = level;
            nombre = datoEnemigo.nombre;
            hpM = (int)((datoEnemigo.hpM * level / 100 + 10));
            hp = hpM;
            att = (int)((5 + datoEnemigo.att * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            def = (int)((5 + datoEnemigo.def * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            acc = datoEnemigo.acc;
            speed = (int)((5 + datoEnemigo.speed * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            manaM = (int)((5 + datoEnemigo.manaM * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            mana = manaM;
            attMa = (int)((5 + datoEnemigo.attMa * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
        }

        public string GetName()
        {
            return nombre + " ("+level+")";
        }
        public void SetName(string n)
        {
            nombre = n;
        }
        public int GetLevel()
        {
            return level;
        }

        public class DatoEnemigo
        {
            public string nombre;
            public int hpM, att, def, speed, manaM, attMa;
            public float acc;

            public DatoEnemigo(string nombre, int hpM, int att, int def, float acc, int speed, int manaM, int attMa)
            {
                this.nombre = nombre;
                this.hpM = hpM;
                this.att = att;
                this.def = def;
                this.acc = acc;
                this.speed = speed;
                this.manaM = manaM;
                this.attMa = attMa;
            }
        }
        
    }
}
