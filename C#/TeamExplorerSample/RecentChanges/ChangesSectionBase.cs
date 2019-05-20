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
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

namespace Microsoft.TeamExplorerSample.RecentChanges
{
    /// <summary>
    /// Changes section base class.
    /// </summary>
    public abstract class ChangesSectionBase : TeamExplorerBaseSection
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangesSectionBase()
            : base()
        {
            this.IsVisible = true;
            this.IsExpanded = true;
            this.IsBusy = false;
            this.SectionContent = new ChangesSectionView();
            this.View.ParentSection = this;
        }

        /// <summary>
        /// Get the view.
        /// </summary>
        protected ChangesSectionView View
        {
            get { return this.SectionContent as ChangesSectionView; }
        }

        /// <summary>
        /// Store the base title without any decorations for later use.
        /// </summary>
        private string BaseTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Initialize override.
        /// </summary>
        public async override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);

            // Save off the base title that was set during the ctor
            this.BaseTitle = this.Title;

            // If the user navigated back to this page, there could be saved context
            // info that is passed in
            if (e.Context != null && e.Context is ChangesSectionContext)
            {
                // Restore the context instead of refreshing
                ChangesSectionContext context = (ChangesSectionContext)e.Context;
                this.Changesets = context.Changesets;
                this.View.SelectedIndex = context.SelectedIndex;
            }
            else
            {
                // Kick off the initial refresh
                await RefreshAsync();
            }
        }

        /// <summary>
        /// Save contextual information about the current section state.
        /// </summary>
        public override void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
            base.SaveContext(sender, e);

            // Save our current changeset list, selected item, and topmost item
            // so they can be quickly restored when the user navigates back to
            // the page
            ChangesSectionContext context = new ChangesSectionContext();
            context.Changesets = this.Changesets;
            context.SelectedIndex = this.View.SelectedIndex;
            
            e.Context = context;
        }

        /// <summary>
        /// Refresh override.
        /// </summary>
        public async override void Refresh()
        {
            base.Refresh();
            await RefreshAsync();
        }

        /// <summary>
        /// Refresh the changeset data asynchronously.
        /// </summary>
        private async Task RefreshAsync()
        {
            try
            {
                // Set our busy flag and clear the previous data
                this.IsBusy = true;
                this.Changesets.Clear();

                ObservableCollection<Changeset> changesets = new ObservableCollection<Changeset>();
                
                // Make the server call asynchronously to avoid blocking the UI
                await Task.Run(() =>
                {
                    ITeamFoundationContext context = this.CurrentContext;
                    if (context != null && context.HasCollection && context.HasTeamProject)
                    {
                        VersionControlServer vcs = context.TeamProjectCollection.GetService<VersionControlServer>();
                        if (vcs != null)
                        {
                            // Ask the derived section for the history parameters
                            string user;
                            int maxCount;
                            GetHistoryParameters(vcs, out user, out maxCount);

                            string path = "$/" + context.TeamProjectName;
                            foreach (Changeset changeset in vcs.QueryHistory(path, VersionSpec.Latest, 0, RecursionType.Full,
                                                                             user, null, null, maxCount, false, true))
                            {
                                changesets.Add(changeset);
                            }
                        }
                    }
                });

                // Now back on the UI thread, update the bound collection and section title
                this.Changesets = changesets;
                this.Title = this.Changesets.Count > 0 ? String.Format("{0} ({1})", this.BaseTitle, this.Changesets.Count)
                                                       : this.BaseTitle;
            }
            catch (Exception ex)
            {
                ShowNotification(ex.Message, NotificationType.Error);
            }
            finally
            {
                // Always clear our busy flag when done
                this.IsBusy = false;
            }
        }

        /// <summary>
        /// Get the parameters for the history query.
        /// </summary>
        protected abstract void GetHistoryParameters(VersionControlServer vcs, out string user, out int maxCount);

        /// <summary>
        /// ContextChanged override.
        /// </summary>
        protected override async void ContextChanged(object sender, TeamFoundation.Client.ContextChangedEventArgs e)
        {
            base.ContextChanged(sender, e);
            
            // If the team project collection or team project changed, refresh
            // the data for this section
            if (e.TeamProjectCollectionChanged || e.TeamProjectChanged)
            {
                await RefreshAsync();
            }
        }

        /// <summary>
        /// List of changesets.
        /// </summary>
        public ObservableCollection<Changeset> Changesets
        {
            get { return m_changesets; }
            protected set { m_changesets = value; RaisePropertyChanged("Changesets"); }
        }
        private ObservableCollection<Changeset> m_changesets = new ObservableCollection<Changeset>();

        /// <summary>
        /// Class to preserve the contextual information for this section.
        /// </summary>
        private class ChangesSectionContext
        {
            public ObservableCollection<Changeset> Changesets { get; set; }
            public int SelectedIndex { get; set; }
        }

        /// <summary>
        /// View details for the changeset.
        /// </summary>
        public void ViewChangesetDetails(int changesetId)
        {
            // The preferred method would be to use VersionControlExt.ViewChangesetDetails, but that
            // method still uses the modal Changeset Details dialog at this time.  As a temporary
            // workaround, navigate to the Changeset Details Team Explorer page and pass the
            // changeset ID.

            //VersionControlExt vcExt = Helpers.GetVersionControlExt(ParentSection.ServiceProvider);
            //if (vcExt != null)
            //{
            //    vcExt.ViewChangesetDetails(changesetId);
            //}

            ITeamExplorer teamExplorer = this.GetService<ITeamExplorer>();
            if (teamExplorer != null)
            {
                teamExplorer.NavigateToPage(new Guid(TeamExplorerPageIds.ChangesetDetails), changesetId);
            }
        }

        /// <summary>
        /// View history using the same parameters as this section.
        /// </summary>
        public void ViewHistory()
        {
            ITeamFoundationContext context = this.CurrentContext;
            if (context != null && context.HasCollection && context.HasTeamProject)
            {
                VersionControlServer vcs = context.TeamProjectCollection.GetService<VersionControlServer>();
                if (vcs != null)
                {
                    // Ask the derived section for the history parameters
                    string path = "$/" + context.TeamProjectName;
                    string user;
                    int maxCount;
                    GetHistoryParameters(vcs, out user, out maxCount);

                    VersionControlExt vcExt = Helpers.GetVersionControlExt(this.ServiceProvider);
                    if (vcExt != null)
                    {
                        vcExt.History.Show(path, VersionSpec.Latest, 0, RecursionType.Full,
                                           user, null, null, Int32.MaxValue, true);
                    }
                }
            }
        }
    }
}
