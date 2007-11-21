#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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

#if	UNIT_TESTS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;
using NUnit.Framework;
using ClearCanvas.Dicom.OffisNetwork;

namespace ClearCanvas.Dicom.OffisNetwork.Tests
{
   [TestFixture]
    public class DcmNetTest
    {   
        [TestFixtureSetUp]
        public void Init()
        {
            // check to see if OFFIS storescp.exe exists
            //string programToRun = "C:\\Windows\\storescp.exe";
            //string arguments = " -v 104";

            //if (!System.IO.File.Exists(programToRun))
            //    throw new Exception("Could not find the DICOM server program to run tests against");
            
            ///* Note: I haven't figured out how to determine whether the dcmnet.dll 
            // * dependency is somewhere that NUnit can access and properly load into
            // * memory
            // * 
            //// check to see if the dependent C++ library file exists
            //string dependentLibraryFile = "\\dcmnet.dll";
            //if (!System.IO.File.Exists(dependentLibraryFile))
            //    throw new Exception("Could not find the dependent C++ library");
            // */

            //try
            //{
            //    ProcessStartInfo startInfo = new ProcessStartInfo(programToRun);
            //    startInfo.Arguments = arguments;

            //    _dicomServer = Process.Start(startInfo);
            //}
            //catch (System.Exception e)
            //{
            //    throw new Exception("Could not start the DICOM server", e);
            //}
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            //if (null != _dicomServer)
            //{
            //    _dicomServer.Kill();
            //}
        }

        [Test]
        public void Verify()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("10.2.34.203"),
                new AETitle("AE_TITLE"), new ListeningPort(4001));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            bool successVerify = dicomClient.Verify(serverAE);

