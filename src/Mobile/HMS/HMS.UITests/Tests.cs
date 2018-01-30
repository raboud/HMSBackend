﻿using NUnit.Framework;
using Xamarin.UITest;

namespace HMS.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void LoginTest()
        {
            app.Tap(x => x.Text("[ LOGIN ]"));
            app.Tap(x => x.Class("WebView").Css("INPUT#Email"));
            app.EnterText(x => x.Class("WebView").Css("INPUT#Email"), "jdoe@randreng.com");
            app.Tap(x => x.Class("WebView").Css("INPUT#Password"));
            app.EnterText(x => x.Class("WebView").Css("INPUT#Password"), "Hms.123!");
            app.Tap(x => x.Class("WebView").Css("BUTTON.btn.btn-default.btn-brand.btn-brand-big"));
            app.Screenshot("HMS Login process");
        }
    }
}
