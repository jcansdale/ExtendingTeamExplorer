/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

namespace Microsoft.TeamExplorerSample.RecentChanges
{
    /// <summary>
    /// Interaction logic for ChangesSectionView.xaml
    /// </summary>
    public partial class ChangesSectionView : UserControl
    {
        public ChangesSectionView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Parent section.
        /// </summary>
        public ChangesSectionBase ParentSection
        {
            get { return (ChangesSectionBase)GetValue(ParentSectionProperty); }
            set { SetValue(ParentSectionProperty, value); }
        }
        public static readonly DependencyProperty ParentSectionProperty =
            DependencyProperty.Register("ParentSection", typeof(ChangesSectionBase), typeof(ChangesSectionView));

        /// <summary>
        /// View changeset details.
        /// </summary>
        private void ViewChangesetDetails()
        {
            if (changesetList.SelectedItems.Count == 1)
            {
                Changeset changeset = changesetList.SelectedItems[0] as Changeset;
                if (changeset != null)
                {
                    this.ParentSection.ViewChangesetDetails(changeset.ChangesetId);
                }
            }
        }

        /// <summary>
        /// Changeset list MouseDoubleClick event handler.
        /// </summary>
        private void changesetList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && changesetList.SelectedItems.Count == 1)
            {
                ViewChangesetDetails();
            }
        }

        /// <summary>
        /// Changeset list KeyDown event handler.
        /// </summary>
        private void changesetList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && changesetList.SelectedItems.Count == 1)
            {
                ViewChangesetDetails();
            }
        }

        /// <summary>
        /// History link Click event handler.
        /// </summary>
        private void historyLink_Click(object sender, RoutedEventArgs e)
        {
            this.ParentSection.ViewHistory();
        }

        /// <summary>
        /// Get/set the selected index.
        /// </summary>
        public int SelectedIndex
        {
            get { return changesetList.SelectedIndex; }
            set { changesetList.SelectedIndex = value; changesetList.ScrollIntoView(changesetList.SelectedItem); }
        }
    }

    #region Converters

    /// <summary>
    /// Changeset comment converter class.
    /// </summary>
    public class ChangesetCommentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string comment = (value is string) ? (string)value : String.Empty;
            StringBuilder sb = new StringBuilder(comment);
            sb.Replace('\r', ' ');
            sb.Replace('\n', ' ');
            sb.Replace('\t', ' ');

            if (sb.Length > 64)
            {
                sb.Remove(61, sb.Length - 61);
                sb.Append("...");
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    #endregion
}
