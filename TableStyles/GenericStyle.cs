using System.Collections.Generic;
using System.Text;

namespace BowieD.Unturned.IDTableGenerator.TableStyles
{
    public sealed class GenericStyle : ITableStyle
    {
        public string Create(IEnumerable<TableRecord> records, EInclude include)
        {
            StringBuilder sb = new StringBuilder();

            List<string> ds = new List<string>();

            if (include.HasFlag(EInclude.Type))
                ds.Add("Type");
            if (include.HasFlag(EInclude.ID))
                ds.Add("ID");
            if (include.HasFlag(EInclude.Name))
                ds.Add("Name");
            if (include.HasFlag(EInclude.GUID))
                ds.Add("GUID");

            sb.AppendLine(string.Join("\t", ds));

            foreach (var item in records)
            {
                ds.Clear();

                if (include.HasFlag(EInclude.Type))
                    ds.Add(item.Type);
                if (include.HasFlag(EInclude.ID))
                    ds.Add(item.ID.ToString());
                if (include.HasFlag(EInclude.Name))
                    ds.Add(item.Name);
                if (include.HasFlag(EInclude.GUID))
                    ds.Add(item.GUID.ToString("N"));

                sb.AppendLine(string.Join("\t", ds));
            }

            return sb.ToString();
        }
    }
}
