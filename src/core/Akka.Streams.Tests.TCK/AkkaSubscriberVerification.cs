﻿//-----------------------------------------------------------------------
// <copyright file="AkkaSubscriberBlackboxVerification.cs" company="Akka.NET Project">
//     Copyright (C) 2015-2016 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using Akka.Actor;
using Akka.Streams.TestKit.Tests;
using Akka.TestKit.Internal;
using Akka.TestKit.Internal.StringMatcher;
using Akka.TestKit.TestEvent;
using NUnit.Framework;
using Reactive.Streams.TCK;

namespace Akka.Streams.Tests.TCK
{
    [TestFixture]
    abstract class AkkaSubscriberBlackboxVerification<T> : SubscriberBlackboxVerification<T>, IDisposable
    {
        protected AkkaSubscriberBlackboxVerification() : this(false)
        {

        }

        protected AkkaSubscriberBlackboxVerification(bool writeLineDebug)
            : this(
                new TestEnvironment(Timeouts.DefaultTimeoutMillis,
                    TestEnvironment.EnvironmentDefaultNoSignalsTimeoutMilliseconds(), writeLineDebug))
        {
        }

        protected AkkaSubscriberBlackboxVerification(TestEnvironment environment) : base(environment)
        {
            System = ActorSystem.Create(GetType().Name, AkkaSpec.TestConfig);
            System.EventStream.Publish(new Mute(new ErrorFilter(typeof(Exception), new ContainsString("Test exception"))));
            Materializer = ActorMaterializer.Create(System, ActorMaterializerSettings.Create(System));
        }

        protected ActorSystem System { get; private set; }

        protected ActorMaterializer Materializer { get; private set; }
        
        public void Dispose()
        {
            if (!System.Terminate().Wait(Timeouts.ShutdownTimeout))
                throw new Exception($"Failed to stop {System.Name} within {Timeouts.ShutdownTimeout}");
        }
    }

    abstract class AkkaSubscriberWhiteboxVerification<T> : SubscriberWhiteboxVerification<T>, IDisposable
    {
        protected AkkaSubscriberWhiteboxVerification() : this(false)
        {

        }

        protected AkkaSubscriberWhiteboxVerification(bool writeLineDebug)
            : this(
                new TestEnvironment(Timeouts.DefaultTimeoutMillis,
                    TestEnvironment.EnvironmentDefaultNoSignalsTimeoutMilliseconds(), writeLineDebug))
        {
        }

        protected AkkaSubscriberWhiteboxVerification(TestEnvironment environment) : base(environment)
        {
            System = ActorSystem.Create(GetType().Name, AkkaSpec.TestConfig);
            System.EventStream.Publish(new Mute(new ErrorFilter(typeof(Exception), new ContainsString("Test exception"))));
            Materializer = ActorMaterializer.Create(System, ActorMaterializerSettings.Create(System));
        }

        protected ActorSystem System { get; private set; }

        protected ActorMaterializer Materializer { get; private set; }

        public void Dispose()
        {
            if (!System.Terminate().Wait(Timeouts.ShutdownTimeout))
                throw new Exception($"Failed to stop {System.Name} within {Timeouts.ShutdownTimeout}");
        }
    }
}
