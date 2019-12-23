using Logicka.Core.Bridges;
using Logicka.Core.Helpers;
using System.Collections.Generic;

namespace Logicka.Core.Entities
{
    // Facade for everything related to logicka
    public class LHippocampus
    {
        private LContext _generalContext;
        private OpenNLPBridge _bridge;


        public LHippocampus()
        {
            _generalContext = new LContext();
            _bridge = new OpenNLPBridge();
        }


        public LSyntagm Submit(string statement)
        {
            return _bridge.GetSyntagmFromText(statement);
        }

        public List<LSyntagm> Query(string query)
        {
            LSyntagm querySyntagm = _bridge.GetQuerySyntagmFromText(query);
            
            return _generalContext.Match(querySyntagm);
        }
        

        public void SaveToFile(string filePath)
        {
            SerializerHelper.SerializeObject<LContext>(this._generalContext, filePath);
        }

        public void LoadFromFile(string filePath)
        {
            this._generalContext = SerializerHelper.DeserializeObject<LContext>(filePath);
        }


        public LSyntagm ResolvePrim(string token)
        {
            if (_generalContext.Primaries.ContainsKey(token))
            {
                return _generalContext.Primaries[token];
            }
            else
            {
                LSyntagm newPrim = new LSyntagm(token);
                _generalContext.Primaries.Add(token, newPrim);

                return newPrim;
            }
        }
    }
}