using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Entities;
using Digital_World.Database;
using Digital_World.Helpers;
using System.IO;
using Org.BouncyCastle.Bcpg;

namespace Digital_World.Packets.Game
{
    public class CharInfo : Packet
    {
        public CharInfo(Character Tamer)
        {
            packet.Type(0x03EB);
            packet.WriteInt(1);
            packet.WriteInt(Tamer.Location.PosX);
            packet.WriteInt(Tamer.Location.PosY);
            packet.WriteInt(Tamer.TamerHandle);
            packet.WriteInt((int)Tamer.Model);
            packet.WriteString(Tamer.Name);
            packet.WriteInt(Tamer.EXP);
            packet.WriteInt(0);
            packet.WriteShort((short)Tamer.Level);
            packet.WriteInt(Tamer.MaxHP); // Max HP
            packet.WriteInt(Tamer.MaxDS); // Max DS
            packet.WriteInt(Tamer.HP); // HP
            packet.WriteInt(Tamer.DS); // DS
            packet.WriteInt(Tamer.Fatigue); // Fatigue
            packet.WriteInt(Tamer.AT); // AT
            packet.WriteInt(Tamer.DE); // DE
            packet.WriteInt(Tamer.MS); // MS
            packet.WriteBytes(Tamer.Equipment.ToArray()); //884
            packet.WriteBytes(Tamer.ChipSets.ToArray()); //816
            packet.WriteBytes(Tamer.Digivice.ToArray()); //68
            for (int i = 0; i < 1248; i++)  //1317
            {
                packet.WriteByte(0);
            }
            packet.WriteInt(Tamer.Incubator);
            packet.WriteInt(Tamer.IncubatorLevel);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort(0x03);
            packet.WriteInt(115659);
            packet.WriteInt(1607453999);
            packet.WriteInt(2700024);
            packet.WriteInt(115657);
            packet.WriteInt(1607453999);
            packet.WriteInt(2700022);
            packet.WriteInt(115658);
            packet.WriteInt(1607453999);
            packet.WriteInt(2700023);
            Digimon_Partner(Tamer.Partner, 0);
            packet.WriteInt(1);
            packet.WriteBytes(new byte[54]);
            packet.WriteInt(16618);
            packet.WriteByte(0);
            packet.WriteInt(658787);
            packet.WriteShort(0);
            packet.WriteInt(0x05);
            packet.WriteBytes(new byte[192]);
            packet.WriteInt(1);
            packet.WriteBytes(new byte[11]);
            packet.WriteInt(0x63);
            packet.WriteBytes(new byte[159]);
        }