            Assert.IsTrue(successVerify);
        }

        [Test]
        [ExpectedException(typeof(NetworkDicomException))]
        public void VerifyCausesRejection()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("clintondesk"),
                new AETitle("WRONGAE"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            bool successVerify = dicomClient.Verify(serverAE);
            Assert.IsFalse(successVerify);
        }

        [Test]
        [ExpectedException(typeof(NetworkDicomException))]
        public void VerifyCausesException()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("192.168.0.101"),
                new AETitle("WRONGAE"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            bool successVerify = dicomClient.Verify(serverAE);
            Assert.IsFalse(successVerify);
        }

        [Test]
        public void QueryByPatientId()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
                new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            if (!dicomClient.Verify(serverAE))
                throw new Exception("Target server is not running");

            ReadOnlyQueryResultCollection results = dicomClient.Query(serverAE, new PatientId("0009703828"));

            Assert.IsTrue(results.Count > 0);

            foreach (QueryResult qr in results)
            {
                foreach (DicomTagPath path in qr.DicomTagPathCollection)
                {
					Console.WriteLine("{0} - {1}", path.ToString(), qr[path]);
                }
                Console.WriteLine("Patient's Name: {0}", qr.PatientsName);
                Console.WriteLine("Patient ID: {0}", qr.PatientId);
            }
        }

        [Test]
        public void QueryByPatientsName()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
                new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            if (!dicomClient.Verify(serverAE))
                throw new Exception("Target server is not running");

            ReadOnlyQueryResultCollection results = dicomClient.Query(serverAE, new PersonName("HEAD EXP2"));

            Assert.IsTrue(results.Count > 0);

			foreach (QueryResult qr in results)
			{
				foreach (DicomTagPath path in qr.DicomTagPathCollection)
				{
					Console.WriteLine("{0} - {1}", path.ToString(), qr[path]);
				}
                Console.WriteLine("Patient's Name: {0}", qr.PatientsName);
                Console.WriteLine("Patient ID: {0}", qr.PatientId);
            }
        }

        [Test]
        public void QueryByPatientIdWithEvent()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(110));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
                new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            if (!dicomClient.Verify(serverAE))
                throw new Exception("Target server is not running");

            dicomClient.QueryResultReceived += QueryResultReceivedEventHandler;
            dicomClient.QueryCompleted += QueryCompletedEventHandler;

            ReadOnlyQueryResultCollection results = dicomClient.Query(serverAE, new PatientId(""), new PersonName(""));

            dicomClient.QueryResultReceived -= QueryResultReceivedEventHandler;
            dicomClient.QueryCompleted -= QueryCompletedEventHandler;

            Assert.IsTrue(results.Count > 0);
        }

 
        [Test]
        public void QueryByAccessionNumber()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
                new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            if (!dicomClient.Verify(serverAE))
                throw new Exception("Target server is not running");

            ReadOnlyQueryResultCollection results = dicomClient.Query(serverAE, new PatientId("111111111A"), new Accession(""));

            Assert.IsTrue(results.Count > 0);

            foreach (QueryResult qr in results)
            {
				foreach (DicomTagPath path in qr.DicomTagPathCollection)
				{
					Console.WriteLine("{0} - {1}", path.ToString(), qr[path]);
				}
				Console.WriteLine("Patient's Name: {0}", qr.PatientsName);
                Console.WriteLine("Patient ID: {0}", qr.PatientId);
            }
        }

        [Test]
        public void QueryByAccessionNumber2()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
                new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            if (!dicomClient.Verify(serverAE))
                throw new Exception("Target server is not running");

            ReadOnlyQueryResultCollection results = dicomClient.Query(serverAE, new PatientId("111111111A"), new PersonName("TEST PATIENT"), new Accession("10101010"));

            Assert.IsTrue(results.Count > 0);

            foreach (QueryResult qr in results)
            {
				foreach (DicomTagPath path in qr.DicomTagPathCollection)
				{
					Console.WriteLine("{0} - {1}", path.ToString(), qr[path]);
				}
				Console.WriteLine("Patient's Name: {0}", qr.PatientsName);
                Console.WriteLine("Patient ID: {0}", qr.PatientId);
            }
        }

        [Test]
        public void QueryByStudyInstanceUid()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(110));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
                new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            if (!dicomClient.Verify(serverAE))
                throw new Exception("Target server is not running");

            //ReadOnlyQueryResultCollection results = dicomClient.Query(serverAE, new Uid("1.3.46.670589.5.2.10.2156913941.892665384.993397"));
            ReadOnlyQueryResultCollection results = dicomClient.Query(serverAE, new Uid("1.2.804.114118.11.20060704.115022.339091099.19"));
            Assert.IsTrue(results.Count > 0);

            foreach (QueryResult qr in results)
            {
				foreach (DicomTagPath path in qr.DicomTagPathCollection)
				{
					Console.WriteLine("{0} - {1}", path.ToString(), qr[path]);
				}
			}
        }

        [Test]
        public void QueryForSeries()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));
            ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
                new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

            DicomClient dicomClient = new DicomClient(myOwnAEParameters);

            if (!dicomClient.Verify(serverAE))
                throw new Exception("Target server is not running");

            //ReadOnlyQueryResultCollection results = dicomClient.Query(serverAE, new Uid("1.3.46.670589.5.2.10.2156913941.892665384.993397"));
            ReadOnlyQueryResultCollection results = dicomClient.QuerySeries(serverAE, new Uid("1.2.804.114118.11.20060704.115022.339091099.19.1"));
            Assert.IsTrue(results.Count > 0);

            foreach (QueryResult qr in results)
            {
				foreach (DicomTagPath path in qr.DicomTagPathCollection)
				{
					Console.WriteLine("{0} - {1}", path.ToString(), qr[path]);
				}
			}
        }

        [Test]
        public void QueryByMultipleKeys()
        {
        }

        [Test]
        public void Retrieve()
        {
			ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
				new AETitle("CCNETTEST"), new ListeningPort(4000));
			ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
				new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

			DicomClient dicomClient = new DicomClient(myOwnAEParameters);

			if (!dicomClient.Verify(serverAE))
				throw new Exception("Target server is not running");

			dicomClient.RetrieveProgressUpdated += new EventHandler<RetrieveProgressUpdatedEventArgs>(RetrieveProgressUpdated);
			dicomClient.Retrieve(serverAE, new Uid("1.2.804.114118.11.20060704.115022.339091099.19"), @"..\studies\");
			dicomClient.RetrieveProgressUpdated += new EventHandler<RetrieveProgressUpdatedEventArgs>(RetrieveProgressUpdated);
        }

        [Test]
        public void RetrieveSeries()
        {
			ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
				new AETitle("CCNETTEST"), new ListeningPort(4000));
			ApplicationEntity serverAE = new ApplicationEntity(new HostName("172.16.10.167"),
				new AETitle("CONQUESTSRV1"), new ListeningPort(5678));

			DicomClient dicomClient = new DicomClient(myOwnAEParameters);

			if (!dicomClient.Verify(serverAE))
				throw new Exception("Target server is not running");

			dicomClient.RetrieveProgressUpdated += new EventHandler<RetrieveProgressUpdatedEventArgs>(RetrieveProgressUpdated);
			dicomClient.RetrieveSeries(serverAE, new Uid("1.2.804.114118.11.20060704.115022.339091099.19.1"), @"..\studies\");
			dicomClient.RetrieveProgressUpdated += new EventHandler<RetrieveProgressUpdatedEventArgs>(RetrieveProgressUpdated);
        }

        [Test]
        public void ServerTest()
        {
            ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
                new AETitle("CCNETTEST"), new ListeningPort(4000));

            DicomServer dicomServer = new DicomServer(myOwnAEParameters, @"..\studies\");

            dicomServer.Start();

            Thread.Sleep(TimeSpan.FromMinutes(5));

            dicomServer.Stop();
        }

        [Test]
        public void SendTest()
        {
			ApplicationEntity myOwnAEParameters = new ApplicationEntity(new HostName("localhost"),
				new AETitle("CCNETTEST"), new ListeningPort(4000));

			ApplicationEntity serverAE = new ApplicationEntity(new HostName("localhost"),
				new AETitle("STORESCP"), new ListeningPort(12000));

			DicomClient dicomClient = new DicomClient(myOwnAEParameters);

			string[] files = Directory.GetFiles(@"c:\temp\UnitTestFiles\ClearCanvas.Dicom.Tests.DcmNetTest.SendTest\");
			dicomClient.Store(serverAE, files);
        }

        #region Non-test utility methods

		public void RetrieveProgressUpdated(object sender, RetrieveProgressUpdatedEventArgs e)
		{
			Console.WriteLine("Beg of RetrieveProgressUpdated-------------");
			Console.WriteLine("       Completed: {0}, Failed: {1}, Remaining: {2}", e.CompletedSuboperations, e.FailedSuboperations, e.RemainingSuboperations);
			Console.WriteLine("End of RetrieveProgressUpdated-------------");
		}
		
	   public static void QueryResultReceivedEventHandler(object source, QueryResultReceivedEventArgs args)
        {
            Console.WriteLine("Beg of QueryResultReceivedEventHandler-------------");
			foreach (DicomTagPath path in args.Result.DicomTagPathCollection)
			{
				Console.WriteLine("{0} - {1}", path.ToString(), args.Result[path]);
			}
			Console.WriteLine("End of QueryResultReceivedEventHandler-------------");
        }

        public static void QueryCompletedEventHandler(object source, QueryCompletedEventArgs args)
        {
            Console.WriteLine("Beg of QueryCompletedEventHandler-------------");
            foreach (QueryResult qr in args.Results)
            {
			foreach (DicomTagPath path in qr.DicomTagPathCollection)
			{
				Console.WriteLine("{0} - {1}", path.ToString(), qr[path]);
			}
            }
            Console.WriteLine("End of QueryCompletedEventHandler-------------");

        }

        #endregion
    }
}

#endif