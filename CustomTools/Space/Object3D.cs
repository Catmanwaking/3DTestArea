using System.IO;

namespace Space
{
    public struct Object3D
    {
        public readonly Point3D[] vertices;
        public readonly byte[][] verticiesFaceIndex;

        public Object3D(string path)
        {
            try
            {
                using (BinaryReader binReader = new BinaryReader(File.OpenRead(path)))
                {
                    vertices = new Point3D[binReader.ReadInt32()];
                    verticiesFaceIndex = new byte[binReader.ReadInt32()][];
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        vertices[i].X = binReader.ReadDouble();
                        vertices[i].Y = binReader.ReadDouble();
                        vertices[i].Z = binReader.ReadDouble();
                    }
                    for (int i = 0; i < verticiesFaceIndex.Length; i++)
                    {
                        verticiesFaceIndex[i] = new byte[3]
                            { binReader.ReadByte(),
                                binReader.ReadByte(),
                                binReader.ReadByte()
                            };
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }

        }
    }
}
