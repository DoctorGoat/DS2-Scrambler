using SoulsFormats;
using System.Text;

namespace DS2_Scrambler
{
    // Credit to TKGP for these extensions.
    static class Extensions
    {
        public static T GetRandom<T>(this List<T> list, Random rand)
        {
            return list[rand.Next(list.Count)];
        }

        public static T PopRandom<T>(this List<T> list, Random rand)
        {
            int index = rand.Next(list.Count);
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static void Shuffle<T>(this IList<T> list, Random rand)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

    // TODO: add new scramble type: Scalar, that scales the existing value instead of generating a new value. This will be a selection box in actions. Existing will be called Generated

    public class ParamScrambler
    {
        public Random rand;
        public Regulation regulation;
        public ScramblerData Data;
        public ParamScrambler(Random random, Regulation reg, ScramblerData scramblerData)
        {
            Data = scramblerData;
            rand = random;
            regulation = reg;
        }


        public int GetRandomInt(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (int)result;
        }

        public uint GetRandomUInt(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (uint)result;
        }

        public short GetRandomShort(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (short)result;
        }

        public ushort GetRandomUShort(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (ushort)result;
        }

        public byte GetRandomByte(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (byte)result;
        }

        public sbyte GetRandomSByte(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (sbyte)result;
        }

        public float GetRandomFloat(double min, double max)
        {
            double random_value = SimpleRNG.GetUniform();
            double result = (max * random_value) < min ? min : (max * random_value);

            return (float)result;
        }

        public Regulation Scramble_WeaponAttributes(
            bool c_ItemParam_Weapon_Price,
            bool c_ItemParam_Weapon_Effect,
            bool c_WeaponParam_Weapon_Weight,
            bool c_WeaponParam_Weapon_Durability,
            bool c_ItemParam_Weapon_Animation_Speed,
            bool c_WeaponParam_StatRequirements,
            bool c_WeaponParam_Damage,
            bool c_WeaponReinforceParam_Reinforcement,
            bool c_WeaponParam_StaminaConsumption,
            bool c_WeaponTypeParam_CastSpeed,
            bool c_WeaponTypeParam_BowDistance,
            bool c_ArrowParam_AmmoDamage,
            bool c_WeaponActionCategoryParam_Moveset,
            bool c_Tweak_WeaponParam_RemoveStatRequirements
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;
            List<PARAM.Row> ArmorParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ArmorParam")).ToList()[0].Rows;
            List<PARAM.Row> WeaponParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"WeaponParam")).ToList()[0].Rows;
            List<PARAM.Row> RingParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"RingParam")).ToList()[0].Rows;
            List<PARAM.Row> WeaponReinforceParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"WeaponReinforceParam")).ToList()[0].Rows;
            List<PARAM.Row> ArrowParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ArrowParam")).ToList()[0].Rows;
            List<PARAM.Row> WeaponTypeParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"WeaponTypeParam")).ToList()[0].Rows;
            List<PARAM.Row> WeaponActionCategoryParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"WeaponActionCategoryParam")).ToList()[0].Rows;
            StreamWriter sw = new StreamWriter("weapons.txt");

            if (c_ItemParam_Weapon_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End
                ).ToList();
                sw.WriteLine("!!!   WEAPON_PRICES:   !!!");
                foreach (PARAM.Row row in rows)
                {
                    sw.WriteLine("\n\n" + "Row ID:" + row.ID);

                    sw.WriteLine("Default base: " + row["base_price"].Value);
                    var price = (int)row["base_price"].Value * rand.Next(80, 120) / 100;
                    row["base_price"].Value = price;

                    price = (int)row["sell_price"].Value * rand.Next(80, 120) / 100;
                    sw.WriteLine("Default sale:" + row["sell_price"].Value);
                    sw.WriteLine("Random base price:" + (int)row["base_price"].Value);
                    row["sell_price"].Value = price;
                    sw.WriteLine("Random sell price:" + row["sell_price"].Value);
                }
            }
            if (c_ItemParam_Weapon_Effect)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                    row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                    row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End
                ).ToList();

                List<int> list = new List<int>();

                if (ScramblerData_Params.Static.WeaponParamData.Include_Weapon_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Weapon_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.WeaponParamData.Include_Armor_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Armor_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.WeaponParamData.Include_Ring_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Ring_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.WeaponParamData.Include_Item_Action_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Action_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.WeaponParamData.Include_Item_Soul_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Soul_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.WeaponParamData.Include_Item_Warp_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Warp_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.WeaponParamData.Include_Item_Misc_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Misc_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }

                foreach (PARAM.Row row in rows)
                {
                    if (list.Count > 0)
                    {
                        var result = list[rand.Next(list.Count)];

                        row["speffect_id"].Value = result;
                    }
                }
            }
            if (c_WeaponParam_Weapon_Weight)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                    row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End &&
                    !ScramblerData_Items.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    sw.WriteLine("!!! WEIGHT !!!!");
                    sw.WriteLine(row.ID);
                    var weight = (Single)row["weight"].Value * rand.Next(80, 120) / 100;
                    sw.WriteLine("Original weight: " + row["weight"].Value);
                    sw.WriteLine("New weight:" + weight);
                    row["weight"].Value = weight;
                }
            }
            if (c_WeaponParam_Weapon_Durability)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                    row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End &&
                    !ScramblerData_Items.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    sw.WriteLine("!!! Durability! !!!");
                    var max_durability = (Single)row["max_durability"].Value * rand.Next(80, 120) / 100;
                    sw.WriteLine("ID: " + row.ID + "\nDefault:" + row["max_durability"].Value + "New: ");
                    row["max_durability"].Value = max_durability;
                    Console.Write(row["max_durability"].Value);
                }
            }
            if (c_ItemParam_Weapon_Animation_Speed)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    sw.WriteLine("ID: " + row.ID + " Default aspd: " + row["animation_speed"].Value);
                    var animationspeed = (Single)row["animation_speed"].Value * rand.Next(80, 120) / 100;
                    row["animation_speed"].Value = animationspeed;
                    sw.WriteLine("New speed: " + (Single)row["animation_speed"].Value);
                }
            }
            if (c_WeaponParam_StatRequirements)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                    row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End &&
                    !ScramblerData_Items.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                string[] reqs = { "str_requirement", "dex_requirement", "int_requirement", "fth_requirement" };
                foreach (PARAM.Row row in rows)
                {
                    foreach (string s in reqs)
                    {
                        sw.WriteLine(row.ID + ": ");
                        sw.WriteLine("Starting " + s + ": " + row[s].Value);
                        var requirement = (Int16)row[s].Value * (rand.Next(80, 130)) / 100;
                        sw.WriteLine("Required stat: " + s + ": " + requirement);
                        row[s].Value = requirement;
                        sw.WriteLine(s + ": " + row[s].Value);
                        sw.WriteLine("\n");
                    }
                }
            }
            if (c_WeaponParam_Damage)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                    row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End &&
                    !ScramblerData_Items.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                string[] damage = { "stamina_damage", "damage_mult", "stamina_damage_mult", "durability_damage_mult", "status_effect_amount", "posture_damage_mult", "hitback_radius", "hitback_length" };

                foreach (PARAM.Row row in rows)
                {
                    foreach (string s in damage)
                    {
                        sw.WriteLine(row.ID + " initial values for :" + s + ": " + row[s].Value);
                        var dmg = (Single)row[s].Value * rand.Next(80, 120) / 100;
                        row[s].Value = dmg;
                        sw.WriteLine(row.ID + " final values:" + row[s].Value);
                    }
                }

                // WeaponTypeParam
                rows = WeaponTypeParam;

                foreach (PARAM.Row row in rows)
                {
                    sw.WriteLine("Initial counter damage: " + row.ID + "\n:" + row["counter_damage"].Value);
                    var counterdamage = (Single)row["counter_damage"].Value * rand.Next(50, 150) / 100;
                    row["counter_damage"].Value = counterdamage;
                    sw.WriteLine("Final counter damage: " + row["counter_damage"].Value);
                }
            }
            if (c_WeaponReinforceParam_Reinforcement)
            {
                List<PARAM.Row> rows = WeaponReinforceParam;

                foreach (PARAM.Row row in rows)
                {
                    // Damage
                    // Lots to do here.
                    // And I don't think it works right now. 
                    sw.WriteLine("Row ID" + row.ID);
                    sw.WriteLine("Row name:" + row.Name);

                    string[] damage = { "damage_physical", "damage_magic", "damage_lightning", "damage_dark", "damage_fire", "damage_poison", "damage_bleed" };
                    //Set this to 6 or something to skip this segment.
                    if(rand.Next(0,5) >= 4)
                    {
                        int dmgtype1 = 0;
                        sw.WriteLine(row[damage[dmgtype1]]);
                        var swap = damage[dmgtype1];
                        int dmgtype2 = rand.Next(0, 4);
                        row["max_" + damage[dmgtype1]].Value = row["max_" + damage[dmgtype2]].Value;
                        damage[dmgtype1] = damage[dmgtype2];
                        damage[dmgtype1] = swap;
                        while(rand.Next(0,10) >= 9)
                        {
                            int rnd = rand.Next(0, 4);
                            sw.WriteLine("Jackpot!");
                            var split = (float)row[damage[rnd]].Value * ((rand.NextDouble() + 0.75));
                            row[damage[rnd]].Value = (float)row[damage[rnd]].Value * split;
                            sw.WriteLine(split + "applied to " + damage[rnd] + " this.");
                        }
                        row["max_" + damage[dmgtype1]].Value = row[damage[dmgtype2]].Value;
                    }

                    //This part definitely works. 
                    foreach (string s in damage)
                    {
                        float growth = GetRandomFloat(1.3, 2.5);
                        sw.WriteLine("Random growth: " + growth);
                        sw.WriteLine(s + " default: " + row[s].Value);
                        sw.WriteLine(s + " default max: " + row["max_" + s].Value);
                        var value = (Single)row[s].Value * rand.Next(100, 130) / 100;
                        row[s].Value = value;
                        sw.WriteLine("New value for " + s + ":" + value);
                        row["max_" + s].Value = value * growth;
                        sw.WriteLine("New value for " + "max_" + s + ": " + row["max_" + s].Value);
                    }

                    var stability = Convert.ToSingle(row["stability_0"].Value);
                    sw.WriteLine("Original stability" + stability);

                    // Shields
                    if (row.ID >= 11000)
                    {
                        row["stability_0"].Value = stability;
                        for (int i = 1; i < 10; i++)
                        {
                            sw.WriteLine("Original stability_" + i + ":" + row["stability_" + i].Value);
                            row["stability_" + i].Value = (stability * ((rand.Next(95, 120) / 100) * i));
                            if ((float)row["stability_" + i].Value > 100)
                            {
                                row["stability_" + i].Value = 100;
                            }
                            sw.WriteLine("New stability_" + i + ":" + row["stability_" + i].Value);
                        }
                    }
                    else
                    {
                        for(int i = 0; i <= 10; i++)
                        {
                            row["stability_" + i].Value = stability;
                        }
                    }

                    string[] base_abs = { "absorption_physical", "absorption_magic", "absorption_lightning", "absorption_dark", "absorption_poison", "absorption_bleed", "absorption_petrify", "absorption_curse" };

                    //this is softcoded but it only has one variable and doesn't change
                    var factor = 1.2;

                    foreach (string s in base_abs)
                    {
                        sw.WriteLine("Original absorption for " + s + ": " + row[s].Value);
                        var absorp = (Single)row[s].Value * rand.Next(75, 125) / 100;
                        row[s].Value = absorp;
                        sw.WriteLine("Final absorption for " + s + ": " + row[s].Value);
                    }
                    if (row.ID >= 11000)
                        factor = ScramblerData_Params.Static.WeaponParamData.Shield_Absorption_Factor;
                }
            }
            if (c_WeaponParam_StaminaConsumption)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                    row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End &&
                    !ScramblerData_Items.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["stamina_consumption"].Value = GetRandomFloat(ScramblerData_Params.Static.WeaponParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.WeaponParamData.Stamina_Consumption_Max);
                }
            }
            if (c_WeaponTypeParam_CastSpeed)
            {
                List<PARAM.Row> rows = WeaponTypeParam.Where(row => row.ID >= 300 && row.ID <= 358).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["cast_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.WeaponParamData.Cast_Speed_Min, ScramblerData_Params.Static.WeaponParamData.Cast_Speed_Max);
                }
            }
            if (c_WeaponTypeParam_BowDistance)
            {
                List<PARAM.Row> rows = WeaponTypeParam.Where(row => row.ID >= 360 && row.ID <= 386).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["bow_distance"].Value = GetRandomUShort(ScramblerData_Params.Static.WeaponParamData.Bow_Distance_Min, ScramblerData_Params.Static.WeaponParamData.Bow_Distance_Max);
                }
            }
            if (c_ArrowParam_AmmoDamage)
            {
                List<PARAM.Row> rows = ArrowParam;

                foreach (PARAM.Row row in rows)
                {
                    //TODO: walk over this and make it cascade/have a luck system. 
                    if ((ushort)row["damage_physical"].Value > 0 )
                        row["damage_physical"].Value = GetRandomUShort(ScramblerData_Params.Static.WeaponParamData.Arrow_Physical_Damage_Min, ScramblerData_Params.Static.WeaponParamData.Arrow_Physical_Damage_Max);

                    if ((ushort)row["damage_magic"].Value > 0)
                        row["damage_magic"].Value = GetRandomUShort(ScramblerData_Params.Static.WeaponParamData.Arrow_Magic_Damage_Min, ScramblerData_Params.Static.WeaponParamData.Arrow_Magic_Damage_Max);

                    if ((ushort)row["damage_lightning"].Value > 0)
                        row["damage_lightning"].Value = GetRandomUShort(ScramblerData_Params.Static.WeaponParamData.Arrow_Lightning_Damage_Min, ScramblerData_Params.Static.WeaponParamData.Arrow_Lightning_Damage_Max);

                    if ((ushort)row["damage_fire"].Value > 0)
                        row["damage_fire"].Value = GetRandomUShort(ScramblerData_Params.Static.WeaponParamData.Arrow_Fire_Damage_Min, ScramblerData_Params.Static.WeaponParamData.Arrow_Fire_Damage_Max);

                    if ((ushort)row["damage_dark"].Value > 0)
                        row["damage_dark"].Value = GetRandomUShort(ScramblerData_Params.Static.WeaponParamData.Arrow_Dark_Damage_Min, ScramblerData_Params.Static.WeaponParamData.Arrow_Dark_Damage_Max);

                    if ((byte)row["damage_poison"].Value > 0)
                        row["damage_poison"].Value = GetRandomByte(ScramblerData_Params.Static.WeaponParamData.Arrow_Poison_Damage_Min, ScramblerData_Params.Static.WeaponParamData.Arrow_Poison_Damage_Max);

                    if ((byte)row["damage_bleed"].Value > 0)
                        row["damage_bleed"].Value = GetRandomByte(ScramblerData_Params.Static.WeaponParamData.Arrow_Bleed_Damage_Min, ScramblerData_Params.Static.WeaponParamData.Arrow_Bleed_Damage_Max);
                }
            }
            if (c_WeaponActionCategoryParam_Moveset)
            {
                List<PARAM.Row> rows = WeaponActionCategoryParam;

                // This assigns new values based on the range of values within the existing fields.
                // This is due to the fact that the movesets require similar moves in each slot.
                // Failure to do this causes weird issues to occur.
                foreach (string value in ScramblerData_Core.Static.WeaponActionCategoryFields)
                {
                    AssignRandomMoveset(value, rows);
                }
            }

            // Tweaks
            if (c_Tweak_WeaponParam_RemoveStatRequirements)
            {
                List<PARAM.Row> rows = WeaponParam.Where(row =>
                    row.ID >= ScramblerData_Items.Static.WeaponParam_Category_Start &&
                    row.ID <= ScramblerData_Items.Static.WeaponParam_Category_End &&
                    !ScramblerData_Items.Static.Category_Fists.Contains(row.ID)
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["str_requirement"].Value = 0;
                    row["dex_requirement"].Value = 0;
                    row["int_requirement"].Value = 0;
                    row["fth_requirement"].Value = 0;
                }
            }
            sw.Close();
            return regulation;
        }

        public void AssignRandomMoveset(string field, List<PARAM.Row> rows)
        {
            List<uint> new_list = new List<uint>();

            // Build list of possible values
            foreach (PARAM.Row row in rows)
            {
                foreach (PARAM.Cell cell in row.Cells)
                {
                    if (cell.Def.InternalName == field)
                    {
                        new_list.Add((uint)cell.Value);
                    }
                }
            }

            // Assign new
            foreach (PARAM.Row row in rows)
            {
                foreach (PARAM.Cell cell in row.Cells)
                {
                    if (cell.Def.InternalName == field)
                    {
                        cell.Value = new_list[rand.Next(new_list.Count)];
                    }
                }
            }
        }

        public Regulation Scramble_ArmorAttributes(
            bool c_ItemParam_Armor_Price,
            bool c_ItemParam_Armor_Effect,
            bool c_ArmorParam_Armor_Weight,
            bool c_ArmorParam_Armor_Durability,
            bool c_ArmorParam_Defence,
            bool c_ArmorParam_StatRequirements,
            bool c_ArmorParam_Poise,
            bool c_ArmorReinforceParam_Absorption,
            bool c_Tweak_ArmorParam_RemoveStatRequirements
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;
            List<PARAM.Row> ArmorParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ArmorParam")).ToList()[0].Rows;
            List<PARAM.Row> ArmorReinforceParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ArmorReinforceParam")).ToList()[0].Rows;
            StreamWriter sw = new StreamWriter("armor.txt");
            if (c_ItemParam_Armor_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
               row.ID >= ScramblerData_Items.Static.ArmorParam_Category_Start &&
               row.ID <= ScramblerData_Items.Static.ArmorParam_Category_End
               ).ToList();

                    sw.WriteLine("!!!   ARMOR_PRICES:   !!!");
                    foreach (PARAM.Row row in rows)
                    {
                        sw.WriteLine("\n\n" + "Row ID:" + row.ID);

                        sw.WriteLine("Default base: " + row["base_price"].Value);
                        var price = (int)row["base_price"].Value * rand.Next(80, 120) / 100;
                        row["base_price"].Value = price;

                        price = (int)row["sell_price"].Value * rand.Next(80, 120) / 100;
                        sw.WriteLine("Default sale:" + row["sell_price"].Value);
                        sw.WriteLine("Random base price:" + (int)row["base_price"].Value);
                        row["sell_price"].Value = price;
                        sw.WriteLine("Random sell price:" + row["sell_price"].Value);
                    }
            }
            if (c_ItemParam_Armor_Effect)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                    row.ID >= ScramblerData_Items.Static.ArmorParam_Category_Start &&
                    row.ID <= ScramblerData_Items.Static.ArmorParam_Category_End
                ).ToList();

                List<int> list = new List<int>();

                if (ScramblerData_Params.Static.ArmorParamData.Include_Weapon_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Weapon_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ArmorParamData.Include_Armor_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Armor_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ArmorParamData.Include_Ring_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Ring_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ArmorParamData.Include_Item_Action_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Action_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ArmorParamData.Include_Item_Soul_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Soul_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ArmorParamData.Include_Item_Warp_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Warp_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ArmorParamData.Include_Item_Misc_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Misc_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }

                foreach (PARAM.Row row in rows)
                {
                    if (list.Count > 0)
                    {
                        var result = list[rand.Next(list.Count)];

                        row["speffect_id"].Value = result;
                    }
                }
            }
            if (c_ArmorParam_Armor_Weight)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 1715950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    sw.WriteLine("!!! WEIGHT !!!!");
                    sw.WriteLine(row.ID);
                    var weight = (Single)row["weight"].Value * rand.Next(80, 120) / 100;
                    sw.WriteLine("Original weight: " + row["weight"].Value);
                    sw.WriteLine("New weight:" + weight);
                    row["weight"].Value = weight;
                }
            }
            if (c_ArmorParam_Armor_Durability)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                 row.ID >= 11010100 &&
                 row.ID <= 17950103
                 ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    sw.WriteLine("!!! Durability! !!!");
                    var durability = (Single)row["durability"].Value * rand.Next(80, 120) / 100;
                    sw.WriteLine("ID: " + row.ID + "\nDefault:" + row["durability"].Value);
                    row["durability"].Value = durability;
                    Console.WriteLine("Final:" + row["durability"].Value);
                }
            }
            if (c_ArmorParam_Defence)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    sw.WriteLine("Original armor scaling:" + row["physical_defence_scaling"].Value);
                    var physdefscale = (Single)row["physical_defence_scaling"].Value * rand.Next(90, 110) / 100;
                    row["physical_defence_scaling"].Value = physdefscale;
                    sw.WriteLine("New armor scaling:" + row["physical_defence_scaling"].Value);
                }
            }
            if (c_ArmorParam_StatRequirements)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                string[] reqs = { "strength_requirement", "dexterity_requirement", "intelligence_requirement", "faith_requirement" };
                foreach (PARAM.Row row in rows)
                {
                    foreach (string s in reqs)
                    {
                        sw.WriteLine(row.ID + ": ");
                        sw.WriteLine("Starting " + s + ": " + row[s].Value);
                        var requirement = (Int16)row[s].Value * (rand.Next(80, 130)) / 100;
                        sw.WriteLine("Required stat: " + s + ": " + requirement);
                        row[s].Value = requirement;
                        sw.WriteLine(s + ": " + row[s].Value);
                        sw.WriteLine("\n");
                    }
                }
            }

                if (c_ArmorParam_Poise)
                {                
                    List<PARAM.Row> rows = ArmorParam.Where(row =>
                    row.ID >= 11010100 &&
                    row.ID <= 17950103
                    ).ToList();

                    foreach (PARAM.Row row in rows)
                    {
                        sw.WriteLine("!!! Poise !!!!");
                        sw.WriteLine(row.ID);
                        var weight = (Single)row["poise"].Value * rand.Next(80, 120) / 100;
                        sw.WriteLine("Original poise: " + row["poise"].Value);
                        row["poise"].Value = weight;
                        sw.WriteLine("New poise: " + row["poise"].Value);
                }
            }

            if (c_ArmorReinforceParam_Absorption)
            {
                List<PARAM.Row> rows = ArmorReinforceParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    // Defence
                    var growth = GetRandomFloat(ScramblerData_Params.Static.ArmorParamData.Absorption_Growth_Min, ScramblerData_Params.Static.ArmorParamData.Absorption_Growth_Max);

                    string[] defence = { "_slash", "_thrust", "_strike", "_standard" };
                    string[] absorp = { "_poison", "_petrify", "_bleed", "_curse", "_magic", "_fire", "_dark", "_lightning" };
                    foreach(string s in defence)
                    {
                        sw.WriteLine(s);
                        sw.WriteLine("Original value:" + row["defence" + s].Value);
                        var tempvar = (Single)row["defence" + s].Value * rand.Next(90, 110) / 100;
                        row["defence" + s].Value = tempvar;
                        sw.WriteLine("New value:" + row["defence" + s].Value);
                        sw.WriteLine("Original max value:" + row["max_defence" + s].Value);
                        row["max_defence" + s].Value = tempvar * rand.Next(120, 150) / 100;
                        sw.WriteLine("New max value:" + row["max_defence" + s].Value);
                    }

                    foreach(string s in absorp)
                    {
                        sw.WriteLine(s);
                        sw.WriteLine("Original value:" + row["absorption" + s].Value);
                        var tempvar = (Single)row["absorption" + s].Value * rand.Next(90, 150) / 100;
                        row["absorption" + s].Value = tempvar;
                        sw.WriteLine("New value:" + row["absorption" + s].Value);
                        sw.WriteLine("Original max value:" + row["max_absorption" + s].Value);
                        row["max_absorption" + s].Value = tempvar * rand.Next(90, 130) / 100;
                        sw.WriteLine("New max value:" + row["max_absorption" + s].Value);

                    }
                }
            }

            // Tweaks
            if (c_Tweak_ArmorParam_RemoveStatRequirements)
            {
                List<PARAM.Row> rows = ArmorParam.Where(row =>
                row.ID >= 11010100 &&
                row.ID <= 17950103
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["strength_requirement"].Value = 0;
                    row["dexterity_requirement"].Value = 0;
                    row["intelligence_requirement"].Value = 0;
                    row["faith_requirement"].Value = 0;
                }
            }
            sw.Close();
            return regulation;
        }

        public Regulation Scramble_RingAttributes(
            bool c_ItemParam_Ring_Price,
            bool c_ItemParam_Ring_Effect,
            bool c_RingParam_Ring_Weight,
            bool c_RingParam_Ring_Durability
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;
            List<PARAM.Row> RingParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"RingParam")).ToList()[0].Rows;

            if (c_ItemParam_Ring_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.RingParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.RingParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["base_price"].Value = GetRandomInt(ScramblerData_Params.Static.RingParamData.Base_Price_Min, ScramblerData_Params.Static.RingParamData.Base_Price_Max);
                    row["sell_price"].Value = GetRandomInt(ScramblerData_Params.Static.RingParamData.Sell_Price_Min, ScramblerData_Params.Static.RingParamData.Sell_Price_Max);
                }
            }
            if (c_ItemParam_Ring_Effect)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.RingParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.RingParam_Category_End
                ).ToList();

                List<int> list = new List<int>();

                if (ScramblerData_Params.Static.RingParamData.Include_Weapon_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Weapon_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.RingParamData.Include_Armor_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Armor_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.RingParamData.Include_Ring_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Ring_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.RingParamData.Include_Item_Action_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Action_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.RingParamData.Include_Item_Soul_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Soul_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.RingParamData.Include_Item_Warp_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Warp_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.RingParamData.Include_Item_Misc_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Misc_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }

                foreach (PARAM.Row row in rows)
                {
                    if (list.Count > 0)
                    {
                        var result = list[rand.Next(list.Count)];

                        row["speffect_id"].Value = result;
                    }
                }
            }
            if (c_RingParam_Ring_Weight)
            {
                List<PARAM.Row> rows = RingParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.RingParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.RingParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["weight"].Value = GetRandomFloat(ScramblerData_Params.Static.RingParamData.Weight_Min, ScramblerData_Params.Static.RingParamData.Weight_Max);
                }
            }
            if (c_RingParam_Ring_Durability)
            {
                List<PARAM.Row> rows = RingParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.RingParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.RingParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["durability"].Value = (float)GetRandomInt(ScramblerData_Params.Static.RingParamData.Durability_Min, ScramblerData_Params.Static.RingParamData.Durability_Max);
                }
            }
            
            return regulation;
        }
        public Regulation Scramble_ItemAttributes(
            bool c_ItemParam_Item_Price,
            bool c_ItemParam_Item_Animation_Speed,
            bool c_ItemParam_Item_Max_Hold_Count,
            bool c_ItemParam_Item_Effect
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;

            if (c_ItemParam_Item_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.ItemParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.ItemParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["base_price"].Value = GetRandomInt(ScramblerData_Params.Static.ItemParamData.Base_Price_Min, ScramblerData_Params.Static.ItemParamData.Base_Price_Max);
                    row["sell_price"].Value = GetRandomInt(ScramblerData_Params.Static.ItemParamData.Sell_Price_Min, ScramblerData_Params.Static.ItemParamData.Sell_Price_Max);
                }
            }
            if (c_ItemParam_Item_Animation_Speed)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.ItemParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.ItemParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["animation_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.ItemParamData.Animation_Speed_Min, ScramblerData_Params.Static.ItemParamData.Animation_Speed_Max);
                }
            }
            if (c_ItemParam_Item_Max_Hold_Count)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.ItemParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.ItemParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    if ((ushort)row["max_held_count"].Value > 1)
                        row["max_held_count"].Value = GetRandomUShort(ScramblerData_Params.Static.ItemParamData.Hold_Count_Min, ScramblerData_Params.Static.ItemParamData.Hold_Count_Max);
                }
            }
            if (c_ItemParam_Item_Effect)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.ItemParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.ItemParam_Category_End
                ).ToList();

                List<int> list = new List<int>();

                if (ScramblerData_Params.Static.ItemParamData.Include_Weapon_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Weapon_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ItemParamData.Include_Armor_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Armor_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ItemParamData.Include_Ring_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Ring_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ItemParamData.Include_Item_Action_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Action_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ItemParamData.Include_Item_Soul_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Soul_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ItemParamData.Include_Item_Warp_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Warp_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ItemParamData.Include_Item_Misc_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Misc_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }

                foreach (PARAM.Row row in rows)
                {
                    if (list.Count > 0)
                    {
                        var result = list[rand.Next(list.Count)];

                        row["speffect_id"].Value = result;
                    }

                    if (ScramblerData_Params.Static.ItemParamData.Force_Homeward_Item_Effects == 1)
                    {
                        if (row.ID == 60350000)
                        {
                            row["speffect_id"].Value = 60350000;
                        }
                        if (row.ID == 60355000)
                        {
                            row["speffect_id"].Value = 60355000;
                        }
                        if (row.ID == 60360000)
                        {
                            row["speffect_id"].Value = 60360000;
                        }
                    }
                }
            }
            
            return regulation;
        }

        public Regulation Scramble_SpellAttributes(
            bool c_ItemParam_Spell_Price,
            bool c_SpellParam_StatRequirements,
            bool c_SpellParam_StartupSpeed,
            bool c_SpellParam_CastAnimations,
            bool c_SpellParam_StaminaConsumption,
            bool c_SpellParam_CastSpeed,
            bool c_SpellParam_SlotsUsed,
            bool c_SpellParam_Casts,
            bool c_Tweak_SpellParam_RemoveStatRequirements
        )
        {
            List<PARAM.Row> ItemParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ItemParam")).ToList()[0].Rows;
            List<PARAM.Row> SpellParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"SpellParam")).ToList()[0].Rows;

            if (c_ItemParam_Spell_Price)
            {
                List<PARAM.Row> rows = ItemParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["base_price"].Value = GetRandomInt(ScramblerData_Params.Static.SpellParamData.Base_Price_Min, ScramblerData_Params.Static.SpellParamData.Base_Price_Max);
                    row["sell_price"].Value = GetRandomInt(ScramblerData_Params.Static.SpellParamData.Sell_Price_Min, ScramblerData_Params.Static.SpellParamData.Sell_Price_Max);
                }
            }
            if (c_SpellParam_StatRequirements)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["int_requirement"].Value = GetRandomUShort(ScramblerData_Params.Static.SpellParamData.INT_Requirement_Min, ScramblerData_Params.Static.SpellParamData.INT_Requirement_Max);
                    row["fth_requirement"].Value = GetRandomUShort(ScramblerData_Params.Static.SpellParamData.FTH_Requirement_Min, ScramblerData_Params.Static.SpellParamData.FTH_Requirement_Max);
                }
            }
            if (c_SpellParam_StartupSpeed)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["startup_duration"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Startup_Speed_Min, ScramblerData_Params.Static.SpellParamData.Startup_Speed_Max);
                }
            }
            if (c_SpellParam_CastAnimations)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                // This assigns new values based on the range of values within the existing fields.
                foreach (string value in ScramblerData_Core.Static.SpellCastAnimationFields)
                {
                    AssignRandomCast(value, rows);
                }
            }
            if (c_SpellParam_StaminaConsumption)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["stamina_cost_1h_left"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Max);
                    row["stamina_cost_1h_right"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Max);
                    row["stamina_cost_2h_left"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Max);
                    row["stamina_cost_2h_right"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Max);
                    row["Unk13"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Max);
                    row["Unk18"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Max);
                    row["Unk23"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Max);
                    row["Unk28"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Min, ScramblerData_Params.Static.SpellParamData.Stamina_Consumption_Max);
                }
            }
            if (c_SpellParam_CastSpeed)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["cast_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.SpellParamData.Cast_Speed_Min, ScramblerData_Params.Static.SpellParamData.Cast_Speed_Max);
                }
            }
            if (c_SpellParam_SlotsUsed)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["slots_used"].Value = GetRandomByte(ScramblerData_Params.Static.SpellParamData.Slots_Used_Min, ScramblerData_Params.Static.SpellParamData.Slots_Used_Max);
                }
            }
            if (c_SpellParam_Casts)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    var base_casts = GetRandomByte(ScramblerData_Params.Static.SpellParamData.Base_Casts_Min, ScramblerData_Params.Static.SpellParamData.Base_Casts_Max);
                    var growth = GetRandomByte(ScramblerData_Params.Static.SpellParamData.Cast_Growth_Rate_Min, ScramblerData_Params.Static.SpellParamData.Cast_Growth_Rate_Max);

                    row["casts_tier_1"].Value = (byte)base_casts;
                    row["casts_tier_2"].Value = (byte)(base_casts + growth);
                    row["casts_tier_3"].Value = (byte)(base_casts + (growth * 2));
                    row["casts_tier_4"].Value = (byte)(base_casts + (growth * 3));
                    row["casts_tier_5"].Value = (byte)(base_casts + (growth * 4));
                    row["casts_tier_6"].Value = (byte)(base_casts + (growth * 5));
                    row["casts_tier_7"].Value = (byte)(base_casts + (growth * 6));
                    row["casts_tier_8"].Value = (byte)(base_casts + (growth * 7));
                    row["casts_tier_9"].Value = (byte)(base_casts + (growth * 8));
                    row["casts_tier_10"].Value = (byte)(base_casts + (growth * 9));
                }
            }

            // Tweaks
            if (c_Tweak_SpellParam_RemoveStatRequirements)
            {
                List<PARAM.Row> rows = SpellParam.Where(row =>
                row.ID >= ScramblerData_Items.Static.SpellParam_Category_Start &&
                row.ID <= ScramblerData_Items.Static.SpellParam_Category_End
                ).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["int_requirement"].Value = 0;
                    row["fth_requirement"].Value = 0;
                }
            }

            return regulation;
        }

        public void AssignRandomCast(string field, List<PARAM.Row> rows)
        {
            List<int> new_list = new List<int>();

            // Build list of possible values
            foreach (PARAM.Row row in rows)
            {
                foreach (PARAM.Cell cell in row.Cells)
                {
                    if (cell.Def.InternalName == field)
                    {
                        new_list.Add((int)cell.Value);
                    }
                }
            }

            // Assign new
            foreach (PARAM.Row row in rows)
            {
                foreach (PARAM.Cell cell in row.Cells)
                {
                    if (cell.Def.InternalName == field)
                    {
                        cell.Value = new_list[rand.Next(new_list.Count)];
                    }
                }
            }
        }

        public Regulation Scramble_BulletParams(
            bool c_Bullet_IncludePlayer,
            bool c_Bullet_IncludeEnemy,
            bool c_Bullet_IncludeBoss,
            bool c_Bullet_IncludeTraps,
            bool c_Bullet_VFX,
            bool c_Bullet_Movement,
            bool c_Bullet_Angle,
            bool c_Bullet_SpawnDistance,
            bool c_Bullet_Duration,
            bool c_Bullet_Tracking,
            bool c_Bullet_Effect,
            bool c_Bullet_Count
        )
        {
            List<PARAM.Row> BulletParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"BulletParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyBulletParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyBulletParam")).ToList()[0].Rows;
            List<PARAM.Row> SystemBulletParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"SystemBulletParam")).ToList()[0].Rows;

            if (c_Bullet_IncludePlayer)
            {
                List<PARAM.Row> rows = BulletParam;

                Scramble_Bullets(rows,
                c_Bullet_VFX,
                c_Bullet_Movement,
                c_Bullet_Angle,
                c_Bullet_SpawnDistance,
                c_Bullet_Duration,
                c_Bullet_Tracking,
                c_Bullet_Effect,
                c_Bullet_Count);
            }
            if (c_Bullet_IncludeEnemy)
            {
                List<PARAM.Row> rows = EnemyBulletParam;
                rows = Util.GetRowsFromSubMatch(rows, ScramblerData_Core.Static.Boss_EnemyParamID_List, 2, 4, "1", false);

                Scramble_Bullets(rows,
                c_Bullet_VFX,
                c_Bullet_Movement,
                c_Bullet_Angle,
                c_Bullet_SpawnDistance,
                c_Bullet_Duration,
                c_Bullet_Tracking,
                c_Bullet_Effect,
                c_Bullet_Count);
            }
            if (c_Bullet_IncludeBoss)
            {
                List<PARAM.Row> rows = EnemyBulletParam;
                rows = Util.GetRowsFromSubMatch(rows, ScramblerData_Core.Static.Boss_EnemyParamID_List, 2, 4, "1", true);

                Scramble_Bullets(rows,
                c_Bullet_VFX,
                c_Bullet_Movement,
                c_Bullet_Angle,
                c_Bullet_SpawnDistance,
                c_Bullet_Duration,
                c_Bullet_Tracking,
                c_Bullet_Effect,
                c_Bullet_Count);
            }
            if (c_Bullet_IncludeTraps)
            {
                List<PARAM.Row> rows = SystemBulletParam.Where(row => row.ID <= 220100700).ToList();

                Scramble_Bullets(rows,
                c_Bullet_VFX,
                c_Bullet_Movement,
                c_Bullet_Angle,
                c_Bullet_SpawnDistance,
                c_Bullet_Duration,
                c_Bullet_Tracking,
                c_Bullet_Effect,
                c_Bullet_Count);
            }

            return regulation;
        }

        public void Scramble_Bullets(
            List<PARAM.Row> rows,
            bool c_Bullet_VFX,
            bool c_Bullet_Movement,
            bool c_Bullet_Angle,
            bool c_Bullet_SpawnDistance,
            bool c_Bullet_Duration,
            bool c_Bullet_Tracking,
            bool c_Bullet_Effect,
            bool c_Bullet_Count
        )
        {
            if (c_Bullet_VFX)
            {
                foreach (PARAM.Row row in rows)
                {
                    row["sfx_id"].Value = ScramblerData_Core.Static.FFX_List[rand.Next(ScramblerData_Core.Static.FFX_List.Count)];
                    row["hit_sfx_id"].Value = ScramblerData_Core.Static.FFX_List[rand.Next(ScramblerData_Core.Static.FFX_List.Count)];
                    row["expire_sfx_id"].Value = ScramblerData_Core.Static.FFX_List[rand.Next(ScramblerData_Core.Static.FFX_List.Count)];
                }
            }
            if (c_Bullet_Movement)
            {
                foreach (PARAM.Row row in rows)
                {
                    row["initial_horizontal_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Initial_Horizontal_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Initial_Horizontal_Velocity_Max);
                    row["horizontal_acceleration_start_delay"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Horizontal_Acceleration_Start_Delay_Min, ScramblerData_Params.Static.ProjectileParamData.Horizontal_Acceleration_Start_Delay_Max);
                    row["horizontal_target_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Target_Horizontal_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Target_Horizontal_Velocity_Max);
                    row["horizontal_max_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Max_Horizontal_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Max_Horizontal_Velocity_Max);
                    row["initial_vertical_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Initial_Vertical_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Initial_Vertical_Velocity_Max);
                    row["vertical_acceleration_start_delay"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Vertical_Acceleration_Start_Delay_Min, ScramblerData_Params.Static.ProjectileParamData.Vertical_Acceleration_Start_Delay_Max);
                    row["vertical_target_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Target_Vertical_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Target_Vertical_Velocity_Max);
                    row["vertical_max_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Max_Vertical_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Max_Vertical_Velocity_Max);
                    row["initial_tan_vertical_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Initial_Tan_Vertical_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Initial_Tan_Vertical_Velocity_Max);
                    row["tan_vertical_acceleration_start_delay"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Tan_Vertical_Acceleration_Start_Delay_Min, ScramblerData_Params.Static.ProjectileParamData.Tan_Vertical_Acceleration_Start_Delay_Max);
                    row["tan_vertical_target_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Target_Tan_Vertical_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Target_Tan_Vertical_Velocity_Max);
                    row["tan_vertical_max_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Max_Tan_Vertical_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Max_Tan_Vertical_Velocity_Max);
                    row["initial_tan_horizontal_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Initial_Tan_Horizontal_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Initial_Tan_Horizontal_Velocity_Max);
                    row["tan_horizontal_acceleration_start_delay"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Tan_Horizontal_Acceleration_Start_Delay_Min, ScramblerData_Params.Static.ProjectileParamData.Tan_Horizontal_Acceleration_Start_Delay_Max);
                    row["tan_horizontal_target_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Target_Tan_Horizontal_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Target_Tan_Horizontal_Velocity_Max);
                    row["tan_horizontal_max_velocity"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Max_Tan_Horizontal_Velocity_Min, ScramblerData_Params.Static.ProjectileParamData.Max_Tan_Horizontal_Velocity_Max);
                }
            }
            if (c_Bullet_Angle)
            {
                foreach (PARAM.Row row in rows)
                {
                    row["shooter_horizontal_angle"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Shooter_Horizontal_Angle_Min, ScramblerData_Params.Static.ProjectileParamData.Shooter_Horizontal_Angle_Max);
                    row["shooter_unknown_angle_0"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Shooter_Unk0_Angle_Min, ScramblerData_Params.Static.ProjectileParamData.Shooter_Unk0_Angle_Max);
                    row["shooter_vertical_angle"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Shooter_Vertical_Angle_Min, ScramblerData_Params.Static.ProjectileParamData.Shooter_Vertical_Angle_Max);
                    row["shooter_unknown_angle_1"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Shooter_Unk1_Angle_Min, ScramblerData_Params.Static.ProjectileParamData.Shooter_Unk1_Angle_Max);
                    row["vertical_angle_randomizer"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Vertical_Angle_Randomizer_Min, ScramblerData_Params.Static.ProjectileParamData.Vertical_Angle_Randomizer_Max);
                    row["horizontal_angle_randomizer"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Horizontal_Angle_Randomizer_Min, ScramblerData_Params.Static.ProjectileParamData.Horizontal_Angle_Randomizer_Max);
                    row["vertical_spread"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Vertical_Spread_Min, ScramblerData_Params.Static.ProjectileParamData.Vertical_Spread_Max);
                    row["horizontal_spread"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Horizontal_Spread_Min, ScramblerData_Params.Static.ProjectileParamData.Horizontal_Spread_Max);
                }
            }
            if (c_Bullet_SpawnDistance)
            {
                foreach (PARAM.Row row in rows)
                {
                    row["vertical_spawn_distance"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Vertical_Spawn_Distance_Min, ScramblerData_Params.Static.ProjectileParamData.Vertical_Spawn_Distance_Max);
                    row["unknown_spawn_distance"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Unk_Spawn_Distance_Min, ScramblerData_Params.Static.ProjectileParamData.Unk_Spawn_Distance_Max);
                    row["horizontal_spawn_distance"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Horizontal_Spawn_Distance_Min, ScramblerData_Params.Static.ProjectileParamData.Horizontal_Spawn_Distance_Max);
                }
            }
            if (c_Bullet_Duration)
            {
                foreach (PARAM.Row row in rows)
                {
                    row["max_life"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Max_Life_Min, ScramblerData_Params.Static.ProjectileParamData.Max_Life_Max);
                }
            }
            if (c_Bullet_Tracking)
            {
                foreach (PARAM.Row row in rows)
                {
                    row["max_angle_change"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Max_Angle_Change_Min, ScramblerData_Params.Static.ProjectileParamData.Max_Angle_Change_Max);
                    row["max_tracking_angle_change"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Max_Tracking_Angle_Change_Min, ScramblerData_Params.Static.ProjectileParamData.Max_Tracking_Angle_Change_Max);
                    row["max_tracking_time"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Max_Tracking_Time_Min, ScramblerData_Params.Static.ProjectileParamData.Max_Tracking_Time_Max);
                }
            }
            if (c_Bullet_Effect)
            {
                List<int> list = new List<int>();

                if (ScramblerData_Params.Static.ProjectileParamData.Include_Weapon_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Weapon_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ProjectileParamData.Include_Armor_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Armor_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ProjectileParamData.Include_Ring_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Ring_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ProjectileParamData.Include_Item_Action_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Action_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ProjectileParamData.Include_Item_Soul_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Soul_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ProjectileParamData.Include_Item_Warp_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Warp_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }
                if (ScramblerData_Params.Static.ProjectileParamData.Include_Item_Misc_Effects == 1)
                {
                    foreach (int entry in ScramblerData_Core.Static.Item_Misc_SpEffect_ID_List)
                    {
                        list.Add(entry);
                    }
                }

                foreach (PARAM.Row row in rows)
                {
                    if (list.Count > 0)
                    {
                        var result = list[rand.Next(list.Count)];

                        if (rand.Next(100) <= ScramblerData_Params.Static.ProjectileParamData.Effect_Application_Chance) // Only apply to 25% of bullets
                            row["speffect_on_shoot"].Value = result;
                        else
                            row["speffect_on_shoot"].Value = 0;
                    }
                }
            }
            if (c_Bullet_Count)
            {
                foreach (PARAM.Row row in rows)
                {
                    row["shoot_count"].Value = GetRandomByte(ScramblerData_Params.Static.ProjectileParamData.Shoot_Count_Min, ScramblerData_Params.Static.ProjectileParamData.Shoot_Count_Max);
                    row["shoot_sequential_count"].Value = GetRandomByte(ScramblerData_Params.Static.ProjectileParamData.Shoot_Sequential_Count_Min, ScramblerData_Params.Static.ProjectileParamData.Shoot_Sequential_Count_Max);
                    row["shoot_interval"].Value = GetRandomFloat(ScramblerData_Params.Static.ProjectileParamData.Shoot_Interval_Min, ScramblerData_Params.Static.ProjectileParamData.Shoot_Interval_Max);
                }
            }
        }

        public Regulation Scramble_PlayerParams(
            bool c_PlayerStatusParam_StartingAttributes,
            bool c_PlayerStatusParam_StartingEquipment,
            bool c_PlayerStatusParam_StartingGifts,
            bool c_PlayerLevelUpSoulsParam_LevelupCost,
            bool c_EventCommonParam_ShrineOfWinter_Cost,
            bool c_BossBattleParam_BossSoulDrops,
            bool c_LockOnParam_CameraDistance,
            bool c_LockOnParam_CameraFOV,
            bool c_ChrMoveParam_Walk,
            bool c_ChrMoveParam_Run,
            bool c_ChrMoveParam_Jump,
            bool c_ChrMoveParam_Ladder,
            bool c_ChrMoveParam_Turn,
            bool c_ChrMoveParam_Evasion,
            bool c_Tweak_AnyEquipmentForStartingEquipment,
            bool c_Tweak_BigJumpMode
        )
        {
            List<PARAM.Row> PlayerStatusParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"PlayerStatusParam")).ToList()[0].Rows;
            List<PARAM.Row> PlayerLevelUpSoulsParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"PlayerLevelUpSoulsParam")).ToList()[0].Rows;
            List<PARAM.Row> EventCommonParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EventCommonParam")).ToList()[0].Rows;
            List<PARAM.Row> BossBattleParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"BossBattleParam")).ToList()[0].Rows;
            List<PARAM.Row> LockOnParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"LockOnParam")).ToList()[0].Rows;
            List<PARAM.Row> ChrMoveParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"ChrMoveParam")).ToList()[0].Rows;



            if (c_PlayerStatusParam_StartingAttributes)
            {
                List<PARAM.Row> rows = PlayerStatusParam.Where(row => row.ID >= 20 && row.ID <= 100).ToList();

                foreach (PARAM.Row row in rows)
                {
                    RandomiseClassStats(row);
                }
            }
            if (c_PlayerStatusParam_StartingEquipment)
            {
                List<PARAM.Row> rows = PlayerStatusParam.Where(row => row.ID >= 20 && row.ID <= 100).ToList();

                foreach (PARAM.Row row in rows)
                {
                    RandomiseClassEquipment(row, c_Tweak_AnyEquipmentForStartingEquipment);
                }
            }
            if (c_PlayerStatusParam_StartingGifts)
            {
                List<PARAM.Row> rows = PlayerStatusParam.Where(row => row.ID >= 510 && row.ID <= 570).ToList();

                foreach (PARAM.Row row in rows)
                {
                    RandomiseGift(row);
                }
            }
            if (c_PlayerLevelUpSoulsParam_LevelupCost)
            {
                List<PARAM.Row> rows = PlayerLevelUpSoulsParam;

                var base_min = GetRandomInt(ScramblerData_Params.Static.PlayerParamData.Base_Level_Up_Cost_Min, ScramblerData_Params.Static.PlayerParamData.Base_Level_Up_Cost_Max);
                var base_max = GetRandomInt(ScramblerData_Params.Static.PlayerParamData.Max_Level_Up_Cost_Min, ScramblerData_Params.Static.PlayerParamData.Max_Level_Up_Cost_Max);
                var growth = (base_max / 850);

                var current_value = base_min;

                foreach (PARAM.Row row in rows)
                {
                    row["soul_level_cost"].Value = current_value;

                    current_value = current_value + growth;
                }
            }
            if (c_EventCommonParam_ShrineOfWinter_Cost)
            {
                List<PARAM.Row> rows = EventCommonParam.Where(row => row.ID == 14).ToList();

                foreach (PARAM.Row row in rows)
                {
                    row["event_value"].Value = GetRandomInt(ScramblerData_Params.Static.PlayerParamData.Shrine_of_Winter_Unlock_Amount_Min, ScramblerData_Params.Static.PlayerParamData.Shrine_of_Winter_Unlock_Amount_Max);
                }
            }
            if (c_BossBattleParam_BossSoulDrops)
            {
                List<PARAM.Row> rows = BossBattleParam;

                foreach (PARAM.Row row in rows)
                {
                    row["souls"].Value = GetRandomUInt(ScramblerData_Params.Static.PlayerParamData.Boss_Soul_Drop_Amount_Min, ScramblerData_Params.Static.PlayerParamData.Boss_Soul_Drop_Amount_Max);
                }
            }
            if (c_LockOnParam_CameraDistance)
            {
                List<PARAM.Row> rows = LockOnParam;

                foreach (PARAM.Row row in rows)
                {
                    row["max_dist_behind_player"].Value = (1 + rand.NextDouble() * GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Camera_Distance_Behind_Player_Min, ScramblerData_Params.Static.PlayerParamData.Camera_Distance_Behind_Player_Max));
                }
            }
            if (c_LockOnParam_CameraFOV)
            {
                List<PARAM.Row> rows = LockOnParam;

                foreach (PARAM.Row row in rows)
                {
                    row["fov_0"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Cameria_Horizontal_FOV_Min, ScramblerData_Params.Static.PlayerParamData.Cameria_Horizontal_FOV_Max);
                    row["fov_1"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Cameria_Vertical_FOV_Min, ScramblerData_Params.Static.PlayerParamData.Cameria_Vertical_FOV_Max);
                }
            }
            if (c_ChrMoveParam_Walk)
            {
                List<PARAM.Row> rows = ChrMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["walk_acceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Walk_Acceleration_Min, ScramblerData_Params.Static.PlayerParamData.Walk_Acceleration_Max);
                    row["walk_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Walk_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Walk_Speed_Max);
                    row["walk_deceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Walk_Deceleration_Min, ScramblerData_Params.Static.PlayerParamData.Walk_Deceleration_Max);
                    row["lock_on_walk_acceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Walk_LockOn_Acceleration_Min, ScramblerData_Params.Static.PlayerParamData.Walk_LockOn_Acceleration_Max);
                    row["lock_on_walk_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Walk_LockOn_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Walk_LockOn_Speed_Max);
                    row["lock_on_walk_deceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Walk_LockOn_Deceleration_Min, ScramblerData_Params.Static.PlayerParamData.Walk_LockOn_Deceleration_Max);
                }
            }
            if (c_ChrMoveParam_Run)
            {
                List<PARAM.Row> rows = ChrMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["run_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Run_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Run_Speed_Max);
                    row["run_acceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Run_Acceleration_Min, ScramblerData_Params.Static.PlayerParamData.Run_Acceleration_Max);
                    row["run_to_walk_deceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Run_Deceleration_Min, ScramblerData_Params.Static.PlayerParamData.Run_Deceleration_Max);
                    row["lock_on_run_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Run_LockOn_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Run_LockOn_Speed_Max);
                    row["lock_on_run_acceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Run_LockOn_Acceleration_Min, ScramblerData_Params.Static.PlayerParamData.Run_LockOn_Acceleration_Max);
                    row["lock_on_run_to_walk_deceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Run_LockOn_Deceleration_Min, ScramblerData_Params.Static.PlayerParamData.Run_LockOn_Deceleration_Max);
                }
            }
            if (c_ChrMoveParam_Jump)
            {
                List<PARAM.Row> rows = ChrMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["jump_height"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Height_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Height_Max);
                    row["Unk51"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Unk51_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Unk51_Max);
                    row["Unk52"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Unk52_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Unk52_Max);
                    row["jump_length_min"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Length_Minimum_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Length_Minimum_Max);
                    row["jump_length_max"].Value = ((float)row["jump_length_min"].Value * GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Length_Maximum_Multiplier_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Length_Maximum_Multiplier_Max));
                    row["Unk53"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Unk53_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Unk53_Max);
                    row["Unk54"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Unk54_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Unk54_Max);
                    row["jump_gravity_min"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Gravity_Minimum_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Gravity_Minimum_Max);
                    row["jump_gravity_max"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Gravity_Maximum_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Gravity_Maximum_Max);
                    row["Unk55"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Unk55_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Unk55_Max);
                    row["Unk56"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Unk56_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Unk56_Max);
                    row["Unk57"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Jump_Unk57_Min, ScramblerData_Params.Static.PlayerParamData.Jump_Unk57_Max);

                    // Adjust these so the player doesn't just die when they jump high
                    row["fall_distance_breakpoint_1"].Value = ((float)row["jump_height"].Value * 1.2);
                    row["fall_distance_breakpoint_2"].Value = ((float)row["jump_height"].Value * 1.4);
                    row["fall_distance_breakpoint_3"].Value = ((float)row["jump_height"].Value * 1.6);
                    row["fall_distance_max"].Value = ((float)row["jump_height"].Value * 2);
                }
            }
            if (c_ChrMoveParam_Ladder)
            {
                List<PARAM.Row> rows = ChrMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["ladder_climb_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Climb_Ladder_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Climb_Ladder_Speed_Max);
                    row["ladder_fast_climb_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Climb_Ladder_Fast_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Climb_Ladder_Fast_Speed_Max);
                    row["ladder_slide_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Climb_Ladder_Slide_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Climb_Ladder_Slide_Speed_Max);
                    row["Unk107"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Climb_Unk107_Min, ScramblerData_Params.Static.PlayerParamData.Climb_Unk107_Max);
                    row["Unk108"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Climb_Unk108_Min, ScramblerData_Params.Static.PlayerParamData.Climb_Unk108_Max);
                }
            }
            if (c_ChrMoveParam_Turn)
            {
                List<PARAM.Row> rows = ChrMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["slow_walk_turn_rate"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Slow_Walk_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Slow_Walk_Turn_Rate_Max);
                    row["walk_turn_rate"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Walk_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Walk_Turn_Rate_Max);
                    row["Unk08"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk08_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Unk08_Turn_Rate_Max);
                    row["Unk09"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk09_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Unk09_Turn_Rate_Max);
                    row["Unk12"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk12_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Unk12_Turn_Rate_Max);
                    row["Unk13"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk13_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Unk13_Turn_Rate_Max);
                    row["Unk14"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk14_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Unk14_Turn_Rate_Max);
                    row["Unk15"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk15_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Unk15_Turn_Rate_Max);
                    row["lock_on_walk_turn_rate"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Walk_LockOn_Turn_Rate_Min, ScramblerData_Params.Static.PlayerParamData.Walk_LockOn_Turn_Rate_Max);
                }
            }
            if (c_ChrMoveParam_Evasion)
            {
                List<PARAM.Row> rows = ChrMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["backstep_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Backstep_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Backstep_Speed_Max);
                    row["backstep_distance"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Backstep_Distance_Min, ScramblerData_Params.Static.PlayerParamData.Backstep_Distance_Max);
                    row["roll_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Roll_Speed_Min, ScramblerData_Params.Static.PlayerParamData.Roll_Speed_Max);
                    row["Unk45"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk45_Min, ScramblerData_Params.Static.PlayerParamData.Unk45_Max);
                    row["Unk47"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk47_Min, ScramblerData_Params.Static.PlayerParamData.Unk47_Max);
                    row["Unk49"].Value = GetRandomFloat(ScramblerData_Params.Static.PlayerParamData.Unk49_Min, ScramblerData_Params.Static.PlayerParamData.Unk49_Max);
                }
            }
            if (c_Tweak_BigJumpMode)
            {
                List<PARAM.Row> rows = ChrMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["jump_height"].Value = ScramblerData_Params.Static.PlayerParamData.BigJump_Jump_Height;
                    row["jump_length_min"].Value = ScramblerData_Params.Static.PlayerParamData.BigJump_Jump_Length_Minimum;
                    row["jump_length_max"].Value = ScramblerData_Params.Static.PlayerParamData.BigJump_Jump_Length_Maximum;
                    row["jump_gravity_min"].Value = ScramblerData_Params.Static.PlayerParamData.BigJumpJump_Gravity_Minimum;
                    row["jump_gravity_max"].Value = ScramblerData_Params.Static.PlayerParamData.BigJumpJump_Gravity_Maximum;
                    row["Unk55"].Value = ScramblerData_Params.Static.PlayerParamData.BigJump_Unk55;
                    row["Unk56"].Value = ScramblerData_Params.Static.PlayerParamData.BigJump_Unk56;
                    row["Unk57"].Value = ScramblerData_Params.Static.PlayerParamData.BigJump_Unk57;

                    // Adjust these so the player doesn't just die when they jump high
                    row["fall_distance_breakpoint_1"].Value = ((float)row["jump_height"].Value * 1.2);
                    row["fall_distance_breakpoint_2"].Value = ((float)row["jump_height"].Value * 1.4);
                    row["fall_distance_breakpoint_3"].Value = ((float)row["jump_height"].Value * 1.6);
                    row["fall_distance_max"].Value = ((float)row["jump_height"].Value * 2);
                }
            }

            return regulation;
        }

        public void RandomiseClassStats(PARAM.Row row)
        {
            int statSkew = ScramblerData_Params.Static.PlayerParamData.Class_Attributes_Stat_Spread;

            ushort vigor = GetRandomStat(row, "vigor", statSkew);
            ushort endurance = GetRandomStat(row, "endurance", statSkew);
            ushort attunement = GetRandomStat(row, "attunement", statSkew);
            ushort vitality = GetRandomStat(row, "vitality", statSkew);
            ushort strength = GetRandomStat(row, "strength", statSkew);
            ushort dexterity = GetRandomStat(row, "dexterity", statSkew);
            ushort intelligence = GetRandomStat(row, "intelligence", statSkew);
            ushort faith = GetRandomStat(row, "faith", statSkew);
            ushort adaptability = GetRandomStat(row, "adaptability", statSkew);

            ushort total = (ushort)((vigor + endurance + attunement + vitality + strength + dexterity + intelligence + faith + adaptability) - 54);

            if (total < 1)
                total = 1;

            row["soul_level"].Value = total;
            row["vigor"].Value = vigor;
            row["endurance"].Value = endurance;
            row["attunement"].Value = attunement;
            row["vitality"].Value = vitality;
            row["strength"].Value = strength;
            row["dexterity"].Value = dexterity;
            row["intelligence"].Value = intelligence;
            row["faith"].Value = faith;
            row["adaptability"].Value = adaptability;
        }

        public ushort GetRandomStat(PARAM.Row row, string stat, int statSkew)
        {
            ushort adjust = (ushort)statSkew;

            ushort stat_value = (ushort)row[stat].Value;

            ushort lower = (ushort)(stat_value - adjust);
            ushort upper = (ushort)(stat_value + adjust);


            if (lower > upper)
                lower = (ushort)(upper - 1);

            if (lower < 1)
                lower = 1;

            if (upper < lower)
                upper = (ushort)(lower + 1);

            return (ushort)rand.Next(lower, upper);
        }

        public void RandomiseGift(PARAM.Row row)
        {
            int roll = rand.Next(100);

            // Clean up current values
            row["ring_item_1"].Value = -1;

            for (int x = 1; x < 10; x++)
            {
                row[$"item_{x}"].Value = -1;
                row[$"item_amount_{x}"].Value = 0;
            }

            // Ring
            if (roll <= 25)
            {
                Util.SetRandomItem(row, "ring_item_1", Data.Row_List_Rings);
            }
            // Good
            else
            {
                int endChance = 20;

                // Multiple Items row
                if (row.ID == 530)
                    endChance = 0;

                for (int x = 1; x < 10; x++)
                {
                    Util.SetRandomGood(row, $"item_{x}", Data, $"item_amount_{x}");

                    endChance = endChance + rand.Next(1, x * 10);
                    if (endChance > 25)
                        break;
                }
            }
        }

        List<PARAM.Row> valid_melee_weapons = new List<PARAM.Row>();
        List<PARAM.Row> valid_sorcery_catalyst = new List<PARAM.Row>();
        List<PARAM.Row> valid_miracle_catalyst = new List<PARAM.Row>();
        List<PARAM.Row> valid_pyromancy_catalyst = new List<PARAM.Row>();
        List<PARAM.Row> valid_hex_catalyst = new List<PARAM.Row>();
        List<PARAM.Row> valid_bows = new List<PARAM.Row>();
        List<PARAM.Row> valid_greatbows = new List<PARAM.Row>();
        List<PARAM.Row> valid_crossbows = new List<PARAM.Row>();
        List<PARAM.Row> valid_shields = new List<PARAM.Row>();
        List<PARAM.Row> valid_head_armor = new List<PARAM.Row>();
        List<PARAM.Row> valid_chest_armor = new List<PARAM.Row>();
        List<PARAM.Row> valid_arm_armor = new List<PARAM.Row>();
        List<PARAM.Row> valid_leg_armor = new List<PARAM.Row>();
        List<PARAM.Row> valid_sorceries = new List<PARAM.Row>();
        List<PARAM.Row> valid_miracles = new List<PARAM.Row>();
        List<PARAM.Row> valid_pyromancies = new List<PARAM.Row>();
        List<PARAM.Row> valid_hexes = new List<PARAM.Row>();

        public bool usesSorceries = false;
        public bool usesMiracles = false;
        public bool usesPyromancies = false;
        public bool usesHexes = false;
        public bool usesArrows = false;
        public bool usesGreatarrows = false;
        public bool usesBolts = false;
        public bool setFirstSpell = false;

        public int ScramblerData_Params_LowScale { get; private set; }

        public void RandomiseClassEquipment(PARAM.Row row, bool allowAnyEquipment)
        {
            ushort attunement = (ushort)row["attunement"].Value;
            ushort vitality = (ushort)row["vitality"].Value;
            ushort strength = (ushort)row["strength"].Value;
            ushort dexterity = (ushort)row["dexterity"].Value;
            ushort intelligence = (ushort)row["intelligence"].Value;
            ushort faith = (ushort)row["faith"].Value;

            usesSorceries = false;
            usesMiracles = false;
            usesPyromancies = false;
            usesHexes = false;
            usesArrows = false;
            usesGreatarrows = false;
            usesBolts = false;
            setFirstSpell = false;

            UpdateSelectionListsForClass((int)attunement, (int)vitality, (int)strength, (int)dexterity, (int)intelligence, (int)faith, allowAnyEquipment);

            // Weapons
            AssignWeaponSlot(row, "right_weapon_item_1", false);

            if (rand.Next(100) >= 50)
                AssignWeaponSlot(row, "right_weapon_item_2", false);

            if (rand.Next(100) >= 75)
                AssignWeaponSlot(row, "right_weapon_item_3", false);

            AssignWeaponSlot(row, "left_weapon_item_1", true);

            if (rand.Next(100) >= 50)
                AssignWeaponSlot(row, "left_weapon_item_2", true);

            if (rand.Next(100) >= 75)
                AssignWeaponSlot(row, "left_weapon_item_3", true);

            // Spells
            AssignSpell(row, "spell_item_1");

            if (rand.Next(100) >= 50)
                AssignSpell(row, "spell_item_2");

            if (rand.Next(100) >= 75)
                AssignSpell(row, "spell_item_3");

            // Armor
            AssignArmor(row, "head_item", valid_head_armor);
            AssignArmor(row, "chest_item", valid_chest_armor);
            AssignArmor(row, "hands_item", valid_arm_armor);
            AssignArmor(row, "legs_item", valid_leg_armor);

            // Rings
            if (rand.Next(100) >= 50)
                Util.SetRandomItem(row, "ring_item_1", Data.Row_List_Rings);

            // Ammo
            if (usesArrows)
                Util.SetRandomItemWithAmount(row, "arrow_item_1", Data.Row_List_Ammunition_Arrow, "arrow_amount_1", 25, 50);

            if (usesGreatarrows)
                Util.SetRandomItemWithAmount(row, "arrow_item_1", Data.Row_List_Ammunition_Greatarrow, "arrow_amount_1", 25, 50);

            if (usesBolts)
                Util.SetRandomItemWithAmount(row, "bolt_item_1", Data.Row_List_Ammunition_Bolt, "bolt_amount_1", 25, 50);

            // Starting Item
            for (int x = 1; x <= 7; x++)
            {
                if (rand.Next(100) > 50 || row.ID == 90)
                    Util.SetRandomGood(row, $"item_{x}", Data, $"item_amount_{x}");
            }
        }

        public void AssignArmor(PARAM.Row row, string slot, List<PARAM.Row> armorList)
        {
            Random rand = new Random();

            if (armorList.Count > 0)
            {
                // Conver the ArmorParam ID to ItemParam ID if required
                string raw_id = armorList[rand.Next(armorList.Count)].ID.ToString();

                if (raw_id.Substring(0, 1) == "1")
                {
                    string new_id = raw_id.Substring(1);
                    raw_id = $"2{new_id}";
                }

                row[slot].Value = raw_id;
            }
        }

        public void AssignSpell(PARAM.Row row, string slot)
        {
            Random rand = new Random();

            int roll = rand.Next(100);

            if (!setFirstSpell)
            {
                roll = 25;
                setFirstSpell = true;
            }

            // Sorceries
            if (usesSorceries && valid_sorceries.Count > 0)
            {
                if (roll >= 25)
                {
                    row[slot].Value = valid_sorceries[rand.Next(valid_sorceries.Count)].ID;
                    usesSorceries = true;
                }
            }
            // Miracles
            if (usesMiracles && valid_miracles.Count > 0)
            {
                if (roll >= 25)
                {
                    row[slot].Value = valid_miracles[rand.Next(valid_miracles.Count)].ID;
                    usesSorceries = true;
                }
            }
            // Pyromancies
            if (usesPyromancies && valid_pyromancies.Count > 0)
            {
                if (roll >= 25)
                {
                    row[slot].Value = valid_pyromancies[rand.Next(valid_pyromancies.Count)].ID;
                    usesSorceries = true;
                }
            }
            // Hexes
            if (usesHexes && valid_hexes.Count > 0)
            {
                if (roll >= 25)
                {
                    row[slot].Value = valid_hexes[rand.Next(valid_hexes.Count)].ID;
                    usesSorceries = true;
                }
            }
        }

        public void AssignWeaponSlot(PARAM.Row row, string slot, bool allowShields)
        {
            Random rand = new Random();

            // Right Weapon 1
            int roll = rand.Next(100);

            // Caster
            if (roll >= 66 && !usesSorceries && !usesMiracles && !usesPyromancies && !usesHexes)
            {
                roll = rand.Next(100);

                // Sorceries
                if (roll >= 75 && valid_sorceries.Count > 0 && valid_sorcery_catalyst.Count > 0)
                {
                    row[slot].Value = valid_sorcery_catalyst[rand.Next(valid_sorcery_catalyst.Count)].ID;
                    usesSorceries = true;
                }
                // Miracles
                else if (roll >= 50 && roll < 75 && valid_miracles.Count > 0 && valid_miracle_catalyst.Count > 0)
                {
                    row[slot].Value = valid_miracle_catalyst[rand.Next(valid_miracle_catalyst.Count)].ID;
                    usesMiracles = true;
                }
                // Pyromancies
                else if (roll >= 25 && roll < 50 && valid_pyromancies.Count > 0 && valid_pyromancy_catalyst.Count > 0)
                {
                    row[slot].Value = valid_pyromancy_catalyst[rand.Next(valid_pyromancy_catalyst.Count)].ID;
                    usesPyromancies = true;
                }
                // Hexes
                else if (roll < 25 && valid_hexes.Count > 0 && valid_hex_catalyst.Count > 0)
                {
                    row[slot].Value = valid_hex_catalyst[rand.Next(valid_hex_catalyst.Count)].ID;
                    usesHexes = true;
                }
                // Fallback
                else if (valid_melee_weapons.Count > 0)
                {
                    row[slot].Value = valid_melee_weapons[rand.Next(valid_melee_weapons.Count)].ID;
                }
                else
                {
                    row[slot].Value = Data.Row_List_Weapons_Melee[rand.Next(Data.Row_List_Weapons_Melee.Count)].ID;
                }
            }
            // Ranged
            else if (roll >= 33 && roll < 66 && !usesArrows && !usesGreatarrows && !usesArrows)
            {
                roll = rand.Next(100);

                // Crossbow
                if (roll >= 66 && valid_crossbows.Count > 0)
                {
                    row[slot].Value = valid_crossbows[rand.Next(valid_crossbows.Count)].ID;
                    usesArrows = true;
                }
                // Greatbow
                else if (roll >= 33 && roll < 66 && valid_greatbows.Count > 0)
                {
                    row[slot].Value = valid_greatbows[rand.Next(valid_greatbows.Count)].ID;
                    usesGreatarrows = true;
                }
                // Bow
                else if (roll < 33 && valid_bows.Count > 0)
                {
                    row[slot].Value = valid_bows[rand.Next(valid_bows.Count)].ID;
                    usesArrows = true;
                }
                // Fallback
                else if (valid_melee_weapons.Count > 0)
                {
                    row[slot].Value = valid_melee_weapons[rand.Next(valid_melee_weapons.Count)].ID;
                }
                else
                {
                    row[slot].Value = Data.Row_List_Weapons_Melee[rand.Next(Data.Row_List_Weapons_Melee.Count)].ID;
                }
            }
            // Melee
            else
            {
                if (allowShields)
                {
                    roll = rand.Next(100);

                    if (roll >= 50 && valid_shields.Count > 0)
                    {
                        row[slot].Value = valid_shields[rand.Next(valid_shields.Count)].ID;
                    }
                    // Fallback
                    else if (valid_melee_weapons.Count > 0)
                    {
                        row[slot].Value = valid_melee_weapons[rand.Next(valid_melee_weapons.Count)].ID;
                    }
                    else
                    {
                        row[slot].Value = Data.Row_List_Weapons_Melee[rand.Next(Data.Row_List_Weapons_Melee.Count)].ID;
                    }
                }
                // Fallback
                else if (valid_melee_weapons.Count > 0)
                {
                    row[slot].Value = valid_melee_weapons[rand.Next(valid_melee_weapons.Count)].ID;
                }
                else
                {
                    row[slot].Value = Data.Row_List_Weapons_Melee[rand.Next(Data.Row_List_Weapons_Melee.Count)].ID;
                }
            }
        }

        public void UpdateSelectionListsForClass(int attunement, int vitality, int strength, int dexterity, int intelligence, int faith, bool allowAnyEquipment)
        {
            strength = (int)(strength * 1.5);

            if (!allowAnyEquipment)
            {
                valid_melee_weapons = Data.Row_List_Weapons_Melee.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_sorcery_catalyst = Data.Row_List_Weapons_Catalyst_Sorcery.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_miracle_catalyst = Data.Row_List_Weapons_Catalyst_Miracles.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_pyromancy_catalyst = Data.Row_List_Weapons_Catalyst_Pyromancy.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_hex_catalyst = Data.Row_List_Weapons_Catalyst_Hex.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_bows = Data.Row_List_Weapons_Bow.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_greatbows = Data.Row_List_Weapons_Greatbow.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_crossbows = Data.Row_List_Weapons_Crossbow.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_shields = Data.Row_List_Weapons_Shield.Where(row =>
                strength >= (short)row["str_requirement"].Value &&
                dexterity >= (short)row["dex_requirement"].Value &&
                intelligence >= (short)row["int_requirement"].Value &&
                faith >= (short)row["fth_requirement"].Value
                ).ToList();

                valid_head_armor = Data.Row_List_ActualArmor_Head.Where(row =>
                strength >= (ushort)row["strength_requirement"].Value &&
                dexterity >= (ushort)row["dexterity_requirement"].Value &&
                intelligence >= (ushort)row["intelligence_requirement"].Value &&
                faith >= (ushort)row["faith_requirement"].Value
                ).ToList();

                valid_chest_armor = Data.Row_List_ActualArmor_Chest.Where(row =>
                strength >= (ushort)row["strength_requirement"].Value &&
                dexterity >= (ushort)row["dexterity_requirement"].Value &&
                intelligence >= (ushort)row["intelligence_requirement"].Value &&
                faith >= (ushort)row["faith_requirement"].Value
                ).ToList();

                valid_arm_armor = Data.Row_List_ActualArmor_Arms.Where(row =>
                strength >= (ushort)row["strength_requirement"].Value &&
                dexterity >= (ushort)row["dexterity_requirement"].Value &&
                intelligence >= (ushort)row["intelligence_requirement"].Value &&
                faith >= (ushort)row["faith_requirement"].Value
                ).ToList();

                valid_leg_armor = Data.Row_List_ActualArmor_Legs.Where(row =>
                strength >= (ushort)row["strength_requirement"].Value &&
                dexterity >= (ushort)row["dexterity_requirement"].Value &&
                intelligence >= (ushort)row["intelligence_requirement"].Value &&
                faith >= (ushort)row["faith_requirement"].Value
                ).ToList();

                valid_sorceries = Data.Row_List_Spell_Sorceries.Where(row =>
                intelligence >= (ushort)row["int_requirement"].Value &&
                faith >= (ushort)row["fth_requirement"].Value
                ).ToList();

                valid_miracles = Data.Row_List_Spell_Miracles.Where(row =>
                intelligence >= (ushort)row["int_requirement"].Value &&
                faith >= (ushort)row["fth_requirement"].Value
                ).ToList();

                valid_pyromancies = Data.Row_List_Spell_Pyromancies.Where(row =>
                intelligence >= (ushort)row["int_requirement"].Value &&
                faith >= (ushort)row["fth_requirement"].Value
                ).ToList();

                valid_hexes = Data.Row_List_Spell_Hexes.Where(row =>
                intelligence >= (ushort)row["int_requirement"].Value &&
                faith >= (ushort)row["fth_requirement"].Value
                ).ToList();
            }
            else
            {
                valid_melee_weapons = Data.Row_List_Weapons_Melee;
                valid_sorcery_catalyst = Data.Row_List_Weapons_Catalyst_Sorcery;
                valid_miracle_catalyst = Data.Row_List_Weapons_Catalyst_Miracles;
                valid_pyromancy_catalyst = Data.Row_List_Weapons_Catalyst_Pyromancy;
                valid_hex_catalyst = Data.Row_List_Weapons_Catalyst_Hex;
                valid_bows = Data.Row_List_Weapons_Bow;
                valid_greatbows = Data.Row_List_Weapons_Greatbow;
                valid_crossbows = Data.Row_List_Weapons_Crossbow;
                valid_shields = Data.Row_List_Weapons_Shield;
                valid_head_armor = Data.Row_List_Armor_Head;
                valid_chest_armor = Data.Row_List_Armor_Chest;
                valid_arm_armor = Data.Row_List_Armor_Arms;
                valid_leg_armor = Data.Row_List_Armor_Legs;
                valid_sorceries = Data.Row_List_Spell_Sorceries;
                valid_miracles = Data.Row_List_Spell_Miracles;
                valid_pyromancies = Data.Row_List_Spell_Pyromancies;
                valid_hexes = Data.Row_List_Spell_Hexes;
            }
        }

        public Regulation Scramble_MapParams(
            bool c_TreasureBoxParam_TrappedChests
        )
        {
            if (c_TreasureBoxParam_TrappedChests)
            {
                foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
                {
                    PARAM param = wrapper.Param;
                    List<PARAM.Row> param_rows = param.Rows;

                    if (wrapper.Name.Contains("treasureboxparam"))
                    {
                        foreach (PARAM.Row row in param_rows)
                        {
                            bool isTrapped = true;

                            if (rand.Next(100) < ScramblerData_Params.Static.MapParamData.Trapped_Chest_Chance)
                                isTrapped = false;

                            foreach (PARAM.Cell cell in row.Cells)
                            {
                                if (isTrapped)
                                {
                                    if (cell.Def.InternalName == "chest_type")
                                        cell.Value = 1;

                                    if (cell.Def.InternalName == "bullet_id_1")
                                        cell.Value = 200001030;

                                    if (cell.Def.InternalName == "bullet_id_2")
                                        cell.Value = 200001130;

                                    if (cell.Def.InternalName == "bullet_id_3")
                                        cell.Value = 200001330;

                                    if (cell.Def.InternalName == "bullet_id_4")
                                        cell.Value = 200001230;
                                }
                                else
                                {
                                    if (cell.Def.InternalName == "chest_type")
                                        cell.Value = 0;

                                    if (cell.Def.InternalName == "bullet_id_1")
                                        cell.Value = 0;

                                    if (cell.Def.InternalName == "bullet_id_2")
                                        cell.Value = 0;

                                    if (cell.Def.InternalName == "bullet_id_3")
                                        cell.Value = 0;

                                    if (cell.Def.InternalName == "bullet_id_4")
                                        cell.Value = 0;
                                }
                            }
                        }
                    }
                }
            }

            return regulation;
        }

        public Regulation Scramble_CharacterParams(
            bool c_NpcPlayerStatusParam_Equipment
        )
        {
            List<PARAM.Row> NpcPlayerStatusParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"NpcPlayerStatusParam")).ToList()[0].Rows;

            if (c_NpcPlayerStatusParam_Equipment)
            {
                List<PARAM.Row> rows = NpcPlayerStatusParam.Where(row => row.ID >= 6940).ToList();

                foreach (PARAM.Row row in rows)
                {
                    Util.SetRandomItem(row, "spell_item_1", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_2", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_3", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_4", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_5", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_6", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_7", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_8", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_9", Data.Row_List_Spells_NPC);
                    Util.SetRandomItem(row, "spell_item_10", Data.Row_List_Spells_NPC);

                    Util.SetRandomItem(row, "right_weapon_item_1", Data.Row_List_Weapons);
                    Util.SetRandomItem(row, "right_weapon_item_2", Data.Row_List_Weapons);

                    if (rand.Next(100) <= 50)
                        Util.SetRandomItem(row, "right_weapon_item_3", Data.Row_List_Weapons);
                    else
                        row["right_weapon_item_3"].Value = -1;

                    int roll = rand.Next(100);

                    if (roll <= 20)
                        Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Catalyst_Sorcery);
                    else if (roll > 20 && roll <= 30)
                        Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Catalyst_Miracles);
                    else if (roll > 30 && roll <= 40)
                        Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Catalyst_Pyromancy);
                    else if (roll > 40 && roll <= 50)
                        Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Catalyst_Hex);
                    else if (roll > 50 && roll <= 75)
                        Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons_Shield);
                    else
                        Util.SetRandomItem(row, "left_weapon_item_1", Data.Row_List_Weapons);

                    roll = rand.Next(100);

                    if (roll <= 20)
                        Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Catalyst_Sorcery);
                    else if (roll > 20 && roll <= 30)
                        Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Catalyst_Miracles);
                    else if (roll > 30 && roll <= 40)
                        Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Catalyst_Pyromancy);
                    else if (roll > 40 && roll <= 50)
                        Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Catalyst_Hex);
                    else if (roll > 50 && roll <= 75)
                        Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons_Shield);
                    else
                        Util.SetRandomItem(row, "left_weapon_item_2", Data.Row_List_Weapons);

                    if (rand.Next(100) < 50)
                    {
                        if (roll <= 20)
                            Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Catalyst_Sorcery);
                        else if (roll > 20 && roll <= 30)
                            Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Catalyst_Miracles);
                        else if (roll > 30 && roll <= 40)
                            Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Catalyst_Pyromancy);
                        else if (roll > 40 && roll <= 50)
                            Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Catalyst_Hex);
                        else if (roll > 50 && roll <= 75)
                            Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons_Shield);
                        else
                            Util.SetRandomItem(row, "left_weapon_item_3", Data.Row_List_Weapons);
                    }

                    // Armor
                    Util.SetRandomItem(row, "head_item", Data.Row_List_ActualArmor_Head);
                    Util.SetRandomItem(row, "chest_item", Data.Row_List_ActualArmor_Chest);
                    Util.SetRandomItem(row, "hands_item", Data.Row_List_ActualArmor_Arms);
                    Util.SetRandomItem(row, "legs_item", Data.Row_List_ActualArmor_Legs);

                    // Rings
                    if (rand.Next(100) < 25)
                    {
                        Util.SetRandomItem(row, "ring_item_1", Data.Row_List_Rings);
                    }
                }
            }

            return regulation;
        }

        public Regulation Scramble_EnemyParams(
            bool c_Enemy_IncludeBosses,
            bool c_Enemy_IncludeCharacters,
            bool c_Enemy_IncludeSummons,
            bool c_Enemy_IncludeHostileCharacters,
            bool c_LogicComParam_Detection,
            bool c_EnemyParam_HP,
            bool c_EnemyParam_Souls,
            bool c_EnemyParam_Stamina,
            bool c_EnemyParam_Defence,
            bool c_EnemyParam_ShieldDefence,
            bool c_EnemyParam_Poise,
            bool c_EnemyDamageParam_Damage,
            bool c_EnemyDamageParam_Knockback,
            bool c_EnemyDamageParam_AttackSpeed,
            bool c_EnemyMoveParam_Walk,
            bool c_EnemyMoveParam_Run,
            bool c_EnemyMoveParam_Jump,
            bool c_EnemyMoveParam_Climb,
            bool c_EnemyMoveParam_Turn,
            bool c_EnemyMoveParam_Evasion
        )
        {
            List<PARAM.Row> LogicComParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"LogicComParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyDamageParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyDamageParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyMoveParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyMoveParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyBehaviorParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyBehaviorParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyBehaviorSecondParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyBehaviorSecondParam")).ToList()[0].Rows;
            List<PARAM.Row> EnemyBehaviorThirdParam = regulation.regulationParamWrappers.Where(n => n.Name == ($"EnemyBehaviorThirdParam")).ToList()[0].Rows;

            // Exclude the test/debug rows.
            EnemyParam = EnemyParam.Where(row => row.ID >= 100000).ToList();
            EnemyMoveParam = EnemyMoveParam.Where(row => row.ID >= 100000 && row.ID <= 800010).ToList();
            EnemyDamageParam = EnemyDamageParam.Where(row => row.ID >= 110000010).ToList();
            LogicComParam = LogicComParam.Where(row => row.ID >= 100010).ToList();

            // Remove X type rows if the X type has not been included
            if (!c_Enemy_IncludeBosses)
            {
                EnemyParam = EnemyParam.Where(row => !ScramblerData_Core.Static.Boss_EnemyParamID_List.Contains(row.ID)).ToList();
                EnemyMoveParam = EnemyMoveParam.Where(row => !ScramblerData_Core.Static.Boss_EnemyParamID_List.Contains(row.ID)).ToList();
                EnemyDamageParam = EnemyDamageParam.Where(row => !IsDamageParamMatch(row.ID.ToString(), ScramblerData_Core.Static.Boss_EnemyParamID_List)).ToList();
                LogicComParam = LogicComParam.Where(row => !IsLogicComParamMatch(row.ID.ToString(), ScramblerData_Core.Static.Boss_EnemyParamID_List)).ToList();
            }
            if (!c_Enemy_IncludeCharacters)
            {
                EnemyParam = EnemyParam.Where(row => !ScramblerData_Core.Static.Character_EnemyParamID_List.Contains(row.ID)).ToList();
                EnemyMoveParam = EnemyMoveParam.Where(row => !ScramblerData_Core.Static.Character_EnemyParamID_List.Contains(row.ID)).ToList();
                EnemyDamageParam = EnemyDamageParam.Where(row => !IsDamageParamMatch(row.ID.ToString(), ScramblerData_Core.Static.Character_EnemyParamID_List)).ToList();
                LogicComParam = LogicComParam.Where(row => !IsLogicComParamMatch(row.ID.ToString(), ScramblerData_Core.Static.Character_EnemyParamID_List)).ToList();
            }
            if (!c_Enemy_IncludeSummons)
            {
                EnemyParam = EnemyParam.Where(row => !ScramblerData_Core.Static.Summon_Character_EnemyParamID_List.Contains(row.ID)).ToList();
            }
            if (!c_Enemy_IncludeHostileCharacters)
            {
                EnemyParam = EnemyParam.Where(row => !ScramblerData_Core.Static.Hostile_Character_EnemyParamID_List.Contains(row.ID)).ToList();
            }

            // Only apply these if a group is actually included
            if (c_LogicComParam_Detection)
            {
                foreach (PARAM.Row row in LogicComParam)
                {
                    row["sight_distance"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.AI_Sight_Distance_Min, ScramblerData_Params.Static.EnemyParamData.AI_Sight_Distance_Max);
                    row["detect_distance"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.AI_Detect_Distance_Min, ScramblerData_Params.Static.EnemyParamData.AI_Detect_Distance_Max);
                    row["vision_cone_horizontal_angle"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Vision_Cone_Horizontal_Angle_Min, ScramblerData_Params.Static.EnemyParamData.Vision_Cone_Horizontal_Angle_Max);
                    row["vision_cone_vertical_angle"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Vision_Cone_Vertical_Angle_Min, ScramblerData_Params.Static.EnemyParamData.Vision_Cone_Vertical_Angle_Max);
                }
            }
            if (c_EnemyParam_HP)
            {
                foreach (PARAM.Row row in EnemyParam)
                {
                    // Boss
                    if (ScramblerData_Core.Static.Boss_EnemyParamID_List.Contains(row.ID))
                    {
                        row["stat_hp"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Boss_HP_Min, ScramblerData_Params.Static.EnemyParamData.Boss_HP_Max);
                    }
                    // Enemy
                    else
                    {
                        row["stat_hp"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Enemy_HP_Min, ScramblerData_Params.Static.EnemyParamData.Enemy_HP_Max);
                    }
                }
            }
            if (c_EnemyParam_Souls)
            {
                foreach (PARAM.Row row in EnemyParam)
                {
                    // Boss
                    if (ScramblerData_Core.Static.Boss_EnemyParamID_List.Contains(row.ID))
                    {
                        row["souls_held"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Boss_Souls_Min, ScramblerData_Params.Static.EnemyParamData.Boss_Souls_Max);
                    }
                    // Enemy
                    else
                    {
                        row["souls_held"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Enemy_Souls_Min, ScramblerData_Params.Static.EnemyParamData.Enemy_Souls_Max);
                    }
                }
            }
            if (c_EnemyParam_Stamina)
            {
                foreach (PARAM.Row row in EnemyParam)
                {
                    row["stat_stamina"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Stamina_Min, ScramblerData_Params.Static.EnemyParamData.Stamina_Max);
                    row["stat_stamina_regen"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Stamina_Regen_Min, ScramblerData_Params.Static.EnemyParamData.Stamina_Regen_Max);
                }
            }
            if (c_EnemyParam_Defence)
            {
                foreach (PARAM.Row row in EnemyParam)
                {
                    row["defence_physical"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Physical_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Physical_Defence_Max);
                    row["defence_magic"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Magic_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Magic_Defence_Max);
                    row["defence_lightning"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Lightning_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Lightning_Defence_Max);
                    row["defence_fire"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Fire_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Fire_Defence_Max);
                    row["defence_dark"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Dark_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Dark_Defence_Max);
                    row["defence_poison"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Poison_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Poison_Defence_Max);
                    row["defence_bleed"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Bleed_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Bleed_Defence_Max);
                    row["defence_curse"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Curse_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Curse_Defence_Max);
                    row["defence_petrify"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Petrify_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Petrify_Defence_Max);
                }
            }
            if (c_EnemyParam_ShieldDefence)
            {
                foreach (PARAM.Row row in EnemyParam)
                {
                    row["shield_stability"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Stability_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Stability_Max);
                    row["shield_absorption_physical"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Physical_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Physical_Defence_Max);
                    row["shield_absorption_magic"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Magic_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Magic_Defence_Max);
                    row["shield_absorption_lightning"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Lightning_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Lightning_Defence_Max);
                    row["shield_absorption_fire"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Fire_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Fire_Defence_Max);
                    row["shield_absorption_dark"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Dark_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Dark_Defence_Max);
                    row["shield_absorption_poison"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Poison_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Poison_Defence_Max);
                    row["shield_absorption_bleed"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Bleed_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Bleed_Defence_Max);
                    row["Unk22"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Curse_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Curse_Defence_Max);
                    row["Unk23"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Shield_Petrify_Defence_Min, ScramblerData_Params.Static.EnemyParamData.Shield_Petrify_Defence_Max);
                }
            }
            if (c_EnemyParam_Poise)
            {
                foreach (PARAM.Row row in EnemyParam)
                {
                    row["stat_poise"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Poise_Min, ScramblerData_Params.Static.EnemyParamData.Poise_Max);
                    row["stat_poise_regen"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Poise_Regen_Min, ScramblerData_Params.Static.EnemyParamData.Poise_Regen_Max);
                }
            }
            if (c_EnemyDamageParam_Damage)
            {
                foreach (PARAM.Row row in EnemyDamageParam)
                {
                    row["damage_0"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Damage_A_Min, ScramblerData_Params.Static.EnemyParamData.Damage_A_Max);

                    if (rand.Next(100) < ScramblerData_Params.Static.EnemyParamData.Damage_B_Apply_Chance)
                        row["damage_1"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Damage_B_Min, ScramblerData_Params.Static.EnemyParamData.Damage_B_Max);

                    if (rand.Next(100) < ScramblerData_Params.Static.EnemyParamData.Damage_C_Apply_Chance)
                        row["damage_2"].Value = GetRandomInt(ScramblerData_Params.Static.EnemyParamData.Damage_C_Min, ScramblerData_Params.Static.EnemyParamData.Damage_C_Max);

                    row["stamina_damage_mult"].Value = GetRandomUShort(ScramblerData_Params.Static.EnemyParamData.Stamina_Damage_Min, ScramblerData_Params.Static.EnemyParamData.Stamina_Damage_Max);
                }
            }
            if (c_EnemyDamageParam_Knockback)
            {
                foreach (PARAM.Row row in EnemyDamageParam)
                {
                    row["knockback_amount"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Knockback_Amount_Min, ScramblerData_Params.Static.EnemyParamData.Knockback_Amount_Max);
                    row["knockback_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Knockback_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Knockback_Speed_Max);
                }
            }
            if (c_EnemyDamageParam_AttackSpeed)
            {
                foreach (PARAM.Row row in EnemyBehaviorParam)
                {
                    row["group_1_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_2_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_3_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_4_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_5_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_6_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_7_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_8_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_9_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_10_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_1_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_2_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_3_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_4_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_5_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_6_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_7_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_8_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_9_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_10_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                }
                foreach (PARAM.Row row in EnemyBehaviorSecondParam)
                {
                    row["group_1_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_2_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_3_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_4_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_5_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_6_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_7_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_8_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_9_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_10_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_1_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_2_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_3_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_4_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_5_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_6_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_7_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_8_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_9_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_10_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                }
                foreach (PARAM.Row row in EnemyBehaviorThirdParam)
                {
                    row["group_1_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_2_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_3_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_4_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_5_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_6_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_7_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_8_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_9_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_10_attack_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Attack_Speed_Max);
                    row["group_1_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_2_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_3_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_4_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_5_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_6_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_7_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_8_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_9_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                    row["group_10_recovery_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Recovery_Speed_Max);
                }
            }
            if (c_EnemyMoveParam_Walk)
            {
                List<PARAM.Row> rows = EnemyMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["walk_acceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Walk_Acceleration_Min, ScramblerData_Params.Static.EnemyParamData.Walk_Acceleration_Max);
                    row["walk_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Walk_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Walk_Speed_Max);
                    row["walk_deceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Walk_Deceleration_Min, ScramblerData_Params.Static.EnemyParamData.Walk_Deceleration_Max);
                    row["lock_on_walk_acceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Walk_LockOn_Acceleration_Min, ScramblerData_Params.Static.EnemyParamData.Walk_LockOn_Acceleration_Max);
                    row["lock_on_walk_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Walk_LockOn_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Walk_LockOn_Speed_Max);
                    row["lock_on_walk_deceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Walk_LockOn_Deceleration_Min, ScramblerData_Params.Static.EnemyParamData.Walk_LockOn_Deceleration_Max);
                }
            }
            if (c_EnemyMoveParam_Run)
            {
                List<PARAM.Row> rows = EnemyMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["run_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Run_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Run_Speed_Max);
                    row["run_acceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Run_Acceleration_Min, ScramblerData_Params.Static.EnemyParamData.Run_Acceleration_Max);
                    row["run_to_walk_deceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Run_Deceleration_Min, ScramblerData_Params.Static.EnemyParamData.Run_Deceleration_Max);
                    row["lock_on_run_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Run_LockOn_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Run_LockOn_Speed_Max);
                    row["lock_on_run_acceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Run_LockOn_Acceleration_Min, ScramblerData_Params.Static.EnemyParamData.Run_LockOn_Acceleration_Max);
                    row["lock_on_run_to_walk_deceleration"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Run_LockOn_Deceleration_Min, ScramblerData_Params.Static.EnemyParamData.Run_LockOn_Deceleration_Max);
                }
            }
            if (c_EnemyMoveParam_Jump)
            {
                List<PARAM.Row> rows = EnemyMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["jump_height"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Height_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Height_Max);
                    row["Unk51"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Unk51_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Unk51_Max);
                    row["Unk52"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Unk52_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Unk52_Max);
                    row["jump_length_min"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Length_Minimum_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Length_Minimum_Max);
                    row["jump_length_max"].Value = ((float)row["jump_length_min"].Value * GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Length_Maximum_Multiplier_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Length_Maximum_Multiplier_Max));
                    row["Unk53"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Unk53_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Unk53_Max);
                    row["Unk54"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Unk54_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Unk54_Max);
                    row["jump_gravity_min"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Gravity_Minimum_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Gravity_Minimum_Max);
                    row["jump_gravity_max"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Gravity_Maximum_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Gravity_Maximum_Max);
                    row["Unk55"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Unk55_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Unk55_Max);
                    row["Unk56"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Unk56_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Unk56_Max);
                    row["Unk57"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Jump_Unk57_Min, ScramblerData_Params.Static.EnemyParamData.Jump_Unk57_Max);

                    // Adjust these so the player doesn't just die when they jump high
                    row["fall_distance_breakpoint_1"].Value = ((float)row["jump_height"].Value * 1.2);
                    row["fall_distance_breakpoint_2"].Value = ((float)row["jump_height"].Value * 1.4);
                    row["fall_distance_breakpoint_3"].Value = ((float)row["jump_height"].Value * 1.6);
                    row["fall_distance_max"].Value = ((float)row["jump_height"].Value * 2);
                }
            }
            if (c_EnemyMoveParam_Climb)
            {
                List<PARAM.Row> rows = EnemyMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["ladder_climb_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Climb_Ladder_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Climb_Ladder_Speed_Max);
                    row["ladder_fast_climb_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Climb_Ladder_Fast_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Climb_Ladder_Fast_Speed_Max);
                    row["ladder_slide_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Climb_Ladder_Slide_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Climb_Ladder_Slide_Speed_Max);
                    row["Unk107"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Climb_Unk107_Min, ScramblerData_Params.Static.EnemyParamData.Climb_Unk107_Max);
                    row["Unk108"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Climb_Unk108_Min, ScramblerData_Params.Static.EnemyParamData.Climb_Unk108_Max);
                }
            }
            if (c_EnemyMoveParam_Turn)
            {
                List<PARAM.Row> rows = EnemyMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["slow_walk_turn_rate"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Slow_Walk_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Slow_Walk_Turn_Rate_Max);
                    row["walk_turn_rate"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Walk_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Walk_Turn_Rate_Max);
                    row["Unk08"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk08_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Unk08_Turn_Rate_Max);
                    row["Unk09"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk09_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Unk09_Turn_Rate_Max);
                    row["Unk12"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk12_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Unk12_Turn_Rate_Max);
                    row["Unk13"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk13_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Unk13_Turn_Rate_Max);
                    row["Unk14"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk14_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Unk14_Turn_Rate_Max);
                    row["Unk15"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk15_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Unk15_Turn_Rate_Max);
                    row["lock_on_walk_turn_rate"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Walk_LockOn_Turn_Rate_Min, ScramblerData_Params.Static.EnemyParamData.Walk_LockOn_Turn_Rate_Max);
                }
            }
            if (c_EnemyMoveParam_Evasion)
            {
                List<PARAM.Row> rows = EnemyMoveParam;

                foreach (PARAM.Row row in rows)
                {
                    row["backstep_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Backstep_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Backstep_Speed_Max);
                    row["backstep_distance"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Backstep_Distance_Min, ScramblerData_Params.Static.EnemyParamData.Backstep_Distance_Max);
                    row["roll_speed"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Roll_Speed_Min, ScramblerData_Params.Static.EnemyParamData.Roll_Speed_Max);
                    row["Unk45"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk45_Min, ScramblerData_Params.Static.EnemyParamData.Unk45_Max);
                    row["Unk47"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk47_Min, ScramblerData_Params.Static.EnemyParamData.Unk47_Max);
                    row["Unk49"].Value = GetRandomFloat(ScramblerData_Params.Static.EnemyParamData.Unk49_Min, ScramblerData_Params.Static.EnemyParamData.Unk49_Max);
                }
            }

            return regulation;
        }

        public bool IsDamageParamMatch(string id, List<int> comparison_list)
        {
            bool match = false;

            // Only check if the row ID is within the correct range
            if (id.Length >= 9)
            {
                string truncated_id = id.Substring(1, 4);

                foreach (int row in comparison_list)
                {
                    string row_id = row.ToString();
                    string truncated_row_id = row_id.Substring(0, 4);

                    if (truncated_id == truncated_row_id)
                        match = true;
                }
            }

            return match;
        }

        public bool IsLogicComParamMatch(string id, List<int> comparison_list)
        {
            bool match = false;

            // Only check if the row ID is within the correct range
            if (id.Length >= 5)
            {
                string truncated_id = id.Substring(0, 4);

                foreach (int row in comparison_list)
                {
                    string row_id = row.ToString();
                    string truncated_row_id = row_id.Substring(0, 4);

                    if (truncated_id == truncated_row_id)
                        match = true;
                }
            }

            return match;
        }
    }
}
