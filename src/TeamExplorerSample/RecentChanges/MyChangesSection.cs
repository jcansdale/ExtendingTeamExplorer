/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
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
    /// Changes section.
    /// </summary>
    [TeamExplorerSection(MyChangesSection.SectionId, RecentChangesPage.PageId, 10)]
    public class MyChangesSection : ChangesSectionBase
    {
        #region Members

        public const string SectionId = "D1DA4215-B5B6-4787-B7BB-5679493ABD05";

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyChangesSection()
            : base()
        {
            this.Title = "My Changes";
        }

        /// <summary>
        /// Get the parameters for the history query.
        /// </summary>
        protected override void GetHistoryParameters(VersionControlServer vcs, out string user, out int maxCount)
        {
            user = vcs.AuthorizedUser;
            maxCount = 10;
        }
    }
}
