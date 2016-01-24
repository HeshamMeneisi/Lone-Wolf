using System;
using System.Collections.Generic;

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
            // TODO: implement data parsing

            Encrypted = false;
        }

        internal void LoadRawData()
        {
            if (Encrypted) DecryptStrings();
            string[] data = SData.Split('|');
            // TODO: Implement data parsing
        }        
        public bool Encrypted { get; private set; }
        internal GameStateHolder GameState { get; set; }

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
