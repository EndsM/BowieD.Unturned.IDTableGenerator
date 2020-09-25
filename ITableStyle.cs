using System;
using System.Collections.Generic;
using System.Text;

namespace BowieD.Unturned.IDTableGenerator
{
    public enum EInclude
    {
        Empty = 0,
        GUID = 1 << 0,
        ID = 1 << 1,
        Name = 1 << 2,
        Type = 1 << 3,
        All = GUID | ID | Name | Type
    }
    public interface ITableStyle
    {
        string Create(IEnumerable<TableRecord> records, EInclude include);
    }
    public struct TableRecord
    {
        public TableRecord(Guid guid, ushort id, string name, string type)
        {
            GUID = guid;
            ID = id;
            Name = name;
            Type = type;
        }

        public Guid GUID { get; }
        public ushort ID { get; }
        public string Name { get; }
        public string Type { get; }
    }
}
