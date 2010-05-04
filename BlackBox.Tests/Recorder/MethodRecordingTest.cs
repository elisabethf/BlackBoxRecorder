﻿using System.Reflection;

using BlackBox.Tests.Fakes;
using Xunit;
using Xunit.Extensions;

namespace BlackBox.Tests.Recorder
{
    public class MethodRecordingTest
    {
        [Fact]
        public void Can_get_the_type_the_recording_was_made_on()
        {
            recording.CalledOnType.ShouldEqual(typeof (SimpleMath));
        }

        [Fact]
        public void Can_add_dependency_recordings()
        {
            var method = db.GetType().GetMethod("GetContacts");
            recording.AddDependency(db, method, db.GetContacts());
            recording.DependencyRecordings.Count.ShouldEqual(1);
        }

        public MethodRecordingTest()
        {
            math = new SimpleMath();
            MethodInfo method = math.GetType().GetMethod("Add");
            recording = new MethodRecording(method, math, new object[] { 5, 5});

            db = new SimpleAddressBookDb();
        }

        private MethodRecording recording;
        private SimpleAddressBookDb db;
        private SimpleMath math;
    }
}