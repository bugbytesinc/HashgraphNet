#pragma warning disable IDE1006 // Private Static Helper Functions Start with Lower Case
using System;

namespace Hashgraph.NetCore
{
    public partial class Client
    {
        private ContextStack createChildContext(Action<IContext>? configure)
        {
            var context = new ContextStack(_context);
            configure?.Invoke(context);
            return context;
        }
    }
}
