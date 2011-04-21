#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System.Runtime.Serialization;

namespace ClearCanvas.ImageServer.Common.ServiceModel
{
    [DataContract]
    public class ProductVerificationResponse
    {
        [DataMember]
        public string ComponentName { get; set; }
        [DataMember]
        public bool IsManifestValid { get; set; }

        [DataMember]
        public string Edition { get; set; }
    }

    [DataContract]
    public class ProductVerificationRequest
    { }
}