        private void Digimon_Partner(Digimon Mon, int slot)
        {
            packet.WriteByte(0x03); //SLOTS OPEN FOR MERC!
            packet.WriteInt(Mon.Handle);
            //Console.WriteLine(Mon.byteHandle);
            packet.WriteInt(Mon.Species);
            //Console.WriteLine(Tamer.Partner.Model);
            packet.WriteString(Mon.Name); //Partner Name
            //packet.WriteByte(0); // Scale
            packet.WriteByte((byte)Mon.Scale);
            packet.WriteShort(Mon.Size); //Partner Size
            packet.WriteInt(Mon.EXP); //Partner EXP
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort((short)Mon.Level); //Partner Level
            packet.WriteInt(Mon.Stats.MaxHP); // Max HP
            packet.WriteInt(Mon.Stats.MaxDS); // Max MP
            packet.WriteInt(Mon.Stats.DE); //DE
            packet.WriteInt(Mon.Stats.AT); //AT
            packet.WriteInt(Mon.Stats.HP); // HP
            packet.WriteInt(Mon.Stats.DS); // MP
            packet.WriteInt(Mon.Stats.Intimacy); // Intimacy
            packet.WriteInt(Mon.Stats.BL); //BL
            packet.WriteInt(Mon.Stats.EV);
            packet.WriteInt(Mon.Stats.CR);
            packet.WriteInt(Mon.Stats.MS);
            packet.WriteInt(Mon.Stats.AS);
            packet.WriteInt(80);
            packet.WriteInt(Mon.Stats.HT); //HT //EV //CR //MS //AS
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(Mon.Species);
            packet.WriteByte((byte)Mon.Forms.Count); //->EVOLUTIONS
            for (int i = 0; i < Mon.Forms.Count; i++)
            {
                EvolvedForm form = Mon.Forms[i];

                //EVOLUTER

                //UNLOCKED EVOLUTIONS 
                if (Mon.levels_unlocked > i)
                {

                    form.skill_max_level = 0x19;

                }
                else
                //DEFAULT NOT UNLOCKED
                {

                    form.skill_max_level = 0x0A;

                }

                form.skill_points = 0x0A; //SKILL POINTS - > 10
                form.skill1_level = 0x01; //1ST SKILL LEVEL
                form.skill2_level = 0x01; //2ND SKILL LEVEL
                form.skill3_level = 0x01;
                form.skill4_level = 0x01;
                form.skill5_level = 0x01;

                /*
                 * 1F
                 * 01
                 * 02
                 * 08
                 */

                /*EvolvedForm form = Mon.Forms[i];
                form.uByte5 = 0x1d; //->
                form.uByte4 = 0x34; //-> 
                form.b128 = 129;
                form.b0 = 0x95;*/
                //form.Skill1 = 8;
                //form.Skill2 = 8;

                packet.WriteBytes(form.ToArray());
            }

        }
        private void Digimon_Mercs(Digimon Mon, int slot)
        {
            packet.WriteByte((byte)slot); //SLOT
            //Console.WriteLine(Mon.byteHandle);
            packet.WriteUInt(Mon.Model); // -> SEEMS ALRIGHT FOR NOW BUT IT USES MERC HANDLE + INT
            //Console.WriteLine(Tamer.Partner.Model);
            packet.WriteString(Mon.Name); //Partner Name
            packet.WriteByte(0x05); // Scale
            packet.WriteShort(Mon.Size); //Partner Size
            packet.WriteInt(Mon.EXP); //Partner EXP - > Mon.EXP
            packet.WriteInt(0);
            packet.WriteShort((short)Mon.Level); //Partner Level
            packet.WriteInt(Mon.Stats.MaxHP); // Max HP
            packet.WriteInt(Mon.Stats.MaxDS); // Max MP
            packet.WriteInt(Mon.Stats.DE); //DE
            packet.WriteInt(Mon.Stats.AT); //AT
            packet.WriteInt(Mon.Stats.HP); // HP
            packet.WriteInt(Mon.Stats.DS); // MP
            packet.WriteInt(Mon.Stats.Intimacy); // Intimacy
            packet.WriteInt(Mon.Stats.BL); //BL
            packet.WriteInt(Mon.Stats.EV);
            packet.WriteInt(Mon.Stats.CR);
            packet.WriteInt(Mon.Stats.MS);
            packet.WriteInt(Mon.Stats.AS);
            packet.WriteInt(80);
            packet.WriteInt(Mon.Stats.HT); //HT //EV //CR //MS //AS
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(Mon.Species);
            //Console.WriteLine(Mon.Species);
            packet.WriteByte((byte)Mon.Forms.Count); //->EVOLUTIONS

            //packet.WriteBytes(new byte[112]);

            for (int i = 0; i < Mon.Forms.Count; i++)
            {
                /*CHAMPION UNLOCKED
                EvolvedForm form = Mon.Forms[i];
                form.uByte5 = 0x02;
                form.uByte4 = 0x01;
                form.b128 = 0x0A; //FOUND 
                form.b0 = 0x01; //FOUND 
                */
                
                 
                
                EvolvedForm form = Mon.Forms[i];

                //EVOLUTER
                if (Mon.levels_unlocked > i)
                {

                    form.skill_max_level = 0x11;

                }
                else
                {

                    form.skill_max_level = 0x01;

                }
                form.skill_points = 0x01; //SKILL POINTS - > 10
                form.skill1_level = 0x01; //1ST SKILL LEVEL
                form.skill2_level = 0x01; //2ND SKILL LEVEL 

                packet.WriteBytes(form.ToArray());
            }

            packet.WriteBytes(new byte[49]);

        }
     }

    public class CharInfo2 : Packet
    {
        public CharInfo2(Character Tamer)
        {
            packet.Type(0x041E);
            packet.WriteInt(1);
            packet.WriteInt(1200);
            packet.WriteInt(1);
            packet.WriteInt(1200);
            packet.WriteInt(1400);
            packet.WriteInt(793126);
            packet.WriteInt(0x0535);
            packet.WriteInt(0);
            packet.WriteInt(6036016);
            packet.WriteShort(0x0413);
            packet.WriteInt(0x5A);
            packet.WriteInt(0x50);
            packet.WriteInt(0x5A);
            packet.WriteInt(0x50);


            packet.WriteShort((short)Tamer.InventorySize);
            packet.WriteBytes(Tamer.Inventory.ToArray());


            packet.WriteShort((short)Tamer.WarehouseSize);
            packet.WriteBytes(Tamer.Warehouse.ToArray());
        }
    }
}



