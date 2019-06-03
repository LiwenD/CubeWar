using System;
using System.Collections.Generic;
using System.Linq;

namespace YummyGame.Framework
{
    public class UILayer
    {
        public string layer;
        public int layerIndex;
    }

    public static class UIManagerConfig
    {
        private static Dictionary<string, UILayer> layers = new Dictionary<string, UILayer>();

        static UIManagerConfig()
        {
            AddLayer("Background",-1);
            AddLayer("Windows", 1);
            AddLayer("Pop",100);
        }

        public static void AddLayer(string layer,int layerIndex)
        {
            if (layers.ContainsKey(layer)) return;
            layers.Add(layer, new UILayer() { layer = layer, layerIndex = layerIndex });
        }

        public static List<UILayer> GetLayers()
        {
            var allLayers = layers.Values.ToList();
            allLayers.Sort((a, b) => b.layerIndex - a.layerIndex);
            return allLayers;
        }
    }
}
