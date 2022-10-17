using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakeProject
{
    public class FakeEvent
    {
        /// <summary>
        /// Constructor of fake event
        /// </summary>
        /// <param name="text">Text passed along on raised event</param>
        public FakeEvent(string text) { Text = text; }
        /// <summary>
        /// Text of event
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Published of fake events
        /// </summary>
        public class FakeEventPublisher
        {
            /// <summary>
            /// Some fake event handler
            /// </summary>
            /// <param name="sender">Send of event</param>
            /// <param name="e">Fake event args</param>
            public delegate void FakeEventHandler(object sender, FakeEvent e);

            /// <summary>
            /// Some fake event
            /// </summary>
            public event FakeEventHandler FakeEvent;

            /// <summary>
            /// Wrap the event in a protected virtual method 
            /// to enable derived classes to raise the event.
            /// </summary>
            protected virtual void RaiseFakeEvent()
            {
                FakeEvent?.Invoke(this, new FakeEvent("Hello"));
            }
        }


        public static void AddEvent()
        {
            var fakePub = new FakeEventPublisher();
            fakePub.FakeEvent += FakePub_FakeEvent;
        }

        private static void FakePub_FakeEvent(object sender, FakeEvent e)
        {
            throw new NotImplementedException();
        }
    }
}
