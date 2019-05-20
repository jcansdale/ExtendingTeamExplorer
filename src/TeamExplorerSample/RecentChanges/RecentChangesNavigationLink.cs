/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.TeamExplorerSample.RecentChanges
{
    [TeamExplorerNavigationLink(RecentChangesNavigationLink.LinkId, TeamExplorerNavigationItemIds.PendingChanges, 200)]
    public class RecentChangesNavigationLink : TeamExplorerBaseNavigationLink
    {
        #region Members

        public const string LinkId = "0C8891AB-0EA3-4249-9B33-99EE80A53C75";

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        [ImportingConstructor]
        public RecentChangesNavigationLink([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.Text = "Recent Changes";
            this.IsVisible = true;
            this.IsEnabled = true;
        }

        /// <summary>
        /// Execute the link action.
        /// </summary>
        public override void Execute()
        {
            // Navigate to the recent changes page
            ITeamExplorer teamExplorer = GetService<ITeamExplorer>();
            if (teamExplorer != null)
            {
                teamExplorer.NavigateToPage(new Guid(RecentChangesPage.PageId), null);
            }
        }

        public override void Invalidate()
        {
            base.Invalidate();
            this.IsEnabled = true;
            this.IsVisible = true;
        }
    }
}
