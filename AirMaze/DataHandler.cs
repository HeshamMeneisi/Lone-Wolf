using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
#if WP81

#else
using System.IO.IsolatedStorage;
#endif
using System.Xml.Serialization;
#if WINDOWS_UAP || WP81
using System.Net.Http;
using Windows.Storage;
#endif

namespace Utilities
{
    class DataHandler
    {
        static SmartContentManager UIContent = null;

        internal const int TextureUnitDim = 256;
        // Files
        // The index in this array represents the groupindex used in TextureID
        private static Dictionary<PrimaryTexture, string> TextureFiles = new Dictionary<PrimaryTexture, string>();

        private static Dictionary<FontType, string> FontFiles = new Dictionary<FontType, string>();

        private static Dictionary<SoundType, string> SoundFiles = new Dictionary<SoundType, string>();

        static ThemeType loadedtheme = ThemeType.Beach;
        static bool first = true;
        internal static void LoadCurrentTheme()
        {
            if (first) first = false;
            else if (loadedtheme == Manager.GameSettings.CurrentTheme) return;

            if (UIContent != null)
            {
                UIContent.Unload();
                UIContent.Dispose();
                GC.Collect();
            }
            UIContent = new SmartContentManager(Manager.RandomAccessContentManager.ServiceProvider);
            UIContent.RootDirectory = Manager.RandomAccessContentManager.RootDirectory;
            string d = GetCurrentThemeDirectory();

            TextureFiles = new Dictionary<PrimaryTexture, string>();
            foreach (PrimaryTexture t in PrimaryTexture.GetValues(typeof(PrimaryTexture)))
                TextureFiles.Add(t, "Textures\\" + d + "\\" + t.ToString());
            LoadTextures();

            SoundFiles.Clear();
            foreach (SoundType s in SoundType.GetValues(typeof(SoundType)))
                SoundFiles.Add(s, "Sounds\\" + d + "\\" + s.ToString());
            LoadSounds();

            FontFiles.Clear();
            foreach (FontType f in FontType.GetValues(typeof(FontType)))
                FontFiles.Add(f, "Fonts\\" + d + "\\" + f.ToString());
            LoadFonts();
            loadedtheme = Manager.GameSettings.CurrentTheme;
            SoundManager.StopAllLoops();
            SoundManager.PlaySound(DataHandler.Sounds[SoundType.Background], SoundCategory.Music, true);
            Manager.SaveUserDataLocal();

            GC.Collect();
        }

        private static string GetCurrentThemeDirectory()
        {
            return Manager.GameSettings.CurrentTheme.ToString();
        }

