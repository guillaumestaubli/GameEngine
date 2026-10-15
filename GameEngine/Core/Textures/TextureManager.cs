
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Textures
{
    public static class TextureManager
    {
        private static TextureLibrary _libraryEntities;
        private static TextureLibrary _libraryGround;
        private static TextureLibrary _libraryProps;

        private static TextureDetails _texture_NotFound;
        private static TextureDetails _texture_DefaultGround;

        #region GET_SET

        public static TextureLibrary LibraryEntities { get => _libraryEntities; }
        public static TextureLibrary LibraryGround { get => _libraryGround; }
        public static TextureLibrary LibraryProps { get => _libraryProps; }

        public static TextureDetails Texture_NotFound { get => _texture_NotFound; }
        public static TextureDetails Texture_DefaultGround { get => _texture_DefaultGround; }

        #endregion

        static TextureManager()
        {
            Console.WriteLine($"> TextureManager - Loading textures ...");

            _libraryEntities = new TextureLibrary(Config.RESSOURCES_ENTITIES_PATH);

            _libraryGround = new TextureLibrary(Config.RESSOURCES_GROUND_PATH);

            _libraryProps = new TextureLibrary(Config.RESSOURCES_PROP_PATH);

            _texture_NotFound = new TextureDetails(Config.DEFAULT_TEXTURE_PATH);
            _texture_DefaultGround = new TextureDetails(Config.DEFAULT_TEXTURE_GROUND_PATH);

            Console.WriteLine($"> TextureManager - {_libraryEntities.Count + _libraryGround.Count + _libraryProps.Count + 2} textures loaded.");
        }

        public static List<Vector2i> GenerateSpriteAbsoluteBounds(TextureDetails texture)
        {
            Image image = texture.Image;

            return GenerateSpriteAbsoluteBounds(image, texture.Name);
        }


        public static List<Vector2i> GenerateSpriteAbsoluteBounds(Image image, string consoleInfos = "")
        {
            Console.WriteLine($"> {nameof(TextureManager)} - {nameof(GenerateSpriteAbsoluteBounds)} - {consoleInfos} ...");

            List<Pixel> pixelsSpriteBorder = new List<Pixel>();

            List<Pixel> pixels = new List<Pixel>();

            Pixel[,] mappedPixels = new Pixel[image.Size.X, image.Size.Y];

            for (uint x = 0; x < image.Size.X; ++x)
            {
                for (uint y = 0; y < image.Size.Y; ++y)
                {
                    Pixel pixel = new Pixel(image.GetPixel(x, y), new Vector2i((int)x, (int)y));

                    pixels.Add(pixel);
                    mappedPixels[x, y] = pixel;
                }
            }

            List<Pixel> pixelsBorder = new List<Pixel>();
            //List<Pixel> pixelNonTransparent = pixels.Where(x => x.RGBA_Sum > 0).ToList();
            List<Pixel> pixelNonTransparent = pixels.Where(x => x.Color.A == 255).ToList();

            Vector2i leftIndex = new Vector2i(int.MinValue, int.MinValue);
            Vector2i rightIndex = new Vector2i(int.MinValue, int.MinValue);
            Vector2i topIndex = new Vector2i(int.MinValue, int.MinValue);
            Vector2i bottomIndex = new Vector2i(int.MinValue, int.MinValue);

            Pixel? leftPixel = null;
            Pixel? rightPixel = null;
            Pixel? topPixel = null;
            Pixel? bottomPixel = null;

            foreach (Pixel pixel in pixelNonTransparent)
            {
                leftIndex.X = pixel.Index.X - 1;
                leftIndex.Y = pixel.Index.Y;

                if (leftIndex.X < 0)
                    leftPixel = null;
                else
                    leftPixel = mappedPixels[leftIndex.X, leftIndex.Y];

                rightIndex.X = pixel.Index.X + 1;
                rightIndex.Y = pixel.Index.Y;

                if (rightIndex.X >= image.Size.X)
                    rightPixel = null;
                else
                    rightPixel = mappedPixels[rightIndex.X, rightIndex.Y];

                topIndex.X = pixel.Index.X;
                topIndex.Y = pixel.Index.Y - 1;

                if (topIndex.Y < 0)
                    topPixel = null;
                else
                    topPixel = mappedPixels[topIndex.X, topIndex.Y];

                bottomIndex.X = pixel.Index.X;
                bottomIndex.Y = pixel.Index.Y + 1;

                if (bottomIndex.Y >= image.Size.Y)
                    bottomPixel = null;
                else
                    bottomPixel = mappedPixels[bottomIndex.X, bottomIndex.Y];


                //if (leftPixel == null || leftPixel.RGBA_Sum == 0)
                if (leftPixel == null || leftPixel.Color.A < 255)
                {
                    pixelsBorder.Add(pixel);
                }

                //if (rightPixel == null || rightPixel.RGBA_Sum == 0)
                if (rightPixel == null || rightPixel.Color.A < 255)
                {
                    pixelsBorder.Add(pixel);
                }

                //if (topPixel == null || topPixel.RGBA_Sum == 0)
                if (topPixel == null || topPixel.Color.A < 255)
                {
                    pixelsBorder.Add(pixel);
                }

                //if (bottomPixel == null || bottomPixel.RGBA_Sum == 0)
                if (bottomPixel == null || bottomPixel.Color.A < 255)
                {
                    pixelsBorder.Add(pixel);
                }
            }

            pixelsBorder = pixelsBorder.Distinct().ToList();

            List<Vector2i> pointsBorder = SortPixelsSpriteBorder(pixelsBorder);

            Console.WriteLine($"> {nameof(TextureManager)} - {nameof(GenerateSpriteAbsoluteBounds)} - {pointsBorder.Count} points created");

            return pointsBorder;
        }

        private static List<Vector2i> SortPixelsSpriteBorder(List<Pixel> pixels)
        {
            List<Pixel> sortedPixels = new List<Pixel>();

            pixels = pixels.OrderBy(x => x.Index.X).ThenBy(x => x.Index.Y).ToList();

            sortedPixels.Add(pixels.First());
            pixels.RemoveAt(0);

            bool stopLoop = false;
            int indexToRemove = int.MinValue;
            double min_length = double.MaxValue;
            double current_length = 0.0;
            Pixel? closest = null;
            Pixel last = sortedPixels.Last();
            Pixel current;
            Vector2f pos_current;
            Vector2f pos_last;
            Vector2f delta;

            while (pixels.Count > 0 && stopLoop == false)
            {
                indexToRemove = int.MinValue;
                min_length = float.MaxValue;
                closest = null;

                for (int i = 0; i < pixels.Count; ++i)
                {
                    current = pixels[i];

                    pos_current = (Vector2f)current.Index;
                    pos_last = (Vector2f)last.Index;

                    delta = pos_current - pos_last;

                    current_length = Math.Sqrt(Math.Pow(delta.X, 2.0) + Math.Pow(delta.Y, 2.0));

                    if (current_length < min_length)
                    {
                        indexToRemove = i;
                        closest = current;
                        min_length = current_length;
                    }
                }

                if (closest != null)
                {
                    if (min_length < 1.5)
                    {
                        sortedPixels.Add(closest);
                        pixels.RemoveAt(indexToRemove);
                        last = closest;
                    }
                    else
                    {
                        // means the outer border is completed
                        stopLoop = true;
                    }
                }
            }

            List<Vector2i> sortedPoints = new List<Vector2i>();

            for (int i = 0; i < sortedPixels.Count; ++i)
            {
                sortedPoints.Add(sortedPixels[i].Index);
            }

            return sortedPoints;
        }
    }

    public class Pixel
    {
        public Color Color { get; set; } = new Color(0, 0, 0, 0);
        public Vector2i Index { get; set; } = new Vector2i(int.MinValue, int.MinValue);
        public int RGBA_Sum { get; } = int.MinValue;

        public Pixel(Color color, Vector2i index)
        {
            Color = color;
            Index = index;
            RGBA_Sum = color.R + color.G + color.B + color.A;
        }
    }
}
