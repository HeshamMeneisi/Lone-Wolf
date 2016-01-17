using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LoneWolf
{
    public class UserData
    {
        public DateTime _timestamp = DateTime.Now;
        public string SData = "";
        public UserData()
        { Encrypted = true; }
        /// <summary>
        /// Interfacing properties
        /// </summary>      
        internal void UpdateFrom(UserData online)
        {
            _timestamp = DateTime.Now;
            throw new NotImplementedException();
        }
        internal void UpdateRawData()
        {
            _timestamp = DateTime.Now;

            // package lock state
            List<string> data = new List<string>();
            throw new NotImplementedException();

            Encrypted = false;
        }

        internal void LoadRawData()
        {
            if (Encrypted) DecryptStrings();
            string[] data = SData.Split('|');
            throw new NotImplementedException();
        }        
        [XmlIgnore]
        public bool Encrypted { get; private set; }
        internal void EncryptStrings()
        {
            if (Encrypted) return;
            SData = SecurityProvider.Encrypt(SData);
            Encrypted = true;
        }
        internal void DecryptStrings()
        {
            if (!Encrypted) return;
            SData = SecurityProvider.Decrypt(SData);
            Encrypted = false;
        }
    }
}
