using System.Collections.Generic;


namespace AutoBattle.Data
{
    public class CharacterClassInfoData
    {
        //Manual setup because I was taking too long to setup .Resources
        readonly private static List<string> classesJson = new List<string>() {

            //Paladin
            @"{
              ""Id"": 1,
              ""Name"": ""Paladin"",
              ""HpModifier"": 200.0,
              ""ClassDamage"": 20.0,
              ""AttackRange"": 1,
              ""Skills"": [
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
              ""Id"": 2,
              ""Name"": ""Warrior"",
              ""HpModifier"": 100.0,
              ""ClassDamage"": 45.0,
              ""AttackRange"": 1,
              ""Skills"": [
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
              ""Id"": 3,
              ""Name"": ""Cleric"",
              ""HpModifier"": 50.0,
              ""ClassDamage"": 30.0,
              ""AttackRange"": 1,
              ""Skills"": [
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
              ""Id"": 4,
              ""Name"": ""Archer"",
              ""HpModifier"": -20.0,
              ""ClassDamage"": 40.0,
              ""AttackRange"": 3,
              ""Skills"": [
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

            CharacterClassInfo se = new CharacterClassInfo();//testing and checking generated json
            //se.characterChild = new CharacterArcher();
            Newtonsoft.Json.JsonSerializerSettings serializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
            serializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            serializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            string s = Newtonsoft.Json.JsonConvert.SerializeObject(se, serializerSettings);

            //TODO: Create intermediary class to receive json values and pass onto class, so I can change CharacterClassInfo fields set access back to protected, do the same for StatusEffectData.cs
            foreach(string classInfoJson in classesJson)
            {
                CharacterClassInfo classInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterClassInfo>(classInfoJson/*, serializerSettings*/);
                infos.Add(classInfo);
            }


            return infos.ToArray();
        }
    }
}
