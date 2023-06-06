using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Resources;

namespace AutoBattle.Data
{
    public class CharacterClassInfoData
    {
        //public string paladinClassInfo_ =            
        //    @"{
        //      'characterClass': 1,
        //      'hpModifier': 200.0,
        //      'classDamage': 20.0,
        //      'skills': [
        //        {
        //          'name': 'Defend',
        //          'description': 'take half damage from attacks until character takes action next turn',
        //          'damage': 0.0,
        //          'damageMultiplier': 0.0
        //        }
        //      ]
        //    }";

        //public static CharacterClassInfo[] characterClassesInfo;

        //public static void JsonBuilder()
        //{
        //    //CharacterClassInfo c = new CharacterClassInfo() { };
        //    //c.characterClass = CharacterClass.Paladin;
        //    //c.hpModifier = 200f;
        //    //c.classDamage = 20f;
        //    //CharacterSkills[] cs = new CharacterSkills[1];
        //    //cs[0] = new CharacterSkills() { name = "Defend", description = "take half damage from attacks until character takes action next turn" };
        //    //c.skills = cs;

        //    //string s = Newtonsoft.Json.JsonConvert.SerializeObject(c);

        //}
        //
        public static CharacterClassInfo[] SetupClassesInfo() {
            //read all files inside ClassesData
            //ResourceManager rm = new ResourceManager("ArcherC",
            //                   typeof(CharacterClassInfoData).Assembly);
            //Console.WriteLine(typeof(CharacterClassInfoData).Assembly);
            //Console.WriteLine(rm.GetString("json"));
            //Console.WriteLine(rm.GetString("json"));
            ResourceManager rm = new ResourceManager("Archer",
                               typeof(CharacterClassInfoData).Assembly);
            Console.Write(rm.GetString("prompt"));
            string name = Console.ReadLine();
            Console.WriteLine(rm.GetString("greeting"), name);



            return new CharacterClassInfo[0];
        }
    }
}
