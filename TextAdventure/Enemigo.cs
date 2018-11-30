using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Enemigo : CombatClass
    {
        public static DatoEnemigo[] eneList = { new DatoEnemigo("Esqueleto", 115, 200, 0, 100, 0, 0, 0.45f, 0.3f), new DatoEnemigo("Goblin", 125, 150, 200, 150, 0, 0, 0.5f, 0.1f), new DatoEnemigo("Zombie", 300, 120, 200, 150 ,0, 0, 0.3f, 0.1f), new DatoEnemigo("Slime",512 , 85, 255,100 ,0 ,0 , 0, 0.1f)};
        string nombre;
        readonly int level;
        public Enemigo(DatoEnemigo datoEnemigo, int level)
        {
            this.level = level;
            nombre = datoEnemigo.nombre;
            hpM = (int)((datoEnemigo.hpM * level / 100 + 10));
            hp = hpM;
            att = (int)((6 + datoEnemigo.att * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            def = (int)((5 + datoEnemigo.def * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            speed = (int)((5 + datoEnemigo.speed * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            manaM = (int)((5 + datoEnemigo.manaM * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            mana = manaM;
            attMa = (int)((5 + datoEnemigo.attMa * level / 100) * (1 - CustomMath.RandomUnit() * 0.1f));
            avoidPerc = datoEnemigo.avoidPerc;
            hitPerc = datoEnemigo.hitPerc;
        }

        public string GetName()
        {
            return nombre;
        }
        public void SetName(string n)
        {
            nombre = n;
        }
        public int GetLevel()
        {
            return level;
        }

        public void RestoreHealth()
        {
            hp = hpM;
        }

        public class DatoEnemigo
        {
            public string nombre;
            public int hpM, att, def, speed, manaM, attMa;
            public float avoidPerc;
            public float hitPerc;

            public DatoEnemigo(string nombre, int hpM, int att, int def, int speed, int manaM, int attMa, float avoidPerc, float hitPerc)
            {
                this.nombre = nombre;
                this.hpM = hpM;
                this.att = att;
                this.def = def;
                this.speed = speed;
                this.manaM = manaM;
                this.attMa = attMa;
                this.avoidPerc = avoidPerc;
                this.hitPerc = hitPerc;
            }
        }
        
    }
}
