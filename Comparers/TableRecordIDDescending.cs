using System.Collections.Generic;

namespace BowieD.Unturned.IDTableGenerator.Comparers
{
    public sealed class TableRecordIDDescending : IComparer<TableRecord>
    {
        public int Compare(TableRecord x, TableRecord y)
        {
            return y.ID.CompareTo(x.ID);
        }
    }
}
