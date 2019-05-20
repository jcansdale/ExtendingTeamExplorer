/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/
using System;
using Microsoft.TeamFoundation.Controls;

namespace Microsoft.TeamExplorerSample.RecentChanges
{
    /// <summary>
    /// Recent changes page.
    /// </summary>
    [TeamExplorerPage(RecentChangesPage.PageId)]
    public class RecentChangesPage : TeamExplorerBasePage
    {
        #region Members

        public const string PageId = "22BDBABE-D585-40DC-8B20-191154B8CD1A";

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecentChangesPage()
            : base()
        {
            this.Title = "Recent Changes";
            this.PageContent = new RecentChangesPageView();
        }

        /// <summary>
        /// Refresh override.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
        }

        /// <summary>
        /// ContextChanged override.
        /// </summary>
        protected override void ContextChanged(object sender, TeamFoundation.Client.ContextChangedEventArgs e)
        {
            base.ContextChanged(sender, e);
        }
    }
}
