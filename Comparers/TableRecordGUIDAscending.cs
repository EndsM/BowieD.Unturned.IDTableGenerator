using System.Collections.Generic;

namespace BowieD.Unturned.IDTableGenerator.Comparers
{
    public sealed class TableRecordGUIDAscending : IComparer<TableRecord>
    {
        public int Compare(TableRecord x, TableRecord y)
        {
            return x.GUID.CompareTo(y.GUID);
        }
    }
}
