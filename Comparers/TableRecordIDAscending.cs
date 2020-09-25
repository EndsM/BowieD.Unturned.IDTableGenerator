using System.Collections.Generic;

namespace BowieD.Unturned.IDTableGenerator.Comparers
{
    public sealed class TableRecordIDAscending : IComparer<TableRecord>
    {
        public int Compare(TableRecord x, TableRecord y)
        {
            return x.ID.CompareTo(y.ID);
        }
    }
}
