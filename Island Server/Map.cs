using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Seed { get; set; }

        public int Octave { get; set; }
        public double Persistance { get; set; }

        public Tile[,] Tiles { get; set; }
        public int[][] Items { get; set; }

        public Map(int width, int height, int seed)
        {
            Width = width;
            Height = height;
            Seed = Seed == 0 ? new Random().Next(1, 10000) : seed;

            Octave = 8;
            Persistance = 0.6;
        }

        public void Generate()
        {
            Tiles = new Tile[Width, Height];

            double[,] BaseNoise = GenerateWhiteNoise();
            double[,] PerlinNoise = GeneratePerlinNoise(BaseNoise);

            double MaxWidth = Width * 0.45 - 10; //calculates the max width of the island so that it doesn't touch the edges

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    double delta = DistanceSquared(x, y, Width, Height) / MaxWidth;
                    double gradient = delta * delta;
                    double block = PerlinNoise[x, y];

                    if (block >= gradient)
                    {
                        if (block >= 0.75)
                            Tiles[x, y] = new Tile { Type = TileType.Tree, Colour = "#006400" };
                        else if (block >= 0.6 && block <= 0.7)
                            Tiles[x, y] = new Tile { Type = TileType.Foliage, Colour = "#5E8A00" };
                        else if (block >= 0.35 && block <= 0.45)
                            Tiles[x, y] = new Tile { Type = TileType.Dirt, Colour = "#705B47" };
                        else if (block >= 0.16 && block <= 0.35)
                            Tiles[x, y] = new Tile { Type = TileType.Rock, Colour = "#808080" };
                        else
                            Tiles[x, y] = new Tile { Type = TileType.Grass, Colour = "#556B2F" };
                    }
                    else if (block >= gradient - 0.15)
                        Tiles[x, y] = new Tile { Type = TileType.Sand, Colour = "#BDB76B" };
                    else if (block >= gradient - 0.22)
                        Tiles[x, y] = new Tile { Type = TileType.Spawn, Colour = "#2957AD" };
                    else if (block >= gradient - 0.30)
                        Tiles[x, y] = new Tile { Type = TileType.ShallowWater, Colour = "#2957AD" };
                    else if (block >= gradient - 0.45)
                        Tiles[x, y] = new Tile { Type = TileType.MediumWater, Colour = "#214485" };
                    else
                        Tiles[x, y] = new Tile { Type = TileType.DeepWater, Colour = "#142952" };
                }
            }
        }

        private double[,] GenerateWhiteNoise()
        {
            double[,] Noise = new double[Width, Height];
            Random Rand = new Random(Seed);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Noise[x, y] = Math.Round(Rand.NextDouble(), 0);

            return Noise;
        }
        private double[,] GeneratePerlinNoise(double[,] noise)
        {
            double[,] PerlinNoise = new double[Width, Height];

            double Amplitude = 1;
            double TotalAmplitude = 0;

            for (int count = Octave - 1; count >= 0; count--)
            {
                double SamplePeriod = (int)(Math.Pow(2, count));
                double SampleFrequency = 1 / SamplePeriod;

                Amplitude *= Persistance;
                TotalAmplitude += Amplitude;

                for (int y = 0; y < Height; y++)
                {
                    int sample_y0 = (int)((int)(y / SamplePeriod) * SamplePeriod);
                    int sample_y1 = (int)(sample_y0 + SamplePeriod) % Height;
                    double vertical_blend = (y - sample_y0) * SampleFrequency;

                    for (int x = 0; x < Width; x++)
                    {
                        int sample_x0 = (int)((int)(x / SamplePeriod) * SamplePeriod);
                        int sample_x1 = (int)(sample_x0 + SamplePeriod) % Width;
                        double horizontal_blend = (x - sample_x0) * SampleFrequency;

                        double top = Interpolate(noise[sample_x0, sample_y0], noise[sample_x0, sample_y1], vertical_blend);
                        double bottom = Interpolate(noise[sample_x1, sample_y0], noise[sample_x1, sample_y1], vertical_blend);

                        PerlinNoise[x, y] += Interpolate(top, bottom, horizontal_blend) * Amplitude;
                    }
                }
            }

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    PerlinNoise[x, y] /= TotalAmplitude;

            return PerlinNoise;
        }

        private double DistanceSquared(int x, int y, int width, int height)
        {
            double dx = Math.Abs(x - width * 0.5);
            double dy = Math.Abs(y - height * 0.5);

            return (Math.Sqrt(dx * dx + dy * dy));
        }
        private double Interpolate(double x0, double x1, double alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }
    }
}
