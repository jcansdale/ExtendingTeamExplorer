/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.TeamExplorerSample.SelectedFileInfo
{
    /// <summary>
    /// Interaction logic for SelectedFileInfoView.xaml
    /// </summary>
    public partial class SelectedFileInfoView : UserControl
    {
        public SelectedFileInfoView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Parent section.
        /// </summary>
        public SelectedFileInfoSection ParentSection
        {
            get { return (SelectedFileInfoSection)GetValue(ParentSectionProperty); }
            set { SetValue(ParentSectionProperty, value); }
        }
        public static readonly DependencyProperty ParentSectionProperty =
            DependencyProperty.Register("ParentSection", typeof(SelectedFileInfoSection), typeof(SelectedFileInfoView));

        /// <summary>
        /// Set encoding button Click event handler.
        /// </summary>
        private void setEncodingButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
