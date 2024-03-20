using System.Collections.Generic;
using System.Text;

namespace BowieD.Unturned.IDTableGenerator.TableStyles
{
    public sealed class MarkdownStyle : ITableStyle
    {
        public string Create(IEnumerable<TableRecord> records, EInclude include)
        {
            int maxLType = "Type".Length, maxLName = "Name".Length, maxLGUID = "GUID".Length, maxLID = "ID".Length;

            int max(int a, int b)
            {
                if (a > b)
                    return a;
                return b;
            }

            foreach (var r in records)
            {
                maxLType = max(maxLType, r.Type.Length);
                maxLName = max(maxLName, r.Name.Length);
                maxLGUID = max(maxLGUID, r.GUID.ToString("N").Length);
                maxLID = max(maxLID, r.ID.ToString().Length);
            }

            maxLType += 2;
            maxLName += 2;
            maxLGUID += 2;
            maxLID += 2;

            string typeSplitter = new string('-', maxLType),
                nameSplitter = new string('-', maxLName),
                guidSplitter = new string('-', maxLGUID),
                idSplitter = new string('-', maxLID);

            string pad(string original, int required, char padWith = ' ')
            {
                int spaces = required - original.Length;
                int padLeft = spaces / 2 + original.Length;
                return original.PadLeft(padLeft, padWith).PadRight(required, padWith);
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("|");

            bool _hasAppended = false;

            void appendColumn(string content)
            {
                if (_hasAppended)
                    sb.Append("|");

                _hasAppended = true;

                sb.Append(content);
            }

            if (include.HasFlag(EInclude.Type))
                appendColumn(pad("Type", maxLType));
            if (include.HasFlag(EInclude.ID))
                appendColumn(pad("ID", maxLID));
            if (include.HasFlag(EInclude.Name))
                appendColumn(pad("Name", maxLName));
            if (include.HasFlag(EInclude.GUID))
                appendColumn(pad("GUID", maxLGUID));

            sb.AppendLine("|");
            _hasAppended = false;
            sb.Append("|");

            if (include.HasFlag(EInclude.Type))
                appendColumn(typeSplitter);
            if (include.HasFlag(EInclude.ID))
                appendColumn(idSplitter);
            if (include.HasFlag(EInclude.Name))
                appendColumn(nameSplitter);
            if (include.HasFlag(EInclude.GUID))
                appendColumn(guidSplitter);

            sb.AppendLine("|");
            _hasAppended = false;

            foreach (var item in records)
            {
                _hasAppended = false;

                sb.Append("|");

                if (include.HasFlag(EInclude.Type))
                    appendColumn(pad(item.Type, maxLType));
                if (include.HasFlag(EInclude.ID))
                    appendColumn(pad(item.ID.ToString(), maxLID));
                if (include.HasFlag(EInclude.Name))
                    appendColumn(pad(item.Name, maxLName));
                if (include.HasFlag(EInclude.GUID))
                    appendColumn(pad(item.GUID.ToString("N"), maxLGUID));

                sb.AppendLine("|");
            }

            return sb.ToString();
        }
    }
}
