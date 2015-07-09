using System;
using System.Collections.Generic;

namespace OfficeDevPnP.Core.Framework.Provisioning.Model
{
    public class PublishingPageLayout : IEquatable<PublishingPageLayout>
    {
        #region Private Members
        private List<WebPart> _webParts = new List<WebPart>();
        #endregion

        #region Properties

        public string SourceFilePath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string PublishingAssociatedContentType { get; set; }

        public string ContentType { get; set; }

        public bool Overwrite { get; set; }

        public List<WebPart> WebParts
        {
            get { return _webParts; }
            private set { _webParts = value; }
        }

        #endregion

        #region Constructors
        public PublishingPageLayout() { }

        public PublishingPageLayout(string url, bool overwrite, string publishingAssociatedContentType, string contentType, IEnumerable<WebPart> webParts)
        {
            this.Url = url;
            this.Overwrite = overwrite;
            this.PublishingAssociatedContentType = publishingAssociatedContentType;
            this.ContentType = contentType;

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
                this.PublishingAssociatedContentType,
                this.ContentType).GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is File))
            {
                return (false);
            }
            return (Equals((File)obj));
        }

        public bool Equals(PublishingPageLayout other)
        {
            return (this.Url == other.Url &&
                this.Overwrite == other.Overwrite &&
                this.PublishingAssociatedContentType == other.PublishingAssociatedContentType &&
                this.ContentType == other.ContentType);
        }

        #endregion
    }
}
