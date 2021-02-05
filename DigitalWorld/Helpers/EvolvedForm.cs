using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Digital_World.Helpers
{
    [Serializable]
    public class EvolvedForms
    {
        private EvolvedForm[] m_coll;

        public EvolvedForms()
        {
            m_coll = new EvolvedForm[4];
            for (int i = 0; i < m_coll.Length; i++)
                m_coll[i] = new EvolvedForm();
        }

        /// <summary>
        /// CHANGED!!!! int to uint
        /// </summary>
        /// <param name="count"></param>
        public EvolvedForms(int count)
        {
            m_coll = new EvolvedForm[count];
            for (int i = 0; i < m_coll.Length; i++)
                m_coll[i] = new EvolvedForm();
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(m, this);

                buffer = m.ToArray();
            }
            return buffer;
        }

        public int Count
        {
            get
            {
                return m_coll.Length;
            }
        }

        public EvolvedForm this[int idx]
        {
            get
            {
                return m_coll[idx];
            }
            set
            {
                m_coll[idx] = value;
            }
        }
    }

    [Serializable]
    public class EvolvedForm
    {
        public short[] uShorts1;
        public byte skill_points, skill1_level, skill2_level, skill3_level, skill4_level, skill5_level, skill_max_level, skill_mastery_level, uByte3, Skill1, Skill2, Skill3, Skill4, Skill5;
        public short uShort1;
        public int skill_EXP;

        //->NEW
        public byte[] bytes;
        //->NEW

        public EvolvedForm()
        {
            /*uShort1 = 0;
            uShorts1 = new short[24];
            b128 = 128;
            Skill1 = 1;
            Skill2 = 1;
            */

            bytes = new byte[13]; //13 BYTES ARE needed
            
        }

        public EvolvedForm(byte[] Unknowns, byte[] Skills)
        {
            skill2_level = Unknowns[0];
            skill_max_level = Unknowns[1];
            uByte3 = Unknowns[2];

            Skill1 = Skills[0];
            Skill2 = Skills[1];
            Skill3 = Skills[2];
            Skill4 = Skills[3];
            Skill5 = Skills[4];
        }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {

                /*for (int i = 0; i < 3; i++)
                {
                    m.WriteByte(0x00);
                }*/
                m.WriteByte((byte)skill_EXP);
                for (int i = 0; i < 2; i++)
                {
                     m.WriteByte(0x0);
                }
                m.WriteByte(0x4);
                m.WriteByte(0x1);
                for (int i = 0; i < 3; i++)
                {
                    m.WriteByte(0x0);
                }
                m.WriteByte(skill_points);
                m.WriteByte(skill1_level);
                m.WriteByte(skill2_level);
                m.WriteByte(skill3_level);
                m.WriteByte(skill4_level);
                m.WriteByte(skill5_level);
                m.WriteByte(0x0D);
                m.WriteByte(skill_max_level);
                m.WriteByte(0x0D);
                m.WriteByte(skill_max_level);
                m.WriteByte(0x0D);
                m.WriteByte(skill_max_level);
                m.WriteByte(0x0D);
                m.WriteByte(skill_max_level);
                m.WriteByte(0x0D);
                m.WriteByte(skill_max_level);


                buffer = m.ToArray();
            }
            return buffer;
        }
    }
}
