using System.Collections.Generic;

namespace BowieD.Unturned.IDTableGenerator.Comparers
{
    public sealed class TableRecordNameDescending : IComparer<TableRecord>
    {
        public int Compare(TableRecord x, TableRecord y)
        {
            return y.Name.CompareTo(x.Name);
        }
    }
}
