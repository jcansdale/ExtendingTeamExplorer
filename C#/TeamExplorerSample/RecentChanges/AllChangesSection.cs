/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace Microsoft.TeamExplorerSample.RecentChanges
{
    /// <summary>
    /// All Changes section.
    /// </summary>
    [TeamExplorerSection(AllChangesSection.SectionId, RecentChangesPage.PageId, 20)]
    public class AllChangesSection : ChangesSectionBase
    {
        #region Members

        public const string SectionId = "25D8768D-1C15-4975-9EA9-648CF134C62A";

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public AllChangesSection()
            : base()
        {
            this.Title = "All Changes";
        }

        /// <summary>
        /// Get the parameters for the history query.
        /// </summary>
        protected override void GetHistoryParameters(VersionControlServer vcs, out string user, out int maxCount)
        {
            user = null;
            maxCount = 100;
        }
    }
}
