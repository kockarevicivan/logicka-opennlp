using System;
using System.Collections.Generic;
using System.Linq;

namespace Logicka.Core.Entities
{
    [Serializable]
    public class LContext
    {
        public Dictionary<string, LSyntagm> Primaries = new Dictionary<string, LSyntagm>();


        public List<LSyntagm> Match(LSyntagm querySyntagm)
        {
            LSyntagm jokerSyntagm = this.FindJokerStart(querySyntagm);
            string[] startingPrimWords = jokerSyntagm.ToString().Split(' ').Where(w => w != Constants.SAFE_JOKER_TOKEN).ToArray();

            List<LSyntagm> prims = new List<LSyntagm>();

            foreach (string word in startingPrimWords)
                if (this.Primaries.ContainsKey(word))
                    prims.Add(this.Primaries[word]);

            List<LSyntagm> primParents = new List<LSyntagm>();

            foreach (var prim in prims)
            {
                foreach (var parent in prim.Parents)
                {
                    if (!primParents.Contains(parent) && jokerSyntagm.Equals(parent))
                        primParents.Add(parent);
                }
            }

            List<LSyntagm> finalResult = new List<LSyntagm>();

            finalResult.AddRange(this.RecursiveQuery(jokerSyntagm, primParents));

            return finalResult.Distinct().ToList();
        }

        private List<LSyntagm> RecursiveQuery(LSyntagm jokerSyntagm, List<LSyntagm> currentMatches)
        {
            List<LSyntagm> result = new List<LSyntagm>();
            LSyntagm jokerParent = jokerSyntagm.Parents.FirstOrDefault();// Always one parent in question tree.
            List<LSyntagm> currentParents = new List<LSyntagm>();

            if (jokerParent == null)
                return currentMatches;

            foreach (var match in currentMatches)
            {
                currentParents.AddRange(match.Parents);
            }

            currentParents = currentParents.Distinct().ToList();

            foreach (var parent in currentParents)
            {
                if (jokerParent.Equals(parent))
                    result.Add(parent);
            }

            return RecursiveQuery(jokerParent, result);
        }

        private LSyntagm FindJokerStart(LSyntagm querySyntagm)
        {
            if (querySyntagm.ToString() == Constants.SAFE_JOKER_TOKEN)
                return querySyntagm.Parents.First();

            foreach (var child in querySyntagm.Children)
            {
                LSyntagm result = FindJokerStart(child);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
