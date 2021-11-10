using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using BaseX;
namespace FaoLogiX.NeosArrayFixX
{
    [ImplementableClass(true)]
    static class Hooker
    {
        private static Type __connectorType;
        private static Type __connectorTypes;

        static Hooker()
        {
            try
            {
                UniLog.Log("Started FaoLogiX");
                foreach (var item in ReflectiveEnumerator.GetEnumerableOfType<BoundElement>())
                {
                    try
                    {
                        UniLog.Log("Binding "+ item.GetType().Name);
                        item.Bind();
                        UniLog.Log("Bound " + item.GetType().Name);
                    }
                    catch (Exception e) 
                    {
                        UniLog.Error("Failed to Bind " + item.GetType().Name + " Error: " + e.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                UniLog.Error("Failed to start FaoLogiX Error:"+e.ToString());                
            }
        }

        private static FakeConnector InstantiateConnector()
        {
            return new FakeConnector();
        }

        private class FakeConnector : IConnector
        {
            public IImplementable Owner { get; private set; }
            public void ApplyChanges() { }
            public void AssignOwner(IImplementable owner) => Owner = owner;
            public void Destroy(bool destroyingWorld) { }
            public void Initialize() { }
            public void RemoveOwner() => Owner = null;
        }
    }
}
