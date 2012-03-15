#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

#if UNIT_TESTS


using NUnit.Framework;
using ClearCanvas.ImageViewer.Common.WorkItem;

namespace ClearCanvas.ImageViewer.StudyManagement.Storage.Tests
{
	[TestFixture]
	public class SerializationTests
	{
		[WorkItemRequestDataContract("b07a3d7a-2909-4ed1-82ce-8a1ab4f30446")]
		class TestRequestA : WorkItemRequest
		{
			
		}

		[WorkItemRequestDataContract("f6c90b95-d631-4b78-b9a1-a786dc23512a")]
		class TestRequestB : WorkItemRequest
		{
			
		}



		[Test]
		public void Test_WorkItemRequest_serialize_null()
		{
			var a = WorkItemRequestSerializer.Serialize(null);
			Assert.IsNull(a);
		}

		[Test]
		public void Test_WorkItemRequest_deserialize_null()
		{
			var a = WorkItemRequestSerializer.Deserialize(null);
			Assert.IsNull(a);
		}

		[Test]
		public void Test_WorkItemRequest_subclass_roundtrip()
		{
			var requestA = new TestRequestA();
			var requestB = new TestRequestB();

			var a = WorkItemRequestSerializer.Serialize(requestA);
			var b = WorkItemRequestSerializer.Serialize(requestB);

			// ensure that we get instances of the correct sub-classes back, even if we ask for the base-class
			Assert.IsInstanceOfType(typeof(TestRequestA), WorkItemRequestSerializer.Deserialize(a));
			Assert.IsInstanceOfType(typeof(TestRequestB), WorkItemRequestSerializer.Deserialize(b));
		}
	}
}

#endif