﻿#region Copyright (C) 2007-2015 Team MediaPortal

/*
    Copyright (C) 2007-2015 Team MediaPortal
    http://www.team-mediaportal.com

    This file is part of MediaPortal 2

    MediaPortal 2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal 2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal 2. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.Collections.Generic;
using HttpServer.HttpModules;
using MediaPortal.Common;
using MediaPortal.Common.Logging;
using MediaPortal.Common.MediaManagement;
using MediaPortal.Common.MediaManagement.DefaultItemAspects;
using MediaPortal.Common.ResourceAccess;
using MediaPortal.Common.Services.Logging;
using MediaPortal.Extensions.MediaServer.DIDL;
using MediaPortal.Extensions.MediaServer.Objects;
using MediaPortal.Extensions.MediaServer.Objects.MediaLibrary;
using NUnit.Framework;

namespace Test.MediaServer
{
    internal class ResourceServer : IResourceServer
    {

      public string AddressIPv4
      {
        get { return "localhost"; }
      }

      public int PortIPv4
      {
        get { return 0; }
      }

      public string AddressIPv6
      {
        get { return null; }
      }

      public int PortIPv6
      {
          get { return -1; }
      }

      public void Startup()
      {
          throw new NotImplementedException();
      }

      public void Shutdown()
      {
          throw new NotImplementedException();
      }

      public void RestartHttpServers()
      {
          throw new NotImplementedException();
      }

      public void RemoveHttpModule(HttpModule module)
      {
          throw new NotImplementedException();
      }

      public void AddHttpModule(HttpModule module)
      {
          throw new NotImplementedException();
      }
    }

    class TestMediaLibraryAlbumItem : MediaLibraryAlbumItem
    {
        public TestMediaLibraryAlbumItem(string id, string title) : base(id, title) {}

        public override int ChildCount
        {
            get { return 0; }
        }
    }

    [TestFixture]
    class TestMessageBuilder
    {
        [TestFixtureSetUp]
        public void OneTimeSetUp()
        {
            ServiceRegistration.Set<ILogger>(new ConsoleLogger(LogLevel.All, true));
            ServiceRegistration.Set<IResourceServer>(new ResourceServer());
        }

        [Test]
        public void TestAudioItem()
        {
            IList<IDirectoryObject> objects = new List<IDirectoryObject>();

            Guid id = new Guid("11111111-aaaa-aaaa-aaaa-111111111111");
            IDictionary<Guid, IList<MediaItemAspect>> aspects = new Dictionary<Guid, IList<MediaItemAspect>>();

            SingleMediaItemAspect aspect1 = new SingleMediaItemAspect(MediaAspect.Metadata);
            aspect1.SetAttribute(MediaAspect.ATTR_TITLE, "The Track");
            MediaItemAspect.SetAspect(aspects, aspect1);

            SingleMediaItemAspect aspect2 = new SingleMediaItemAspect(ProviderResourceAspect.Metadata);
            aspect2.SetAttribute(ProviderResourceAspect.ATTR_RESOURCE_ACCESSOR_PATH, "c:\\file.mp3");
            MediaItemAspect.SetAspect(aspects, aspect2);

            SingleMediaItemAspect aspect3 = new SingleMediaItemAspect(AudioAspect.Metadata);
            MediaItemAspect.SetAspect(aspects, aspect3);

            MediaItem item = new MediaItem(id, aspects);

            objects.Add(MediaLibraryHelper.InstansiateMediaLibraryObject(item, null, null));

            GenericDidlMessageBuilder builder = new GenericDidlMessageBuilder();
            builder.BuildAll("*", objects);

            string xml = builder.ToString();
            Console.WriteLine("XML: {0}", xml);
        }

        [Test]
        public void TestAlbumItem()
        {
            IList<IDirectoryObject> objects = new List<IDirectoryObject>();

            MediaLibraryAlbumItem item = new TestMediaLibraryAlbumItem("abc123", "The Album");
            item.Initialise();
            objects.Add(item);

            GenericDidlMessageBuilder builder = new GenericDidlMessageBuilder();
            builder.BuildAll("*", objects);

            string xml = builder.ToString();
            Console.WriteLine("XML: {0}", xml);
        }
    }
}