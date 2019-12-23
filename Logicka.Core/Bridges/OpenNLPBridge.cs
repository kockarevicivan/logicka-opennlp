using Logicka.Core.Entities;
using OpenNLP.Tools.Parser;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Logicka.Core.Bridges
{
    public class OpenNLPBridge
    {
        private EnglishTreebankParser _parser;


        public OpenNLPBridge()
        {
            _parser = new EnglishTreebankParser(ConfigurationManager.AppSettings["ModelsLocation"]);
        }


        public LSyntagm GetSyntagmFromText(string submitStatement)
        {
            Parse parse = _parser.DoParse(submitStatement).GetChildren()[0];

            return GetSyntagmFromParse(parse, new LHippocampus());
        }

        public LSyntagm GetQuerySyntagmFromText(string query)
        {
            Parse parse = _parser.DoParse(query.Replace(Constants.JOKER_TOKEN, Constants.SAFE_JOKER_TOKEN)).GetChildren()[0];

            return GetSyntagmFromParse(parse, null);
        }


        private LSyntagm GetSyntagmFromParse(Parse parse, LHippocampus context)
        {
            Parse[] parseChildren = parse.GetChildren();
            List<LSyntagm> children = new List<LSyntagm>();

            if (parseChildren.Length <= 1)
                return context?.ResolvePrim(parse.Value) ?? new LSyntagm(parse.Value);

            foreach (var child in parseChildren)
                children.Add(GetSyntagmFromParse(child, context));

            return GetSyntagmFromChildren(children);
        }

        private LSyntagm GetSyntagmFromChildren(List<LSyntagm> children)
        {
            // Resolve composite syntagm.
            LSyntagm compositeSyntagm = new LSyntagm(children);

            foreach (var child in children)
            {
                LSyntagm found = child.Parents.Where(s => s.Equals(compositeSyntagm)).FirstOrDefault();

                if (found != null) compositeSyntagm = found;
                else child.Parents.Add(compositeSyntagm);
            }

            return compositeSyntagm;
        }
    }
}
