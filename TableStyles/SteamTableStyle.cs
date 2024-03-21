using System.Collections.Generic;
using System.Text;

namespace BowieD.Unturned.IDTableGenerator.TableStyles
{
    public sealed class SteamTableStyle : ITableStyle
    {
        public string Create(IEnumerable<TableRecord> records, EInclude include)
        {
            StringBuilder sb = new StringBuilder();

            void appendth(object content)
            {
                sb.AppendLine($"[th]{content}[/th]");
            }

            sb.AppendLine("[table]");

            sb.AppendLine("[tr]");

            if (include.HasFlag(EInclude.Type))
                appendth("Type");
            if (include.HasFlag(EInclude.ID))
                appendth("ID");
            if (include.HasFlag(EInclude.Name))
                appendth("Name");
            if (include.HasFlag(EInclude.GUID))
                appendth("GUID");

            sb.AppendLine("[/tr]");

            foreach (var item in records)
            {
                sb.AppendLine("[tr]");

                if (include.HasFlag(EInclude.Type))
                    appendth(item.Type);
                if (include.HasFlag(EInclude.ID))
                    appendth(item.ID);
                if (include.HasFlag(EInclude.Name))
                    appendth(item.Name);
                if (include.HasFlag(EInclude.GUID))
                    appendth(item.GUID.ToString("N"));

                sb.AppendLine("[/tr]");
            }

            sb.AppendLine("[/table]");

            return sb.ToString();
        }
    }
}
