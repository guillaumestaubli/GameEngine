using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfmlControls.Utils
{
    internal static class SfmlUtils
    {
        public static Vertex[] GetVerticesLineStripRectangle(Vector2f position, Vector2f size, Color color)
        {
            Vertex[] vertices = new Vertex[5]
            {
                new Vertex()
                {
                    Position = position,
                    Color = color
                },
                new Vertex()
                {
                    Position = position + new Vector2f(0.0f, size.Y),
                    Color = color
                },
                new Vertex()
                {
                    Position = position + size,
                    Color = color
                },
                new Vertex()
                {
                    Position = position + new Vector2f(size.X, 0.0f),
                    Color = color
                },
                new Vertex()
                {
                    Position = position,
                    Color = color
                }
            };

            return vertices;
        }

        public static VertexArray GetLineStripRectangle(Vector2f position, Vector2f size, Color color)
        {
            Vertex[] vertices = GetVerticesLineStripRectangle(position, size, color);

            VertexArray vertexArray = new VertexArray(PrimitiveType.LineStrip);

            for (int i = 0; i < vertices.Length; ++i)
                vertexArray.Append(vertices[i]);

            return vertexArray;
        }

        public static Vertex[] GetVerticesTriangleStripRectangle(Vector2f position, Vector2f size, Color color)
        {
            Vertex[] vertices = new Vertex[4]
            {
                new Vertex()
                {
                    Position = position,
                    Color = color
                },
                new Vertex()
                {
                    Position = position + new Vector2f(0.0f, size.Y),
                    Color = color
                },
                new Vertex()
                {
                    Position = position + new Vector2f(size.X, 0.0f),
                    Color = color
                },
                new Vertex()
                {
                    Position = position + size,
                    Color = color
                }
            };

            return vertices;
        }

        public static VertexArray GetTriangleStripRectangle(Vector2f position, Vector2f size, Color color)
        {
            Vertex[] vertices = GetVerticesTriangleStripRectangle(position, size, color);

            VertexArray vertexArray = new VertexArray(PrimitiveType.TriangleStrip);

            for (int i = 0; i < vertices.Length; ++i)
                vertexArray.Append(vertices[i]);

            return vertexArray;
        }

        public static VertexArray GetTriangleStripRectangle(Vector2f position, Vector2f size, Vector2f texSize, Color color)
        {
            Vertex[] vertices = new Vertex[4]
            {
                new Vertex()
                {
                    Position = position,
                    Color = color,
                    TexCoords = new Vector2f(0.0f, 0.0f)
                },
                new Vertex()
                {
                    Position = position + new Vector2f(0.0f, size.Y),
                    Color = color,
                    TexCoords = new Vector2f(0.0f, texSize.Y)
                },
                new Vertex()
                {
                    Position = position + new Vector2f(size.X, 0.0f),
                    Color = color,
                    TexCoords = new Vector2f(texSize.X, 0.0f)
                },
                new Vertex()
                {
                    Position = position + size,
                    Color = color,
                    TexCoords = texSize
                }
            };

            VertexArray vertexArray = new VertexArray(PrimitiveType.TriangleStrip);

            for (int i = 0; i < vertices.Length; ++i)
                vertexArray.Append(vertices[i]);

            return vertexArray;
        }

        public static Color GetMedianColor(Image image)
        {
            int r = 0, g = 0, b = 0, a = 0;

            int counter = 0;

            for (uint x = 0; x < image.Size.X; ++x)
            {
                for (uint y = 0; y < image.Size.Y; ++y)
                {
                    r += image.GetPixel(x, y).R;
                    g += image.GetPixel(x, y).G;
                    b += image.GetPixel(x, y).B;
                    a += image.GetPixel(x, y).A;

                    ++counter;
                }
            }

            r /= counter;
            g /= counter;
            b /= counter;
            a /= counter;

            byte b_r = (byte)r, b_g = (byte)g, b_b = (byte)b, b_a = (byte)a;

            return new Color(b_r, b_g, b_b, b_a);
        } 
    }
}
