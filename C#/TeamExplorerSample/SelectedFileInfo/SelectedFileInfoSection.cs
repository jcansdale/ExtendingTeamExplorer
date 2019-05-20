/*
* Copyright (c) Microsoft Corporation. All rights reserved. This code released
* under the terms of the Microsoft Limited Public License (MS-LPL).
*/
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Controls.Extensibility;

namespace Microsoft.TeamExplorerSample.SelectedFileInfo
{
    /// <summary>
    /// Selected file info section.
    /// </summary>
    [TeamExplorerSection(SelectedFileInfoSection.SectionId, TeamExplorerPageIds.PendingChanges, 900)]
    public class SelectedFileInfoSection : TeamExplorerBaseSection
    {
        #region Members

        public const string SectionId = "50948F36-9223-4E8C-A8A5-37A6225AE3E2";

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public SelectedFileInfoSection()
            : base()
        {
            this.Title = "Selected File Info";
            this.IsExpanded = true;
            this.IsBusy = false;
            this.SectionContent = new SelectedFileInfoView();
            this.View.ParentSection = this;
        }

        /// <summary>
        /// Get the view.
        /// </summary>
        protected SelectedFileInfoView View
        {
            get { return this.SectionContent as SelectedFileInfoView; }
        }

        /// <summary>
        /// Initialize override.
        /// </summary>
        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);

            // Find the Pending Changes extensibility service and sign up for
            // property change notifications
            IPendingChangesExt pcExt = this.GetService<IPendingChangesExt>();
            if (pcExt != null)
            {
                pcExt.PropertyChanged += pcExt_PropertyChanged;
            }
        }

        /// <summary>
        /// Dispose override.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            IPendingChangesExt pcExt = this.GetService<IPendingChangesExt>();
            if (pcExt != null)
            {
                pcExt.PropertyChanged -= pcExt_PropertyChanged;
            }
        }

        /// <summary>
        /// Pending Changes Extensibility PropertyChanged event handler.
        /// </summary>
        private void pcExt_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedIncludedItems":
                    Refresh();
                    break;
            }
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
                this.ServerPath = null;
                this.LocalPath = null;
                this.LatestVersion = null;
                this.WorkspaceVersion = null;
                this.Encoding = null;

                // Temp variables to hold the data as we retrieve it
                string serverPath = null, localPath = null;
                string latestVersion = null, workspaceVersion = null;
                string encoding = null;

                // Grab the selected included item from the Pending Changes extensibility object
                PendingChangesItem selectedItem = null;
                IPendingChangesExt pcExt = GetService<IPendingChangesExt>();
                if (pcExt != null && pcExt.SelectedIncludedItems.Length > 0)
                {
                    selectedItem = pcExt.SelectedIncludedItems[0];
                }

                if (selectedItem != null && selectedItem.IsPendingChange && selectedItem.PendingChange != null)
                {
                    // Check for rename
                    if (selectedItem.PendingChange.IsRename && selectedItem.PendingChange.SourceServerItem != null)
                    {
                        serverPath = selectedItem.PendingChange.SourceServerItem;
                    }
                    else
                    {
                        serverPath = selectedItem.PendingChange.ServerItem;
                    }

                    localPath = selectedItem.ItemPath;
                    workspaceVersion = selectedItem.PendingChange.Version.ToString();
                    encoding = selectedItem.PendingChange.EncodingName;
                }
                else
                {
                    serverPath = String.Empty;
                    localPath = selectedItem != null ? selectedItem.ItemPath : String.Empty;
                    latestVersion = String.Empty;
                    workspaceVersion = String.Empty;
                    encoding = String.Empty;
                }

                // Go get any missing data from the server
                if (latestVersion == null || encoding == null)
                {
                    // Make the server call asynchronously to avoid blocking the UI
                    await Task.Run(() =>
                    {
                        ITeamFoundationContext context = this.CurrentContext;
                        if (context != null && context.HasCollection)
                        {
                            VersionControlServer vcs = context.TeamProjectCollection.GetService<VersionControlServer>();
                            if (vcs != null)
                            {
                                Item item = vcs.GetItem(serverPath);
                                if (item != null)
                                {
                                    latestVersion = latestVersion ?? item.ChangesetId.ToString();
                                    encoding = encoding ?? FileType.GetEncodingName(item.Encoding);
                                }
                            }
                        }
                    });
                }

                // Now back on the UI thread, update the view data
                this.ServerPath = serverPath;
                this.LocalPath = localPath;
                this.LatestVersion = latestVersion;
                this.WorkspaceVersion = workspaceVersion;
                this.Encoding = encoding;
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
        /// Get/set the server path.
        /// </summary>
        public string ServerPath
        {
            get { return m_serverPath; }
            set { m_serverPath = value; RaisePropertyChanged("ServerPath"); }
        }
        private string m_serverPath = String.Empty;

        /// <summary>
        /// Get/set the local path.
        /// </summary>
        public string LocalPath
        {
            get { return m_localPath; }
            set { m_localPath = value; RaisePropertyChanged("LocalPath"); }
        }
        private string m_localPath = String.Empty;

        /// <summary>
        /// Get/set the latest version.
        /// </summary>
        public string LatestVersion
        {
            get { return m_latestVersion; }
            set { m_latestVersion = value; RaisePropertyChanged("LatestVersion"); }
        }
        private string m_latestVersion = String.Empty;

        /// <summary>
        /// Get/set the workspace version.
        /// </summary>
        public string WorkspaceVersion
        {
            get { return m_workspaceVersion; }
            set { m_workspaceVersion = value; RaisePropertyChanged("WorkspaceVersion"); }
        }
        private string m_workspaceVersion = String.Empty;

        /// <summary>
        /// Get/set the encoding.
        /// </summary>
        public string Encoding
        {
            get { return m_encoding; }
            set { m_encoding = value; RaisePropertyChanged("Encoding"); }
        }
        private string m_encoding = String.Empty;
    }
}
