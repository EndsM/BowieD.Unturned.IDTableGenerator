using System.Collections.Generic;

namespace BowieD.Unturned.IDTableGenerator.Comparers
{
    public sealed class TableRecordTypeAscending : IComparer<TableRecord>
    {
        public int Compare(TableRecord x, TableRecord y)
        {
            return x.Type.CompareTo(y.Type);
        }
    }
}
