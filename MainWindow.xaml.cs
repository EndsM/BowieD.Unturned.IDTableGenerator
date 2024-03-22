using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.IDTableGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ITableStyle CurrentStyle { get; private set; }
        public static IComparer<TableRecord> CurrentComparer { get; private set; }
        public static List<TableRecord> Records { get; } = new List<TableRecord>();
        public static IEnumerable<TableRecord> OrderedRecords
        {
            get
            {
                if (CurrentComparer != null)
                {
                    var lst = new List<TableRecord>(Records);
                    lst.Sort(CurrentComparer);
                    return lst;
                }
                else
                {
                    return Records;
                }
            }
        }

        private static readonly string[] ignoreNames = new string[]
        {
            // These are exclude for they are localization files of given items of game data.
            "arabic",
            "bulgarian",
            "schinese",
            "tchinese",
            "czech",
            "danish",
            "dutch",
            "english",
            "finnish",
            "french",
            "german",
            "greek",
            "hungarian",
            "italian",
            "japanese",
            "koreana",
            "norwegian",
            "polish",
            "portuguese",
            "brazilian",
            "romanian",
            "russian",
            "spanish",
            "latam",
            "swedish",
            "thai",
            "turkish",
            "ukrainian",
            "vietnamese"
        };

        public MainWindow()
        {
            InitializeComponent();

            foreach (var mi in miStyle.Items)
            {
                (mi as MenuItem).Click += MenuItem_Style_Click;
            }

            foreach (var mi in miSortBy.Items)
            {
                (mi as MenuItem).Click += MenuItem_SortBy_Click;
            }

            btnGenerate.Click += BtnGenerate_Click;
            btnSearch.Click += BtnSearch_Click;
        }

        private void MenuItem_SortBy_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as MenuItem);

            if (s.IsChecked)
            {
                CurrentComparer = null;
            }
            else
            {
                var newComparer = s.Tag as IComparer<TableRecord>;

                CurrentComparer = newComparer;

                foreach (var mi in miSortBy.Items)
                {
                    (mi as MenuItem).IsChecked = false;
                }

                s.IsChecked = true;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                OverwritePrompt = false,
                CreatePrompt = false,
                FileName = "Select this folder"
            };

            if (sfd.ShowDialog() == true)
            {
                Records.Clear();
                // This is left-side windows' content
                lstView.Items.Clear();
                // recursively get every .dat files in game folder, then go through each of them
                foreach (var datFile in new DirectoryInfo(Path.GetDirectoryName(sfd.FileName)).GetFiles("*.dat", SearchOption.AllDirectories))
                {
                    string shortName = datFile.Name;

                    if (ignoreNames.Any(d => d + ".dat" == shortName.ToLowerInvariant()))
                        continue;

                    try
                    {
                        string[] lines = File.ReadAllLines(datFile.FullName);

                        Guid? _guid = null;
                        string _name = null;
                        string _type = null;
                        ushort? _id = null;

                        foreach (var line in lines)
                        {
                            // TODO: I looked the .dat files, if I'd like to read more properties of them, I may need to rewrite this part. Use a model to parse these .dat files
                            // Split the line to (usually) two string
                            string[] splitted = line.Split(' ');
                            // In case of a whole line without
                            if (splitted.Length < 2)
                                continue;
                            // This is the letter part of the line, if the line is two parted.
                            string value = string.Join(" ", splitted.Skip(1));

                            string lowerCased = line.ToLowerInvariant();
                            // These If actually read the first part of the line to determine what actually this line is...
                            if (lowerCased.StartsWith("id"))
                            {
                                if (ushort.TryParse(value, out ushort result))
                                    _id = result;
                            }
                            else if (lowerCased.StartsWith("guid"))
                            {
                                if (Guid.TryParse(value, out Guid result))
                                    _guid = result;
                            }
                            else if (lowerCased.StartsWith("type"))
                                _type = value;
                        }

                        if (_id == null)
                            continue;

                        string dir = datFile.DirectoryName;

                        string lPath = System.IO.Path.Combine(dir, "English.dat");
                        if (File.Exists(lPath))
                        {
                            string[] llines = File.ReadAllLines(lPath);

                            foreach (var line in llines)
                            {
                                string[] splitted = line.Split(' ');

                                if (splitted.Length < 2)
                                    continue;

                                string j = string.Join(" ", splitted.Skip(1));

                                string v = line.ToLowerInvariant();
                                if (v.StartsWith("name"))
                                {
                                    _name = j;
                                }
                            }
                        }

                        TableRecord record = new TableRecord(_guid ?? Guid.Empty, _id ?? 0, _name ?? datFile.Directory.Name, _type ?? "Unknown");

                        Records.Add(record);
                    }
                    catch
                    {
                        // wait implement 
                    }
                }

                foreach (var r in Records)
                    lstView.Items.Add(r);
            }
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            EInclude include = EInclude.Empty;

            if (chkInclID.IsChecked == true)
                include |= EInclude.ID;
            if (chkInclGUID.IsChecked == true)
                include |= EInclude.GUID;
            if (chkInclName.IsChecked == true)
                include |= EInclude.Name;
            if (chkInclType.IsChecked == true)
                include |= EInclude.Type;

            txtOutput.Text = CurrentStyle.Create(OrderedRecords, include);
        }

        private void MenuItem_Style_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as MenuItem);

            if (s.IsChecked)
                return;

            var newStyle = s.Tag as ITableStyle;

            CurrentStyle = newStyle;

            foreach (var mi in miStyle.Items)
            {
                (mi as MenuItem).IsChecked = false;
            }

            s.IsChecked = true;

            btnGenerate.IsEnabled = true;
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }
}
