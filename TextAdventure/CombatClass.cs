﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class CombatClass
    {
        protected int hpM;
        protected int hp;
        protected int def;
        protected int att;
        protected int mana;
        protected int manaM;
        protected int attMa;
        protected int speed;
        protected virtual float hitPerc {
            get;
            set;
        }
        protected virtual float avoidPerc
        {
            get;
            set;
        }

        public int ReceiveDamage(int att, int def, bool notRandom = false)
        {
            int damage;
            if (notRandom)
            {
                damage = (int)(Math.Sin(Math.Atan2(att, def)) * att);
            }
            else
            {
                damage = (int)(Math.Sin(Math.Atan2(att, def)) * att * (1 - (CustomMath.RandomIntNumber(20, 0) / 200f)));
            }
            damage = (damage == 0) ? 1 : damage;
            hp -= damage;
            return damage;
        }

        public bool IsDead()
        {
            return hp <= 0;
        }

        public int GetHealth()
        {
            return hp;
        }

        virtual public int GetMHealth()
        {
            return hpM;
        }

        virtual public int GetAtt()
        {
            return att;
        }

        virtual public int GetAttMa()
        {
            return attMa;
        }

        virtual public int GetDef()
        {
            return def;
        }

        virtual public int GetSpeed()
        {
            return speed;
        }

        virtual public int GetManaM()
        {
            return manaM;
        }

        virtual public int GetMana()
        {
            return mana;
        }

        virtual public void SetMana(int newMana)
        {
            mana = newMana;
        }

        virtual public float GetHitPerc()
        {
            return hitPerc;
        }

        virtual public float GetAvoidPerc()
        {
            return avoidPerc;
        }
    }
}
