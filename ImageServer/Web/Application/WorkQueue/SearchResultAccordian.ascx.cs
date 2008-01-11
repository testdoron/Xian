using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Web.Application.WorkQueue
{
    /// <summary>
    /// WorkQueue search result panel.
    /// </summary>
    public partial class SearchResultAccordian : System.Web.UI.UserControl
    {
        #region Private Members

        private IList<Model.WorkQueue> _workqueues = new List<Model.WorkQueue>();
        private Study _selectedWorkQueue;
        private int _pageCount;
        private int _pageIndex = 0;
        private int _pageSize = 15;

        #endregion Private Members

        #region Public Properties

        public int PageSize
        {
            get { return _pageSize; }
        }

       
        public IList<Model.WorkQueue> WorkQueues
        {
            get { return _workqueues; }
            set
            {
                _workqueues = value;
                PageCount = _workqueues.Count/PageSize + 1;
            }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value;
                ViewState.Add("PageIndex", value);
            }
        }

        #endregion Public Properties

        #region Protected Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            RegisterSelectRowScript();
        }

        protected void RegisterSelectRowScript()
        {
            string script =
                @"<script type='text/javascript'>
                    function " + ClientID + @"_SelectWorkQueue(uid)
                    {
                        field = document.getElementById('" + SelectedWorkQueueGUID.ClientID + @"');            
                        field.value = uid;
                    }
                    </script>
                ";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SelectWorkQueue", script);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (ViewState["PageIndex"] != null)
                {
                    PageIndex = (int) ViewState["PageIndex"];
                    if (PageIndex > PageCount)
                        PageIndex = PageCount;
                }
            }
            UpdatePager();
        }

        protected void PageButtonClick(object sender, CommandEventArgs e)
        {
            switch (e.CommandArgument.ToString().ToLower())
            {
                case "first":
                    PageIndex = 0;
                    break;
                case "prev":
                    if (PageIndex > 0)
                        PageIndex = PageIndex - 1;
                    break;
                case "next":
                    if (PageIndex < PageCount)
                        PageIndex = PageIndex + 1;
                    break;
                case "last":
                    PageIndex = PageCount;
                    break;
            }

            UpdatePager();
            DataBind();
        }

        /// <summary>
        /// Updates the grid pager based on the current list.
        /// </summary>
        protected void UpdatePager()
        {
            #region update pager of the gridview if it is used

            // Show Number of studies in the list
            PagerStudyCountLabel.Text = string.Format("{0} studies", WorkQueues.Count);

            // Show current page and the number of pages for the list
            PagerPagingLabel.Text = string.Format("Page {0} of {1}", PageCount == 0 ? 0 : PageIndex + 1, PageCount);

            // Enable/Disable the "Prev" page button
            ImageButton btn = PrevPageButton;
            if (btn != null)
            {
                if (WorkQueues.Count == 0 || PageIndex == 0)
                {
                    btn.ImageUrl = "~/images/icons/BackDisabled.png";
                    btn.Enabled = false;
                }
                else
                {
                    btn.ImageUrl = "~/images/icons/BackEnabled.png";
                    btn.Enabled = true;
                }

                btn.Style.Add("cursor", "hand");
            }

            // Enable/Disable the "Next" page button
            btn = NextPageButton;
            if (btn != null)
            {
                if (WorkQueues.Count == 0 || PageIndex == PageCount - 1)
                {
                    btn.ImageUrl = "~/images/icons/ForwardDisabled.png";
                    btn.Enabled = false;
                }
                else
                {
                    btn.ImageUrl = "~/images/icons/ForwardEnabled.png";
                    btn.Enabled = true;
                }

                btn.Style.Add("cursor", "hand");
            }

            #endregion
        }

        #endregion Protected Methods

        #region Public Methods

        public override void DataBind()
        {
            base.DataBind();

            MainAccordian.Panes.Clear();
            MainAccordian.Visible = true;
            MainAccordian.SuppressHeaderPostbacks = true;
            MainAccordian.SelectedIndex = -1;
            

            for (int i = PageIndex*PageSize; (i < (PageIndex + 1)*PageSize) && i < WorkQueues.Count; i++)
            {
                AccordionPane pane = new AccordionPane();
                
                WorkQueueSummaryPanel workQueueSummaryPanel =
                    LoadControl("WorkQueueSummaryPanel.ascx") as WorkQueueSummaryPanel;
                WorkQueueDetailsPanel workQueueDetailsPanel =
                    LoadControl("WorkQueueDetailsPanel.ascx") as WorkQueueDetailsPanel;

                WorkQueueDetailsAssembler workqueueDetailsAssembler = new WorkQueueDetailsAssembler();
                WorkQueueDetails workqueueDetails = workqueueDetailsAssembler.CreateWorkQueueDetail(WorkQueues[i]);

                WorkQueueSummaryAssembler workqueueSummaryAssembler = new WorkQueueSummaryAssembler();
                WorkQueueSummary workqueueSummary = workqueueSummaryAssembler.CreateWorkQueueSummary(workqueueDetails);

                workQueueDetailsPanel.WorkQueueDetails = workqueueDetails;
                workQueueSummaryPanel.WorkQueueSummary = workqueueSummary;


                // Add onclick event 
                pane.HeaderContainer.Attributes.Add("onClick", ClientID + "_SelectWorkQueue('" + WorkQueues[i].GetKey().Key.ToString() + "');");


                pane.HeaderContainer.Controls.Add(workQueueSummaryPanel);
                pane.ContentContainer.Controls.Add(workQueueDetailsPanel);

                pane.HeaderCssClass = "CSSAccordianStudyHeader";

                pane.ContentCssClass = "CSSAccordionStudyDetails";
                pane.BorderWidth = new Unit(0d);
                pane.Visible = true;


                if (WorkQueues[i].GetKey().Key.ToString() == SelectedWorkQueueGUID.Value)
                {
                    MainAccordian.SelectedIndex = i;
                }

                MainAccordian.Panes.Add(pane);
            }

            if (MainAccordian.SelectedIndex<0)
            {
                SelectedWorkQueueGUID.Value = "";
            }

            MainAccordian.RequireOpenedPane = false;

            
        }




        #endregion Public Methods
    }
}