using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandServer
{
    enum Tile
    {
        DeepWater,
        MediumWater,
        ShallowWater,
        Spawn,
        Sand,
        Grass,
        Foliage,
        Tree,
        Dirt,
        Rock
    }

    public class Map
    {
        public int Seed { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Octave { get; set; }
        public double Persistance { get; set; }
        public int[][] Tiles { get; set; }
        public int[][] Items { get; set; }

        public Map(int width, int height, int seed)
        {
            Width = width;
            Height = height;
            Octave = 8;
            Persistance = 0.6;
            Seed = Seed == 0 ? new Random().Next(1, 10000) : seed;
        }

        private double DistanceSquared(int x, int y, int width, int height)
        {
            double dx = Math.Abs(x - width * 0.5);
            double dy = Math.Abs(y - height * 0.5);

            return (Math.Sqrt(dx * dx + dy * dy));
        }

        private double[][] GeneratePerlinNoise(double[][] baseNoise, int octave, double persistance)
        {
            double[][] perlinNoise = new double[Height][];
            double amplitude = 1;
            double totalAmplitude = 0;

            for (int octaveCount = octave - 1; octaveCount >= 0; octaveCount--)
            {
                double samplePeriod = (int)(Math.Pow(2, octaveCount));
                double sampleFrequency = 1 / samplePeriod;

                amplitude *= persistance;
                totalAmplitude += amplitude;

                for (int y = 0; y < Height; y++)
                {
                    perlinNoise[y] = perlinNoise[y] == null ? new double[Width] : perlinNoise[y];
                    int sample_y0 = (int)((int)(y / samplePeriod) * samplePeriod);
                    int sample_y1 = (int)(sample_y0 + samplePeriod) % Height;
                    double vertical_blend = (y - sample_y0) * sampleFrequency;

                    for (int x = 0; x < Width; x++)
                    {
                        int sample_x0 = (int)((int)(x / samplePeriod) * samplePeriod);
                        int sample_x1 = (int)(sample_x0 + samplePeriod) % Width;
                        double horizontal_blend = (x - sample_x0) * sampleFrequency;

                        double top = Interpolate(baseNoise[sample_y0][sample_x0], baseNoise[sample_y1][sample_x0], vertical_blend);
                        double bottom = Interpolate(baseNoise[sample_y0][sample_x1], baseNoise[sample_y1][sample_x1], vertical_blend);

                        perlinNoise[y][x] += Interpolate(top, bottom, horizontal_blend) * amplitude;
                    }
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    perlinNoise[y][x] /= totalAmplitude;
                }
            }

            return perlinNoise;
        }

        private double Interpolate(double x0, double x1, double alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }

        private double[][] GenerateWhiteNoise(int width, int height, int seed)
        {
            double[][] noise = new double[height][];
            Random rand = new Random(seed);
            for (int y = 0; y < height; y++)
            {
                noise[y] = new double[width];
                for (int x = 0; x < width; x++)
                {
                    noise[y][x] = Math.Round(rand.NextDouble(), 0);
                }
            }
            return noise;
        }

        public void Generate()
        {
            Tiles = new int[Height][];
            double MaxWidth = Width * 0.45 - 10; //calculates the max width of the island so that it doesn't touch the edges

            double[][] baseNoise = GenerateWhiteNoise(Width, Height, Seed);
            double[][] perlinNoise = GeneratePerlinNoise(baseNoise, Octave, Persistance);

            for (int y = 0; y < Height; y++)
            {
                Tiles[y] = new int[Width];
                for (int x = 0; x < Width; x++)
                {
                    double delta = DistanceSquared(x, y, Width, Height) / MaxWidth;
                    double gradient = delta * delta;
                    double block = perlinNoise[y][x];

                    if (block >= gradient) //Land
                    {
                        if (block >= 0.75)
                        {
                            Tiles[y][x] = (int)Tile.Tree;
                        }
                        else if (block >= 0.6 && block <= 0.7)
                        {
                            Tiles[y][x] = (int)Tile.Foliage;
                        }
                        else if (block >= 0.35 && block <= 0.45)
                        {
                            Tiles[y][x] = (int)Tile.Dirt;
                        }
                        else if (block >= 0.16 && block <= 0.35)
                        {
                            Tiles[y][x] = (int)Tile.Rock;
                        }
                        else
                        {
                            Tiles[y][x] = (int)Tile.Grass;
                        }
                    }
                    else if (block >= gradient - 0.15)
                    {
                        Tiles[y][x] = (int)Tile.Sand;
                    }
                    else if (block >= gradient - 0.22)
                    {
                        Tiles[y][x] = (int)Tile.Spawn;
                    }
                    else if (block >= gradient - 0.30)
                    {
                        Tiles[y][x] = (int)Tile.ShallowWater;
                    }
                    else if (block >= gradient - 0.45)
                    {
                        Tiles[y][x] = (int)Tile.MediumWater;
                    }
                    else
                    {
                        Tiles[y][x] = (int)Tile.DeepWater;
                    }
                }
            }
        }
    }
}
