using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render
{


    // one vertex object
    public struct VertexPCTN
    {
        public readonly Vector3 Position;
        public readonly Color4 Color;
        public readonly Vector2 Texture;
        public readonly Vector3 Normal;

        public static readonly VertexInfo VertexInfo = new VertexInfo(typeof(VertexPCTN), new VertexAttribute[] {
            new VertexAttribute("Position",0,3,0),
            new VertexAttribute("Color",1,4,3 * sizeof(float)),
            new VertexAttribute("Texture",2,2, 7 * sizeof(float)),
            new VertexAttribute("Texture",3,3, 9 * sizeof(float))
        });

        public VertexPCTN(Vector3 Position, Color4 Color, Vector2 Texture, Vector3 Normal)
        {
            this.Position = Position;
            this.Color = Color;
            this.Texture = Texture;
            this.Normal = Normal;
        }
    }
    public struct VertexPCT
    {
        public readonly Vector3 Position;
        public readonly Color4 Color;
        public readonly float Texture;

        public static readonly VertexInfo VertexInfo = new VertexInfo(typeof(VertexPCT), new VertexAttribute[] {
            new VertexAttribute("Position",0,3,0),
            new VertexAttribute("Color",1,4,3 * sizeof(float)),
            new VertexAttribute("TextureId",2,1,7 * sizeof(float)),
        });

        public VertexPCT(Vector3 Position, Color4 Color, float Texture)
        {
            this.Position = Position;
            this.Color = Color;
            this.Texture = Texture;
        }
    }
    // all block face instance
    public struct VertexPOTTiN
    {
        public readonly Vector3 Position;
        public readonly Vector3 Offset;
        public readonly Vector2 Texture;
        public readonly float TextureId;
        public readonly Vector3 Normal;

        public static readonly VertexInfo VertexInfo = new VertexInfo(typeof(VertexPOTTiN), new VertexAttribute[] {
            new VertexAttribute("Position" ,0,3,0),
            new VertexAttribute("Offset"   ,1,3,3 * sizeof(float)),
            new VertexAttribute("Texture"  ,2,2,6 * sizeof(float)),
            new VertexAttribute("TextureId",3,1,8 * sizeof(float)),
            new VertexAttribute("Normal"   ,4,3,9 * sizeof(float)),
        });
        public VertexPOTTiN(Vector3 Position, Vector3 Offset, Vector2 Texture, float TextureId, Vector3 Normal)
        {
            this.Position = Position;
            this.Offset = Offset;
            this.Texture = Texture;
            this.TextureId = TextureId;
            this.Normal = Normal;
        }
    }

    // one block instance
    public struct VertexPT
    {
        public readonly Vector3 Position;
        public readonly float Texture;

        public static readonly VertexInfo VertexInfo = new VertexInfo(typeof(VertexPT), new VertexAttribute[] {
            new VertexAttribute("Position",0,3,0),
            new VertexAttribute("TextureId",1,1,3 * sizeof(float)),
        });

        public VertexPT(Vector3 Position, float Texture)
        {
            this.Position = Position;
            this.Texture = Texture;
        }
    }
    // one vertex object
    public struct VertexPCTOTI
    {
        public readonly Vector3 Position;
        public readonly Color4 Color;
        public readonly Vector2 Texture;
        public readonly Vector3 Offset;
        public readonly float TextureId;
        public readonly float VertexId;

        public static readonly VertexInfo VertexInfo = new VertexInfo(typeof(VertexPCTOTI), new VertexAttribute[] {
            new VertexAttribute("Position",0,3,0),
            new VertexAttribute("Color",1,4,3 * sizeof(float)),
            new VertexAttribute("Texture",2,2, 7 * sizeof(float)),
            new VertexAttribute("Offset",3,3, 9 * sizeof(float)),
            new VertexAttribute("TextureId",4,1,12 * sizeof(float)),
            new VertexAttribute("VertexId",5,1,13 * sizeof(float)),
        });
        public VertexPCTOTI(Vector3 Position, Color4 Color, Vector2 Texture, Vector3 Offset, float TextureId, float VertexId)
        {
            this.TextureId = TextureId;
            this.Position = Position;
            this.Color = Color;
            this.Texture = Texture;
            this.Offset = Offset;
            this.VertexId = VertexId;
        }
    }
    public struct VertexPCTp
    {
        public readonly Vector3 Position;
        public readonly Color4 Color;
        public readonly Vector2 Texture;
        

        public static readonly VertexInfo VertexInfo = new VertexInfo(typeof(VertexPCTp), new VertexAttribute[] {
            new VertexAttribute("Position",0,3,0),
            new VertexAttribute("Color",1,4,3 * sizeof(float)),
            new VertexAttribute("Texture",2,2,7 * sizeof(float))
        });
        public VertexPCTp(Vector3 Position, Color4 Color, Vector2 Texture)
        {
            this.Position = Position;
            this.Color = Color;
            this.Texture = Texture;
        }
    }
    



    // all vertex atribute information
    public readonly struct VertexAttribute
    {
        public readonly string Name;
        public readonly int Index;
        public readonly int ComponentCount;
        public readonly int Offset;

        public VertexAttribute(string name, int index, int componentCount, int offset)
        {
            Name = name;
            Index = index;
            ComponentCount = componentCount;
            Offset = offset;
        }
    }

    // all vertex information about atributes
    public sealed class VertexInfo
    {
        public readonly Type Type;
        public readonly int SizeInBytes;
        public readonly VertexAttribute[] VertexAttributes;

        public VertexInfo(Type type, params VertexAttribute[] vertexAttributes)
        {
            Type = type;
            VertexAttributes = vertexAttributes;
            SizeInBytes = 0;
            for (int i = 0; i < vertexAttributes.Length; i++)
            {
                SizeInBytes += vertexAttributes[i].ComponentCount * sizeof(float);
            }
        }
    }
}
