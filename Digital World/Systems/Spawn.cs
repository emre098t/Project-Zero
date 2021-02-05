using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digital_World.Entities;
using Digital_World.Database;
using Digital_World.Packets.Game;

namespace Digital_World.Systems
{
    public partial class Yggdrasil
    {
        public void Spawn()
        {
            int i = 0;
            int j = 0;
            uint value = 200058;
            while(i < 20){
           // uint.TryParse(cmd[1], out value);
            MDBDigimon Mob = MonsterDB.GetDigimon(value);

            uint id = GetModel((uint)(64 + (Mob.Models[0] * 128)) << 8);
            GameMap cMap = Maps[105];


            cMap.Send(new SpawnObject(id, (32687 + j), (26473) + j));
            i++;
            j = j + 100;
            }           
        }

    }
}
