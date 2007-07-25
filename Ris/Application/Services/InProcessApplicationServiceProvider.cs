using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Core;

namespace ClearCanvas.Ris.Application.Services
{
    /// <summary>
    /// This service provider allows the application server to make use of application services internally
    /// by providing these services in-process.
    /// </summary>
    [ExtensionOf(typeof(ServiceProviderExtensionPoint))]
    public class InProcessApplicationServiceProvider : IServiceProvider
    {
         private IServiceFactory _serviceFactory;

        public InProcessApplicationServiceProvider()
        {
            _serviceFactory = new ServiceFactory(new ApplicationServiceExtensionPoint());
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            if (_serviceFactory.HasService(serviceType))
            {
                return _serviceFactory.GetService(serviceType);
            }
            else
            {
                return null;    // as per MSDN
            }
        }

        #endregion
   }
}
