using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aabViewer
{
    public class SignManager
    {
        public static SignManager I;

        public static void Create()
        {
            if(I==null)
            {
                I = new SignManager();
                I.Init();
            }
        }
        public string filePath = "";
        public Dictionary<string, KeyNode> SignCache;
        public void Init()
        {
            filePath = Path.Combine(WinformTools.GetCurrentPath(), Define.SignDataFile);

            if(File.Exists(filePath))
            {
                SignCache = JsonMapper.ToObject<Dictionary<string, KeyNode>>(File.ReadAllText(filePath, Encoding.UTF8));
            }
            else
            {
                SignCache = new Dictionary<string, KeyNode>();
            }
        }

        public void Save()
        {
            File.WriteAllText(filePath, JsonMapper.ToJson(SignCache), Encoding.UTF8);
        }

        public void UpdateSign(string package, Form1 form)
        {
            string alias = form.KeyAlias;
            string alias_password = form.AliasPass;
            string password = form.KeyPass;
            string _path = form.KeyPath;

            if (SignCache.TryGetValue(package, out var _node))
            {
                _node.name = alias;
                _node.alias = alias;
                _node.alias_password = alias_password;
                _node.password = password;
                _node.path = _path;
            }
            else
            {
                _node = new KeyNode();
                _node.name = alias;
                _node.alias = alias;
                _node.alias_password = alias_password;
                _node.password = password;
                _node.path = _path;

                SignCache.Add(package, _node);
            }
            Save();
        }

        public void Match(Form1 form, string _package)
        {
            if (SignCache.TryGetValue(_package, out var _node))
            {
                form.UpdateKey(_node);
            }
        }
    }
}
