using System;
using System.Collections.Generic;
using System.Text;

namespace Logicka.Core.Entities
{
    [Serializable]
    public class LSyntagm : IEquatable<LSyntagm>
    {
        private string _token;


        public LSyntagm(string text)
        {
            this._token = text.Trim();

            this.Parents = new List<LSyntagm>();
            this.Children = new List<LSyntagm>();
        }

        public LSyntagm(List<LSyntagm> children)
        {
            this.Parents = new List<LSyntagm>();
            this.Children = children;

            foreach (var child in this.Children)
                if (!child.Parents.Contains(this))
                    child.Parents.Add(this);
        }


        public List<LSyntagm> Parents { get; private set; }
        public List<LSyntagm> Children { get; private set; }


        public override string ToString()
        {
            if (this._token != null) return this._token;

            StringBuilder sb = new StringBuilder();

            foreach(var child in this.Children)
            {
                sb.Append(child.ToString());
                sb.Append(" ");
            }

            sb.Length--;

            return sb.ToString();
        }

        public bool Equals(LSyntagm obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (this._token == null ^ obj._token == null) return false;

            bool containsSameElements = this.ContainsChildren(obj);

            if (string.IsNullOrEmpty(this._token))
                return obj.Children.Count == this.Children.Count && containsSameElements;
            else
                return obj._token.Equals(this._token);
        }


        private bool ContainsChildren(LSyntagm compared)
        {
            bool containsSameElements = true;

            // Check if compared children contain every child from this syntagm
            // (other than joker character).
            foreach (var child in this.Children)
            {
                // Joker is escaped in query syntagms.
                if (child.ToString().Equals(Constants.SAFE_JOKER_TOKEN))
                    continue;

                bool containsElement = false;
                foreach (var objChild in compared.Children)
                {
                    if (child.Equals(objChild))
                    {
                        containsElement = true;
                        break;
                    }

                    containsElement = false;
                }

                if (!containsElement)
                {
                    containsSameElements = false;
                    break;
                }
            }

            return containsSameElements;
        }
    }
}
