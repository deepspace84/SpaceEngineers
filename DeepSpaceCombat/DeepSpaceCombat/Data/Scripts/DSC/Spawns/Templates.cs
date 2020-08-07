using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace DSC
{
    public class DSC_SpawnTemplates
    {
        
        public DSC_SpawnTemplates() { }

        public Dictionary<string, DSC_SpawnTemplate> Spawns { get; } = new Dictionary<string, DSC_SpawnTemplate>();


        public Dictionary<string, Dictionary<int, DSC_SpawnRoute>> Routes { get; } = new Dictionary<string, Dictionary<int, DSC_SpawnRoute>>()
        {
            {"Testroute", new Dictionary<int, DSC_SpawnRoute>()
            {
                {0, new DSC_SpawnRoute("Start", false, new Vector3D(36127.86 , -1629.97 , 49527.7), 100, false, 0)},
                {1, new DSC_SpawnRoute("End", false, new Vector3D(34967.03 , -1452.38 , 50364.61), 100, false, 0)}
                
            }}

        };

        public void Load()
        {
            Spawns.Add("TestSpawn", new DSC_SpawnTemplate("Testspawn", true, Routes["Testroute"]));
        }
    }


    public class DSC_SpawnTemplate
    {
        public readonly string Name;
        public readonly bool Friendly;
        
        public readonly Dictionary<int, DSC_SpawnRoute> Routes;

        public DSC_SpawnTemplate() { }

        public DSC_SpawnTemplate(string name, bool friendly, Dictionary<int, DSC_SpawnRoute> routes)
        {
            Name = name;
            Friendly = friendly;
            Routes = routes;
        }
    }


    public class DSC_SpawnRoute
    {
        public readonly string Name;
        public readonly bool CollisionAvoid;
        public readonly Vector3D Target;
        public readonly int Speed;
        public readonly bool Dock;
        public readonly int Wait;
        
        public DSC_SpawnRoute() { }

        public DSC_SpawnRoute(string name, bool collisionAvoid, Vector3D target, int speed, bool dock, int wait)
        {
            Name = name;
            CollisionAvoid = collisionAvoid;
            Target = target;
            Speed = speed;
            Dock = dock;
            Wait = wait;
        }
    }
}
