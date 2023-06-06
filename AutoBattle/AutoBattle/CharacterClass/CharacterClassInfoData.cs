using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Resources;

namespace AutoBattle.Data
{
    public class CharacterClassInfoData
    {

        readonly public static List<string> ClassesJson = new List<string>() {

            //Paladin
            @"{
              ""characterClass"": 1,
              ""hpModifier"": 200.0,
              ""classDamage"": 20.0,
              ""attackRange"": 1,
              ""skills"": [
                {
                  ""name"": ""Patch up"",
                  ""description"": ""heals self"",
                  ""damage"": 15.0,
                  ""range"": 0,
                  ""cooldown"": 1,
                  ""specialEffect"": 3
                }
              ]
            }",
            //Warrior
            @"{
              ""characterClass"": 2,
              ""hpModifier"": 100.0,
              ""classDamage"": 45.0,
              ""attackRange"": 1,
              ""skills"": [
                {
                  ""name"": ""Knock Down"",
                  ""description"": ""Opponent cannot take any actions next turn"",
                  ""damage"": 0.0,
                  ""range"": 0,
                  ""cooldown"": 1,
                  ""specialEffect"": 2
                }
              ]
            }",
            //Cleric
            @"{
              ""characterClass"": 3,
              ""hpModifier"": 50.0,
              ""classDamage"": 30.0,
              ""attackRange"": 1,
              ""skills"": [
                {
                  ""name"": ""Heal"",
                  ""description"": ""Heal lowest healt ally"",
                  ""damage"": 45.0,
                  ""range"": 3,
                  ""cooldown"": 2,
                  ""specialEffect"": 3
                }
              ]
            }",
            //Archer
            @"{
              ""characterClass"": 4,
              ""hpModifier"": -20.0,
              ""classDamage"": 40.0,
              ""attackRange"": 3,
              ""skills"": [
                {
                  ""name"": ""Bleed"",
                  ""description"": ""Make target bleed for 3 turns. counter resets if applied again"",
                  ""damage"": 15.0,
                  ""range"": 3,
                  ""cooldown"": 1,
                  ""specialEffect"": 1
                }
              ]
            }"

        };


        public static CharacterClassInfo[] SetupClassesInfo()
        {
            List<CharacterClassInfo> infos = new List<CharacterClassInfo>();

            foreach(string classInfoJson in ClassesJson)
            {
                CharacterClassInfo classInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterClassInfo>(classInfoJson);
                infos.Add(classInfo);
            }






            return infos.ToArray();
        }
    }
}
