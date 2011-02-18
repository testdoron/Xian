#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Web.Application.Controls;
using ClearCanvas.ImageServer.Web.Application.Helpers;
using ClearCanvas.ImageServer.Web.Common.Data;
using ClearCanvas.ImageServer.Web.Common.Data.DataSource;
using ClearCanvas.ImageServer.Web.Common.WebControls.UI;
using AuthorityTokens=ClearCanvas.ImageServer.Enterprise.Authentication.AuthorityTokens;
using ClearCanvas.ImageServer.Web.Application.App_GlobalResources;

[assembly: WebResource("ClearCanvas.ImageServer.Web.Application.Pages.Queues.RestoreQueue.SearchPanel.js", "application/x-javascript")]

namespace ClearCanvas.ImageServer.Web.Application.Pages.Queues.RestoreQueue
{
    [ClientScriptResource(ComponentType="ClearCanvas.ImageServer.Web.Application.Pages.Queues.RestoreQueue.SearchPanel", ResourcePath="ClearCanvas.ImageServer.Web.Application.Pages.Queues.RestoreQueue.SearchPanel.js")]
    public partial class SearchPanel : AJAXScriptControl
    {
        #region Private members

        private readonly RestoreQueueController _controller = new RestoreQueueController();
    	private ServerPartition _serverPartition;

    	#endregion Private members

        #region Public Properties

        [ExtenderControlProperty]
        [ClientPropertyName("DeleteButtonClientID")]
        public string DeleteButtonClientID
        {
            get { return DeleteItemButton.ClientID; }
        }

        [ExtenderControlProperty]
        [ClientPropertyName("ItemListClientID")]
        public string ItemListClientID
        {
            get { return RestoreQueueItemList.RestoreQueueGrid.ClientID; }
        }

		[ExtenderControlProperty]
		[ClientPropertyName("OpenButtonClientID")]
		public string OpenButtonClientID
		{
			get { return ViewStudyDetailsButton.ClientID; }
		}

		/// <summary>
		/// Gets the <see cref="Model.ServerPartition"/> associated with this search panel.
		/// </summary>
		public ServerPartition ServerPartition
		{
			get { return _serverPartition; }
			set { _serverPartition = value; }
		}

		[ExtenderControlProperty]
		[ClientPropertyName("OpenStudyPageUrl")]
		public string OpenStudyPageUrl
		{
			get { return Page.ResolveClientUrl(ImageServerConstants.PageURLs.StudyDetailsPage); }
		}
        #endregion Public Properties  

        #region Public Methods

        /// <summary>
        /// Remove all filter settings.
        /// </summary>
        public void Clear()
        {
            PatientId.Text = string.Empty;
            PatientName.Text = string.Empty;
            ScheduleDate.Text = string.Empty;
            StatusFilter.SelectedIndex = 0;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ClearScheduleDateButton.OnClientClick = ScriptHelper.ClearDate(ScheduleDate.ClientID, ScheduleDateCalendarExtender.ClientID);
                          
            // setup child controls
            GridPagerTop.InitializeGridPager(App_GlobalResources.Labels.GridPagerQueueSingleItem, App_GlobalResources.Labels.GridPagerQueueMultipleItems, RestoreQueueItemList.RestoreQueueGrid, delegate { return RestoreQueueItemList.ResultCount; }, ImageServerConstants.GridViewPagerPosition.Top);
            RestoreQueueItemList.Pager = GridPagerTop;

            MessageBox.Confirmed += delegate(object data)
                            {
                                if (data is IList<Model.RestoreQueue>)
                                {
                                    IList<Model.RestoreQueue> items = data as IList<Model.RestoreQueue>;
                                    foreach (Model.RestoreQueue item in items)
                                    {
                                        _controller.DeleteRestoreQueueItem(item);
                                    }
                                }
                                else if (data is Model.RestoreQueue)
                                {
                                    Model.RestoreQueue item = data as Model.RestoreQueue;
                                    _controller.DeleteRestoreQueueItem(item);
                                }

                                DataBind();
                                SearchUpdatePanel.Update(); // force refresh

                            };

			RestoreQueueItemList.DataSourceCreated += delegate(RestoreQueueDataSource source)
										{
											source.Partition = ServerPartition;
                                            source.DateFormats = ScheduleDateCalendarExtender.Format;

                                            if (!String.IsNullOrEmpty(StatusFilter.SelectedValue) && StatusFilter.SelectedIndex > 0)
                                                source.StatusEnum = RestoreQueueStatusEnum.GetEnum(StatusFilter.SelectedValue);
                                            if (!String.IsNullOrEmpty(PatientId.Text))
												source.PatientId = SearchHelper.TrailingWildCard(PatientId.Text);
											if (!String.IsNullOrEmpty(PatientName.Text))
												source.PatientName = SearchHelper.NameWildCard(PatientName.Text);
											if (!String.IsNullOrEmpty(ScheduleDate.Text))
												source.ScheduledDate = ScheduleDate.Text;
										};
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IList<RestoreQueueStatusEnum> statusItems = RestoreQueueStatusEnum.GetAll();

            int prevSelectedIndex = StatusFilter.SelectedIndex;
            StatusFilter.Items.Clear();
            StatusFilter.Items.Add(new ListItem(SR.All, "All"));
            foreach (RestoreQueueStatusEnum s in statusItems)
                StatusFilter.Items.Add(new ListItem(ServerEnumDescription.GetLocalizedDescription(s), s.Lookup));
            StatusFilter.SelectedIndex = prevSelectedIndex;

            DeleteItemButton.Roles = AuthorityTokens.RestoreQueue.Delete;
			ViewStudyDetailsButton.Roles = AuthorityTokens.Study.View;

			if (!IsPostBack && !Page.IsAsync)
			{
				string patientID = Server.UrlDecode(Request["PatientID"]);
				string patientName = Server.UrlDecode(Request["PatientName"]);
				string partitionKey = Request["PartitionKey"];

				if (patientID != null && patientName != null && partitionKey != null)
				{
					var controller = new ServerPartitionConfigController();
					ServerPartition = controller.GetPartition(new ServerEntityKey("ServerPartition", partitionKey));

					PatientId.Text = patientID;
					PatientName.Text = patientName;

					RestoreQueueItemList.SetDataSource();
					RestoreQueueItemList.Refresh();
				}
			}
        }

        protected void SearchButton_Click(object sender, ImageClickEventArgs e)
        {
            RestoreQueueItemList.Refresh();
        }

        protected void DeleteItemButton_Click(object sender, EventArgs e)
        {
            RestoreQueueItemList.RefreshCurrentPage();
            
            IList<Model.RestoreQueue> items = RestoreQueueItemList.SelectedItems;

            if (items != null && items.Count>0)
            {
                if (items.Count > 1) MessageBox.Message = string.Format(App_GlobalResources.SR.MultipleRestoreQueueDelete);
                else MessageBox.Message = string.Format(App_GlobalResources.SR.SingleRestoreQueueDelete);

                MessageBox.Message += "<table>";
                foreach (Model.RestoreQueue item in items)
                {
                    String text = "";
                    String.Format("<tr align='left'><td>{0}:{1}</td></tr>",
                                    SR.StudyInstanceUID,
                                    StudyStorage.Load(item.StudyStorageKey).StudyInstanceUid);
                    MessageBox.Message += text;
                }
                MessageBox.Message += "</table>";

                MessageBox.MessageType = MessageBox.MessageTypeEnum.YESNO;
                MessageBox.Data = items;
                MessageBox.Show();
            }
        }

        #endregion Protected Methods
    }
}