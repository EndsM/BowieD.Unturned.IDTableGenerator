using System.Collections.Generic;

namespace BowieD.Unturned.IDTableGenerator.Comparers
{
    public sealed class TableRecordGUIDDescending : IComparer<TableRecord>
    {
        public int Compare(TableRecord x, TableRecord y)
        {
            return y.GUID.CompareTo(x.GUID);
        }
    }
}
