using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppEmailBuilder.src.Templates
{
    public class ReadTemplates : IReadTemplates
    {
        private string _defualtFooterTemplate;

        private string _defualtHeaderTemplate;

        private string _defaultBodyTemplate;

        private static object _lockObject = new object();

        private static ReadTemplates _instance = null;
        private ReadTemplates() 
        {
            _defaultBodyTemplate = string.Empty;
            _defualtFooterTemplate = string.Empty;
            _defualtHeaderTemplate = string.Empty;
        }
        public static ReadTemplates GetInstance()
        {
            if(_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ReadTemplates();
                    }
                }
            }
            return _instance;
        }
        
        public string GetDefaultFooterTemplate()
        {
            return _defualtFooterTemplate;
        }

        public string GetDefaultHeaderTemplate()
        {
            return _defualtHeaderTemplate;
        }

        public string GetDefaultBodyTemplate()
        {
            return _defaultBodyTemplate;
        }

        public string GetFullDefaultTemplate()
        {
            throw new NotImplementedException();
        }
    }
}
