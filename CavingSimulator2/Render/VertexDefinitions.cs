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
    public struct VertexPCI
    {
        public readonly Vector3 Position;
        public readonly Color4 Color;
        public readonly float Texture;

        public static readonly VertexInfo VertexInfo = new VertexInfo(typeof(VertexPCI), new VertexAttribute[] {
            new VertexAttribute("Position",0,3,0),
            new VertexAttribute("Color",1,4,3 * sizeof(float)),
            new VertexAttribute("TextureId",2,1,7 * sizeof(float)),
        });

        public VertexPCI(Vector3 Position, Color4 Color, float Texture)
        {
            this.Position = Position;
            this.Color = Color;
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
    public struct VertexP
    {
        public readonly Vector3 Position;

        public static readonly VertexInfo VertexInfo = new VertexInfo(typeof(VertexP), new VertexAttribute[] {
            new VertexAttribute("Position",0,3,0)
        });
        public VertexP(Vector3 Position)
        {
            this.Position = Position;
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
