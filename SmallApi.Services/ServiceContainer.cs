using System;
using SmallApi.Services.Configuration;
using StructureMap;

namespace SmallApi.Services
{
    public class ServiceContainer
    {
        private static IContainer current;
        private static readonly object CurrentLock = new object();

        public static IContainer Current => CurrentContainerFactory();
        
        public static Func<IContainer> CurrentContainerFactory { get; set; } = () =>
        {
            if (current != null) return current;

            lock (CurrentLock)
            {
                current = new Container(c => { c.AddRegistry<CoreRegistry>(); });
            }
            return current;
        };

        public static void SetCurrentFactory(Func<IContainer> container)
        {
            if (container == null)
                throw new ArgumentNullException();
            CurrentContainerFactory = container;
        }
    }
}