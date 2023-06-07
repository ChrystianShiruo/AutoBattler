using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoBattle.Data
{
    public class StatusEffectData
    {
        //Manual setup because I was taking too long to setup .Resources
        readonly private static List<string> statusEffectJson = new List<string>()
        {
            //Bleed
            //"{\"_Status\":1,\"Name\":\"jonas\",\"Description\":null,\"Duration\":0,\"StatusValue\":0.0,\"Target\":null}"
            @"{""Status"":1,""Name"":""Bleed"",""Description"":""deals damage at the start of each player turn"",""Duration"":3,""StatusValue"":15.0}",
            //@"{""Status"":1,""Name"":null,""Description"":null,""Duration"":0,""StatusValue"":0.0,""Target"":null}",
            //@"{""Status"":1 ""Name"":Bleed,""Description"":deals damage at the start of each player turn,""Duration"":3,""Value"":0.0}",
            //"{\"Name\":null,\"Description\":null,\"Duration\":0,\"Value\":0.0}"
            //Stun
            @"{""Status"":2,""Name"":""Stun"",""Description"":""target passess turn"",""Duration"":1}",

            //Heal
            @"{""Status"":3,""Name"":""Heal"",""Description"":""Heals Ally"",""Duration"":0,""StatusValue"":45.0}"

        };

        public static Dictionary<Status, StatusEffect> StatusEffects = new Dictionary<Status, StatusEffect>();
        private static bool _isInit;


        private static void Init()
        {
            StatusEffects = new Dictionary<Status, StatusEffect>();

            var allStatuses = Assembly.GetAssembly(typeof(StatusEffect)).
                GetTypes().Where(t => typeof(StatusEffect).IsAssignableFrom(t) && t != typeof(StatusEffect) && !t.IsInterface);

            foreach(var statusEffect in allStatuses)
            {
                StatusEffect statusEff = Activator.CreateInstance(statusEffect) as StatusEffect;
                StatusEffects.Add(statusEff._Status, statusEff);
            }
            _isInit = true;

        }




        public static IStatusEffect[] SetupStatusEffects()
        {
            List<IStatusEffect> effects = new List<IStatusEffect>();

            //StatusEffect se = new Bleed();
            //se.Status = Status.Bleed;
            //se.Name = "jonas";
            //Newtonsoft.Json.JsonSerializerSettings sett = new Newtonsoft.Json.JsonSerializerSettings();
            //sett.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
            //string s = Newtonsoft.Json.JsonConvert.SerializeObject(se, sett);

            foreach(string statusEffectJson in statusEffectJson)
            {
                StatusEffect statusEffect = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusEffect>(statusEffectJson);
                effects.Add(statusEffect);
            }

            Init();
            return effects.ToArray();
        }
    }
}
