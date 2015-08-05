using System;
using System.Collections.Generic;

namespace OfficeDevPnP.Core.Framework.Provisioning.Model
{
    public class PublishingPage : IEquatable<PublishingPage>
    {
        #region Private Members
        private List<WebPart> _webParts = new List<WebPart>();
        private Dictionary<string, string> _properties = new Dictionary<string, string>();
        #endregion

        #region Properties
        public string PageName { get; set; }
        public string PageLayoutName { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }
        public bool Publish { get; set; }
        public bool Overwrite { get; set; }
        public bool WelcomePage { get; set; }

        public List<WebPart> WebParts
        {
            get { return _webParts; }
            private set { _webParts = value; }
        }

        public Dictionary<string, string> Properties
        {
            get { return _properties; }
            private set { _properties = value; }
        }

        #endregion

        #region Constructors
        public PublishingPage() { }

        public PublishingPage(string pageName, string pageLayoutName, string title, string content, bool overwrite, IEnumerable<WebPart> webParts, Dictionary<string, string> properties, bool publish = true, bool welcomePage = false)
        {
            this.PageName = pageName;
            this.Overwrite = overwrite;
            this.PageLayoutName = pageLayoutName;
            this.Title = title;
            this.Content = content;            
            this.Publish = publish;
            this.WelcomePage = welcomePage;

            if (webParts != null)
            {
                this.WebParts.AddRange(webParts);
            }

            if (_properties != null)
            {
                foreach (var property in properties)
                {
                    this.Properties.Add(property.Key, property.Value);
                }
            }
        }


        #endregion

        #region Comparison code

        public override int GetHashCode()
        {
            return (String.Format("{0}|{1}|{2}",
                this.PageName,
                this.Overwrite,
                this.Title,
                this.PageLayoutName).GetHashCode());
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
            return (this.PageName == other.PageName &&
                this.Title ==  other.Title &&
                this.Overwrite == other.Overwrite &&
                this.PageLayoutName == other.PageLayoutName);
        }

        #endregion
    }
}