        internal static Texture2D getTexture(PrimaryTexture key)
        {
            return getTexture(key.ToString());
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        internal static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

        internal static Dictionary<FontType, SpriteFont> Fonts = new Dictionary<FontType, SpriteFont>();

        internal static Dictionary<SoundType, SoundEffect> Sounds = new Dictionary<SoundType, SoundEffect>();
        internal static string UIKey { get { return PrimaryTexture._UI.ToString(); } }
        internal static string ObjKey { get { return PrimaryTexture._Obj.ToString(); } }
        /// <summary>
        /// We should define new tiles here.
        /// </summary>

        /// <summary>
        /// We should define ui objects here.
        /// </summary>
        #region UI Items
        internal static Dictionary<UIObjectType, TextureID2D[]> UIObjectsTextureMap = new Dictionary<UIObjectType, TextureID2D[]>()
        {
            {UIObjectType.PlayBtn, new TextureID2D[] {new TextureID2D(UIKey,17, 1, 0.5f) } },
            {UIObjectType.EditModeBtn, new TextureID2D[] {new TextureID2D(UIKey,4, 1, 0.5f) } },
            {UIObjectType.OptionsBtn,new TextureID2D[] {new TextureID2D(UIKey,8, 1, 0.5f) } },
            {UIObjectType.Cell, new TextureID2D[] {new TextureID2D(UIKey,1)} },
            {UIObjectType.MenuButton,new TextureID2D[] {new TextureID2D(UIKey,12, 1, 0.5f) } },
            {UIObjectType.ResetButton,new TextureID2D[] {new TextureID2D(UIKey,13, 1, 0.5f) } },
            {UIObjectType.ToggleButton,new TextureID2D[] {new TextureID2D(UIKey,14, 1, 0.5f) } },
            {UIObjectType.SaveButton,new TextureID2D[] {new TextureID2D(UIKey,15, 1, 0.5f)} },
            {UIObjectType.Next,new TextureID2D[] {new TextureID2D(UIKey,18, 2, 1)} },
            {UIObjectType.TryAgain,new TextureID2D[] {new TextureID2D(UIKey,22, 2, 1)} },
            {UIObjectType.BackButton,new TextureID2D[] {new TextureID2D (UIKey,11, 1, 0.5f) } },
            {UIObjectType.MainUser,new TextureID2D[] {new TextureID2D(UIKey,7, 1, 0.5f)} },
            {UIObjectType.DeleteBtn,new TextureID2D[] {new TextureID2D(UIKey,16, 1, 0.5f)} },
            {UIObjectType.LeftButton,new TextureID2D[] {new TextureID2D(UIKey,5, 0.5f,0.5f)} },
            {UIObjectType.RightButton,new TextureID2D[] {new TextureID2D(UIKey, 6, 0.5f, 0.5f) } },
            {UIObjectType.UpButton,new TextureID2D[] {new TextureID2D(UIKey, 9, 0.5f, 0.5f) } },
            {UIObjectType.DownButton,new TextureID2D[] {new TextureID2D(UIKey, 10, 0.5f, 0.5f) } },
            {UIObjectType.Star,new TextureID2D[] {new TextureID2D(UIKey, 20, 1, 1), new TextureID2D(UIKey, 21, 1, 1) } },
            {UIObjectType.Log,new TextureID2D[] {new TextureID2D(UIKey,26,2,1)} },
            {UIObjectType.Lock,new TextureID2D[] {new TextureID2D(UIKey,38,2,2)} },
            {UIObjectType.Border,new TextureID2D[] {new TextureID2D(UIKey,24,2,1)} },
            {UIObjectType.TopLog,new TextureID2D[] {new TextureID2D(UIKey,30,2,1),new TextureID2D(UIKey,28,2,1)} },
            {UIObjectType.ShareBtn,new TextureID2D[] {new TextureID2D(UIKey,32,1,1)} },
            {UIObjectType.Ropes,new TextureID2D[] {new TextureID2D(UIKey,34,2,1)} },
            {UIObjectType.Frame,new TextureID2D[] {new TextureID2D(UIKey,36,2,2)} },
            {UIObjectType.FBBtn,new TextureID2D[] {new TextureID2D(UIKey,44,2,1)} }
        };


        #endregion

#if WP81
        static StorageFolder savegameStorage = ApplicationData.Current.LocalFolder;
#else
        static IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

        internal static IEnumerable<string> getSavedLevelNames()
        {
#if WP81
            foreach (StorageFile f in savegameStorage.GetFilesAsync().AsTask().ConfigureAwait(false).GetAwaiter().GetResult())
                if (f.Name.StartsWith("S_")) yield return f.Name.Split('.')[0].Split('_')[1];
#else
            foreach (string s in savegameStorage.GetFileNames())
                if (s.StartsWith("S_")) yield return s.Split('.')[0].Split('_')[1];
#endif
        }
        internal static void LoadTextures()
        {
            foreach (PrimaryTexture t in TextureFiles.Keys)
            {
                string key = t.ToString();
                if (!Textures.ContainsKey(key))
                    Textures.Add(key, UIContent.Load<Texture2D>(TextureFiles[t]));
                else
                {
                    Textures[key] = UIContent.Load<Texture2D>(TextureFiles[t]);
                }
                GC.Collect();
            }
        }
        internal static void LoadSounds()
        {
            foreach (SoundType p in SoundFiles.Keys)
                if (!Sounds.ContainsKey(p))
                    Sounds.Add(p, Manager.RandomAccessContentManager.Load<SoundEffect>(SoundFiles[p]));
                else
                {
                    Sounds[p] = Manager.RandomAccessContentManager.Load<SoundEffect>(SoundFiles[p]);
                }
        }
        internal static void LoadFonts()
        {
            foreach (FontType f in FontFiles.Keys)
                if (!Fonts.ContainsKey(f))
                    Fonts.Add(f, Manager.RandomAccessContentManager.Load<SpriteFont>(FontFiles[f]));
                else Fonts[f] = Manager.RandomAccessContentManager.Load<SpriteFont>(FontFiles[f]);
        }

        internal static Rectangle getTextureSource(TextureID2D id)
        {
            // Textures are expected to be square
            if (Textures[id.RefKey] == null) return new Rectangle(0, 0, 0, 0);
            int unitsperrow = Textures[id.RefKey].Width / TextureUnitDim;
            int texw = (int)(TextureUnitDim * id.WidthUnits);
            int texh = (int)(TextureUnitDim * id.HeightUnits);
            return new Rectangle(id.Index % unitsperrow * TextureUnitDim + 1, id.Index / unitsperrow * TextureUnitDim + 1, texw - 1, texh - 1);
        }

        internal static Texture2D getTexture(string key)
        {
            return Textures[key];
        }
        internal static Texture2D getTexture(TextureID2D tid)
        {
            return Textures[tid.RefKey];
        }
        internal static void SaveData<T>(T data, string file)
        {
#if WP81    
            StorageFile f;
            int tries = 0;
            Stream str;
            retry:
            try
            {
                f = savegameStorage.CreateFileAsync(file, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                str = f.OpenStreamForWriteAsync().Result;
            }
            catch
            {
                if (tries > 10)
                {
                    Debug.WriteLine("__ALERT__: Failed to save file:" + file);
                    return;
                }
                tries++;
                goto retry;        
            }
#else
            IsolatedStorageFileStream str = savegameStorage.CreateFile(file);
#endif
            string path = "Unkown";
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(str, data);
#if ANDROID
            path = "temp/Inlumino/" + file;
            saveExternal(str, path);
#endif
#if WINDOWS_UAP && DEBUG // Causes crashes after release
            path = str.GetType().GetField("m_FullPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(str).ToString();
#endif
            Debug.WriteLine("______ALERT______SavedFileTo: " + path);
            str.Dispose();
        }

        private static void saveExternal(Stream str, string file)
        {
            string pathToFile = "";
            str.Seek(0, SeekOrigin.Begin);
#if ANDROID
            pathToFile = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, file);
#endif
#if WINDOWS
            pathToFile = file;
#endif
            if (!Directory.Exists(Path.GetDirectoryName(pathToFile))) Directory.CreateDirectory(Path.GetDirectoryName(pathToFile));
            if (File.Exists(pathToFile)) File.Delete(pathToFile);
            if (pathToFile != "")
                using (var fileStream = new FileStream(pathToFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    int read;
                    byte[] buffer = new byte[1024];
                    while ((read = str.Read(buffer, 0, buffer.Length)) > 0)
                        fileStream.Write(buffer, 0, read);
                }
        }

        /// <summary>
        /// Returns default(T) on failure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="debugmode"></param>
        /// <returns></returns>
        internal static T LoadData<T>(string file)
        {
#if WP81
            StorageFile f = null;
            try { f = savegameStorage.GetFileAsync(file).AsTask().ConfigureAwait(false).GetAwaiter().GetResult(); } catch { return default(T); }
            if (f == null) return default(T);
            using (Stream str = f.OpenStreamForReadAsync().Result)
#else
            if (!savegameStorage.FileExists(file)) return default(T);

            using (Stream str = savegameStorage.OpenFile(file, FileMode.Open))
#endif
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                try
                {
                    return (T)serializer.Deserialize(str);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    return default(T);
                }
            }
        }
        /// <summary>
        /// Gets the group index using the file name.
        /// </summary>
        /// <param name="name">File name without extension. (e.g "ObjectTex")</param>
        /// <returns></returns>
        internal static string LoadTexture(string name, Func<Texture2D> textureretriever = null)
        {
            if (Textures.ContainsKey(name)) return name;
            if (textureretriever != null)
            { Textures.Add(name, textureretriever()); return name; }
            try
            {
                Texture2D temp = Manager.RandomAccessContentManager.Load<Texture2D>(name);
                if (temp != null)
                    Textures.Add(name, temp);
                return name;
            }
            catch { return ""; }
        }
        internal static bool isValid(TextureID2D tid)
        {
            return tid != default(TextureID2D) && Textures.ContainsKey(tid.RefKey);
        }
    }
    internal enum SoundType { TapSound = 0, RotateSound = 1, CrystalLit = 2, AllCrystalsLit = 3, Background = 4 }
}
public enum ThemeType { Beach = 0, Space = 1 }
public enum FontType { MainFont = 0 }

public enum PrimaryTexture { _UI = 0, _Obj = 1, _BG = 2, _MMBG = 3, _Aux = 4 }