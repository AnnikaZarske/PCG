using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscape
{
    public enum LandscapeKind
    {
        Water,
        Grass,
        Mountain,
        Forest,
        Beach,
        Empty
    }
    
    [Serializable]
    public class LandscapeType
    {
        public string name;
        public LandscapeKind kind;
        public List<MapNodeTypeColor> typeColors = new List<MapNodeTypeColor>();
    }

    public class LandscapeList
    {
        public List<LandscapeType> landscapeTypeList;
        public MapGeneratorPreview mapGeneratorPreview;

        public LandscapeList(MapGeneratorPreview MGP)
        {
            landscapeTypeList = new List<LandscapeType>();
            this.mapGeneratorPreview = MGP;
            CreateLandscapeList();
        }
        
        void CreateLandscapeList()
        {
            LandscapeType grassType = new LandscapeType();
            grassType.typeColors = new List<MapNodeTypeColor>();
            grassType.typeColors.Add(mapGeneratorPreview.colours.Find(x => x.type == MapGraph.MapNodeType.Grass));
            grassType.name = "Grass";
            grassType.kind = LandscapeKind.Grass;
            landscapeTypeList.Add(grassType);
        
            LandscapeType forestType = new LandscapeType();
            forestType.typeColors = new List<MapNodeTypeColor>();
            forestType.typeColors.Add(mapGeneratorPreview.colours.Find(x => x.type == MapGraph.MapNodeType.Forest));
            forestType.name = "Forest";
            forestType.kind = LandscapeKind.Forest;
            landscapeTypeList.Add(forestType);
        
            LandscapeType waterType = new LandscapeType();
            waterType.typeColors = new List<MapNodeTypeColor>();
            waterType.typeColors.Add(mapGeneratorPreview.colours.Find(x => x.type == MapGraph.MapNodeType.FreshWater));
            waterType.typeColors.Add(mapGeneratorPreview.colours.Find(x => x.type == MapGraph.MapNodeType.SaltWater));
            waterType.name = "Water";
            waterType.kind = LandscapeKind.Water;
            landscapeTypeList.Add(waterType);
        
            LandscapeType mountainType = new LandscapeType();
            mountainType.typeColors = new List<MapNodeTypeColor>();
            mountainType.typeColors.Add(mapGeneratorPreview.colours.Find(x => x.type == MapGraph.MapNodeType.Snow));
            mountainType.typeColors.Add(mapGeneratorPreview.colours.Find(x => x.type == MapGraph.MapNodeType.Mountain));
            mountainType.name = "Mountain";
            mountainType.kind = LandscapeKind.Mountain;
            landscapeTypeList.Add(mountainType);
        
            LandscapeType beachType = new LandscapeType();
            beachType.typeColors = new List<MapNodeTypeColor>();
            beachType.typeColors.Add(mapGeneratorPreview.colours.Find(x => x.type == MapGraph.MapNodeType.Beach));
            beachType.name = "Beach";
            beachType.kind = LandscapeKind.Beach;
            landscapeTypeList.Add(beachType);
        }

        public LandscapeKind GetLandscape(Color testColor)
        {
            foreach (LandscapeType landscape in landscapeTypeList) {
                foreach (MapNodeTypeColor mapNodeTypeColor in landscape.typeColors)
                {
                    if (CompareColors(testColor * 255, mapNodeTypeColor.color * 255))
                    {
                        return landscape.kind;
                    }
                }
            }
            return LandscapeKind.Empty;
        }
        
        public bool CompareColors(Color testColor, Color compareAgainstColor)
        {
            Color tc = testColor;
            Color cac = compareAgainstColor;
            float delta = 5; // 5 = the value range to compare against
            bool isSameColor = false;
            if (tc.r > cac.r - delta && tc.r < cac.r + delta) {
                if (tc.g > cac.g - delta && tc.g < cac.g + delta) {
                    if (tc.b > cac.b - delta && tc.b < cac.b + delta) {
                        isSameColor = true;
                    }
                }
            }
            return isSameColor;
        }
    }
}
