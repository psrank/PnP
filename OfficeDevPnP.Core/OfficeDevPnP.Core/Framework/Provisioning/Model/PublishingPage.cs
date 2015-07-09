using System;
using System.Collections.Generic;

namespace OfficeDevPnP.Core.Framework.Provisioning.Model
{
    public class PublishingPage : IEquatable<PublishingPage>
    {
        #region Private Members
        private List<WebPart> _webParts = new List<WebPart>();
        #endregion

        #region Properties
        public string Url { get; set; }

        public PublishingPageLayout PageLayout { get; set; }

        public bool Overwrite { get; set; }
        public bool WelcomePage { get; set; }

        public List<WebPart> WebParts
        {
            get { return _webParts; }
            private set { _webParts = value; }
        }

        #endregion

        #region Constructors
        public PublishingPage() { }

        public PublishingPage(string url, bool overwrite, PublishingPageLayout layout, IEnumerable<WebPart> webParts, bool welcomePage = false)
        {
            this.Url = url;
            this.Overwrite = overwrite;
            this.PageLayout = layout;
            this.WelcomePage = welcomePage;

            if (webParts != null)
            {
                this.WebParts.AddRange(webParts);
            }
        }


        #endregion

        #region Comparison code

        public override int GetHashCode()
        {
            return (String.Format("{0}|{1}|{2}",
                this.Url,
                this.Overwrite,
                this.PageLayout).GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is File))
            {
                return (false);
            }
            return (Equals((File)obj));
        }

        public bool Equals(PublishingPage other)
        {
            return (this.Url == other.Url &&
                this.Overwrite == other.Overwrite &&
                this.PageLayout == other.PageLayout);
        }

        #endregion
    }
}
