#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using ClearCanvas.Dicom;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Enterprise;

namespace ClearCanvas.ImageServer.Rules
{
    /// <summary>
    /// A context used when applying rules and actions within the ImageServer.
    /// </summary>
    /// <remarks>
    /// This class is used to pass information to rules and to the action procesor
    /// when applying rules within the ImageServer.  It should contain enough
    /// information to apply a given Action for a rule.
    /// </remarks>
    /// <seealso cref="ServerRulesEngine"/>
    public class ServerActionContext
    {
        #region Private Members
        private readonly DicomMessageBase _msg;
        private ServerCommandProcessor _commandProcessor;
        private readonly ServerEntityKey _serverPartitionKey;
        private readonly ServerEntityKey _studyLocationKey;
        private readonly ServerEntityKey _filesystemKey;
        #endregion

        #region Constructors
        public ServerActionContext(DicomMessageBase msg, ServerEntityKey filesystemKey, ServerEntityKey serverPartitionKey, ServerEntityKey studyLocationKey)
        {
            _msg = msg;
            _serverPartitionKey = serverPartitionKey;
            _studyLocationKey = studyLocationKey;
            _filesystemKey = filesystemKey;
        }
        #endregion

        #region Public Properties
        public DicomMessageBase Message
        {
            get { return _msg; }
        }
        public ServerCommandProcessor CommandProcessor
        {
            get { return _commandProcessor; }
            set { _commandProcessor = value; }
        }
        public ServerEntityKey ServerPartitionKey
        {
            get { return _serverPartitionKey; }
        }
        public ServerEntityKey FilesystemKey
        {
            get { return _filesystemKey; }
        }
        public ServerEntityKey StudyLocationKey
        {
            get { return _studyLocationKey; }
        }
        #endregion
    }
}
