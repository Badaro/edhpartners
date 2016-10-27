using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication15
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<CommanderPair> pairs = GetCommanderPairs(GetCommanders());
            string[] colorCombinations = pairs.Select(p => p.ColorIdentity.ToString()).Distinct().OrderBy(c => c.Length).ToArray();

            StringBuilder output = new StringBuilder();

            output.AppendLine("<html>");
            output.AppendLine("<head>");
            output.AppendLine("<title>Partner Commander Combinations</title>");
            output.AppendLine("</head>");

            output.AppendLine("<body>");

            foreach(string colorCombination in colorCombinations)
            {
                output.AppendLine("<h2>" + colorCombination + "</h2>");

                foreach(CommanderPair pair in pairs.Where(p => p.ColorIdentity.ToString()==colorCombination))
                {
                    output.Append("<h4>" + pair.Item1.Name + " & " + pair.Item2.Name + "</h4>");

                    output.AppendLine("<div>");
                    output.Append("<img src=\"" + pair.Item1.Image + "\" title=\"" + pair.Item1.Name + "\"/>)");
                    output.Append("<img src=\"" + pair.Item2.Image + "\" title=\"" + pair.Item2.Name + "\"/>)");
                    output.AppendLine("</div>");
                }
            }


            output.AppendLine("<body>");
            output.AppendLine("</html>");

            File.WriteAllText("partners.html", output.ToString());
        }

        public static IEnumerable<CommanderPair> GetCommanderPairs(IEnumerable<Commander> commanders)
        {
            List<CommanderPair> pairs = new List<CommanderPair>();

            // Lazy combination method
            Dictionary<string, object> addedCommanders = new Dictionary<string, object>();

            foreach(Commander item1 in commanders)
            {
                foreach(Commander item2 in commanders)
                {
                    if(item1.Name != item2.Name)
                    {
                        CommanderPair pair = new CommanderPair(item1, item2);
                        string key = pair.GetKey();

                        if (!addedCommanders.ContainsKey(key))
                        {
                            addedCommanders.Add(key, null);
                            pairs.Add(pair);
                        }
                    }
                }
            }

            return (pairs);
        }

        public static IEnumerable<Commander> GetCommanders()
        {
            List<Commander> commanders = new List<Commander>();

            commanders.Add(new Commander("Silas Renn, Seeker Adept", "http://mythicspoiler.com/c16/cards/silasrennseekeradept.jpg", "UB"));
            commanders.Add(new Commander("Vial Smasher the Fierce", "http://mythicspoiler.com/c16/cards/vialsmasherthefierce.jpg", "BR"));
            commanders.Add(new Commander("Tana, the Bloodsower", "http://mythicspoiler.com/c16/cards/tanathebloodsower.jpg", "RG"));
            commanders.Add(new Commander("Sidar Kondo of Jamuraa", "http://mythicspoiler.com/c16/cards/sidarkondoofjamuraa.jpg", "WG"));
            commanders.Add(new Commander("Ishai, Ojutai Dragonspeaker", "http://mythicspoiler.com/c16/cards/ishaiojutaidragonspeaker.jpg", "WU"));

            commanders.Add(new Commander("Bruse Tarl, Boorish Herder", "http://mythicspoiler.com/c16/cards/brusetarlboorishherder.jpg", "WR"));
            commanders.Add(new Commander("Kydele, Chosen of Kruphix", "http://mythicspoiler.com/c16/cards/kydelechosenofkruphix.jpg", "UG"));
            commanders.Add(new Commander("Ravos, Soultender", "http://mythicspoiler.com/c16/cards/ravossoultender.jpg", "WB"));
            commanders.Add(new Commander("Ludevic, Necro-Alchemist", "http://mythicspoiler.com/c16/cards/ludevicnecroalchemist.jpg", "UR"));
            commanders.Add(new Commander("Ikra Shidiqi, the Usurper", "http://mythicspoiler.com/c16/cards/ikrashidiqitheusurper.jpg", "BG"));

            commanders.Add(new Commander("Akiri, Rope Thrower", "http://mythicspoiler.com/c16/cards/akiriropethrower.jpg", "WR"));
            commanders.Add(new Commander("Thrasios, Triton Hero", "http://mythicspoiler.com/c16/cards/thrasiostritonhero.jpg", "UG"));
            commanders.Add(new Commander("Tymna the Weaver", "http://mythicspoiler.com/c16/cards/tymnatheweaver.jpg", "WB"));
            commanders.Add(new Commander("Kraum, Ludevic's Opus", "http://mythicspoiler.com/c16/cards/kraumludevicsopus.jpg", "UR"));
            commanders.Add(new Commander("Reyhan, Last of the Abzan", "http://mythicspoiler.com/c16/cards/reyhanlastoftheabzan.jpg", "BG"));

            return (commanders);
        }
    }

    class CommanderPair
    {
        public Commander Item1 { get; set; }
        public Commander Item2 { get; set; }
        public ColorIdentity ColorIdentity { get; set; }

        public CommanderPair(Commander item1, Commander item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.ColorIdentity = item1.ColorIdentity.Combine(item2.ColorIdentity);
        }

        public string GetKey()
        {
            return String.Join("|", new string[] { this.Item1.Name, this.Item2.Name }.OrderBy(s => s));
        }

        public override string ToString()
        {
            return "[" + this.ColorIdentity.ToString() + "] " + this.Item1.Name + " & " + this.Item2.Name;
        }
    }


    class Commander
    {
        public string Name { get; set; }
        public Uri Image { get; set; }
        public ColorIdentity ColorIdentity { get; set; }

        public Commander(string name, string image, string color)
        {
            this.Name = name;
            this.Image = new Uri(image);
            this.ColorIdentity = new ColorIdentity(color);
        }

        public override string ToString()
        {
            return "[" + this.ColorIdentity.ToString() + "] " + this.Name;
        }
    }

    class ColorIdentity
    {
        public bool W { get; set; }
        public bool U { get; set; }
        public bool B { get; set; }
        public bool R { get; set; }
        public bool G { get; set; }

        private ColorIdentity()
        {
        }

        public ColorIdentity(string colorIdentity)
        {
            if (colorIdentity.Contains("W")) this.W = true;
            if (colorIdentity.Contains("U")) this.U = true;
            if (colorIdentity.Contains("B")) this.B = true;
            if (colorIdentity.Contains("R")) this.R = true;
            if (colorIdentity.Contains("G")) this.G = true;
        }

        public ColorIdentity Combine(ColorIdentity secondIdentity)
        {
            return new ColorIdentity()
            {
                W = this.W || secondIdentity.W,
                U = this.U || secondIdentity.U,
                B = this.B || secondIdentity.B,
                R = this.R || secondIdentity.R,
                G = this.G || secondIdentity.G
            };
        }

        public override string ToString()
        {
            string result = "";

            if (this.W) result += "W";
            if (this.U) result += "U";
            if (this.B) result += "B";
            if (this.R) result += "R";
            if (this.G) result += "G";

            return result;
        }
    }
}
