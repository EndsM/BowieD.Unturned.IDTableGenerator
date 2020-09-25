﻿using Microsoft.Win32;
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
                lstView.Items.Clear();

                foreach (var fi in new DirectoryInfo(System.IO.Path.GetDirectoryName(sfd.FileName)).GetFiles("*.dat", SearchOption.AllDirectories))
                {
                    string shortName = fi.Name;

                    if (ignoreNames.Any(d => d + ".dat" == shortName.ToLowerInvariant()))
                        continue;

                    try
                    {
                        string[] lines = File.ReadAllLines(fi.FullName);

                        Guid? _guid = null;
                        string _name = null;
                        string _type = null;
                        ushort? _id = null;

                        foreach (var l in lines)
                        {
                            string[] spl = l.Split(' ');

                            if (spl.Length < 2)
                                continue;

                            string j = string.Join(" ", spl.Skip(1));

                            string v = l.ToLowerInvariant();
                            if (v.StartsWith("id"))
                            {
                                if (ushort.TryParse(j, out ushort res))
                                    _id = res;
                            }
                            else if (v.StartsWith("guid"))
                            {
                                if (Guid.TryParse(j, out Guid res))
                                    _guid = res;
                            }
                            else if (v.StartsWith("type"))
                                _type = j;
                        }

                        if (_id == null)
                            continue;

                        string dir = fi.DirectoryName;

                        string lPath = System.IO.Path.Combine(dir, "English.dat");
                        if (File.Exists(lPath))
                        {
                            string[] llines = File.ReadAllLines(lPath);

                            foreach (var l in llines)
                            {
                                string[] spl = l.Split(' ');

                                if (spl.Length < 2)
                                    continue;

                                string j = string.Join(" ", spl.Skip(1));

                                string v = l.ToLowerInvariant();
                                if (v.StartsWith("name"))
                                {
                                    _name = j;
                                }
                            }
                        }

                        TableRecord tr = new TableRecord(_guid ?? Guid.Empty, _id ?? 0, _name ?? fi.Directory.Name, _type ?? "Unknown");

                        Records.Add(tr);
                    }
                    catch { }
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
            AboutWindow aw = new AboutWindow();
            aw.ShowDialog();
        }
    }
}
