using System.Collections.Generic;

namespace BowieD.Unturned.IDTableGenerator.Comparers
{
    public sealed class TableRecordNameAscending : IComparer<TableRecord>
    {
        public int Compare(TableRecord x, TableRecord y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
