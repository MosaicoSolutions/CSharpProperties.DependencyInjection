using CSharpProperties.DependencyInjection.Annotations;

namespace CSharpProperties.DependencyInjection.Test.ViewModels
{
    [PropertiesFile(Path = @".\several-properties.txt")]
    public class SeveralPropertiesViewModel
    {
        public string String { get; set; }

        public bool Bool { get; set; }

        public byte Byte { get; set; }

        public sbyte SByte { get; set; }

        public char Char { get; set; }

        public decimal Decimal { get; set; }

        public double Double { get; set; }

        public float Float { get; set; }

        public int Integer { get; set; }

        public uint UnsignedInteger { get; set; }

        public long Long { get; set; }

        public ulong UnsignedLong { get; set; }

        public short Short { get; set; }

        public ushort UShort { get; set; }
    }
}