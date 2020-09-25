using System.Collections.Generic;

namespace BowieD.Unturned.IDTableGenerator.Comparers
{
    public sealed class TableRecordTypeDescending : IComparer<TableRecord>
    {
        public int Compare(TableRecord x, TableRecord y)
        {
            return y.Type.CompareTo(x.Type);
        }
    }
}
