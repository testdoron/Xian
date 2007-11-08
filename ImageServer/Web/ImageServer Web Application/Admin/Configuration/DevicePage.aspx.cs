using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Web.Common;
using System.Collections.Generic;
using ImageServerWebApplication.Common;

namespace ImageServerWebApplication.Admin.Configuration
{
    /// <summary>
    /// Device Configuration Web Page.
    /// </summary>
    public partial class DevicePage : System.Web.UI.Page
    {
        #region Private members
        // Map between the server partition and the panel
        private IDictionary<ServerEntityKey, DevicePanel> _mapDevicePanel = new Dictionary<ServerEntityKey, DevicePanel>();

        // the controller used for database interaction
        private DeviceConfigurationController _controller = new DeviceConfigurationController();

        #endregion Private members

        #region Protected methods

        /// <summary>
        /// Set up the event handlers for child controls.
        /// </summary>
        protected void SetupEventHandlers()
        {
            AddDeviceControl1.OKClicked += delegate(Device dev)
                                               {
                                                   // Commit the new device into database
                                                   if (_controller.AddDevice(dev))
                                                   {
                                                       DevicePanel panel = _mapDevicePanel[dev.ServerPartition.GetKey()];
                                                       panel.UpdateUI();
                                                   }
                                                   
                                               };
            EditDeviceControl1.OKClicked += delegate(Device dev, ServerPartition oldPartition)
                                                {
                                                    // Update the device information and reload the list in the affected partitions
                                                    if (_controller.UpdateDevice(dev))
                                                    {

                                                        DevicePanel oldPanel = _mapDevicePanel[oldPartition.GetKey()];
                                                        if (oldPanel!=null)
                                                            oldPanel.UpdateUI();

                                                        DevicePanel newPanel = _mapDevicePanel[dev.ServerPartition.GetKey()];
                                                        if (newPanel != null) // the new partition may not be visible
                                                            newPanel.UpdateUI();
                                                    }

                                                };

            ConfirmDialog1.Confirmed += delegate(object data)
                                           {
                                               // delete the device and reload the affected partition.

                                               Device dev = data as Device;
                                               DevicePanel oldPanel = _mapDevicePanel[dev.ServerPartition.GetKey()];
                                               _controller.DeleteDevice(dev);
                                               if (oldPanel!=null)
                                                    oldPanel.UpdateUI();
                                           };
        }

        protected void SetupLoadPartitionTabs()
        {
            int n = 0; 
            
            TabContainer1.Tabs.Clear();
            IList<ServerPartition> partitions = GetPartitions();
            foreach (ServerPartition part in partitions)
            {
                n++;

                // create a tab
                TabPanel tabPannel = new TabPanel();
                tabPannel.HeaderText = part.Description;
                tabPannel.ID = "Tab_" + n;

                // create a device panel
                DevicePanel devPanel = LoadControl("DevicePanel.ascx") as DevicePanel;
                devPanel.Partition = part;
                devPanel.ID = "DevicePanel_" + n;

                // put the panel into a lookup table to be used later
                _mapDevicePanel[part.GetKey()] = devPanel;

                // Setup delegates 
                devPanel.AddDeviceDelegate = delegate(DeviceConfigurationController controller, ServerPartition partition)
                                           {
                                               // Populate the add device dialog and display it
                                               IList<ServerPartition> list = new List<ServerPartition>();
                                               list.Add(partition);
                                               AddDeviceControl1.Partitions = list;
                                               AddDeviceControl1.Show();
                                           };

                devPanel.EditDeviceDelegate =
                    delegate(DeviceConfigurationController controller, ServerPartition partition, Device dev)
                        {
                            // Populate the edit device dialog and display it
                            EditDeviceControl1.Device = dev;
                            EditDeviceControl1.Partitions = controller.GetServerPartitions();
                            EditDeviceControl1.Show();
                        };

                devPanel.DeleteDeviceDelegate = delegate(DeviceConfigurationController controller, ServerPartition partition, Device dev)
                                              {
                                                  ConfirmDialog1.Message = string.Format("Are you sure to remove {0} from {1}?", dev.AeTitle, partition.Description);
                                                  ConfirmDialog1.MessageType = ConfirmDialog.MessageTypeEnum.WARNING;
                                                  ConfirmDialog1.Data = dev;
                                                  ConfirmDialog1.Show();
                                              };


                // Add the device panel into the tab
                tabPannel.Controls.Add(devPanel);

                // Add the tab into the tabstrip
                TabContainer1.Tabs.Add(tabPannel);
                

                
            }
        }

        /// <summary>
        /// Retrieves the partitions to be rendered in the page.
        /// </summary>
        /// <returns></returns>
        private IList<ServerPartition> GetPartitions()
        {
            // TODO We may want to add context or user preference here to specify which partitions to load

            IList<ServerPartition> list = _controller.GetServerPartitions();
            return list;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SetupEventHandlers();

            SetupLoadPartitionTabs();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        #endregion  Protected methods
        
    }
}
