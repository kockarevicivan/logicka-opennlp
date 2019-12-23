using Logicka.Core.Entities;
using System.Collections.Generic;

namespace Logicka.WebAPI.Factories
{
    public class ContextFactory
    {
        private static ContextFactory _instance;
        private Dictionary<string, LHippocampus> _contexts;

        private ContextFactory()
        {
            _contexts = new Dictionary<string, LHippocampus>();
        }

        public LHippocampus GetContext()
        {
            string userId = "random_id";

            if (_contexts.ContainsKey(userId))
            {
                return _contexts[userId];
            }
            else
            {
                LHippocampus userContext = new LHippocampus();
                _contexts.Add(userId, userContext);

                return userContext;
            }
        }

        public static ContextFactory GetInstance()
        {
            return _instance = _instance ?? new ContextFactory();
        }
    }
}