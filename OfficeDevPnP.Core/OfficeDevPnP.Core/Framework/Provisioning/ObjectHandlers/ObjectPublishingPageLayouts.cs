using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Utilities;

namespace OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers
{
    internal class ObjectPublishingPageLayouts : ObjectHandlerBase
    {
        public override string Name
        {
            get { return "Publishing Page Layouts"; }
        }


        public override void ProvisionObjects(Web web, ProvisioningTemplate template)
        {
            Log.Info(Constants.LOGGING_SOURCE_FRAMEWORK_PROVISIONING, CoreResources.Provisioning_ObjectHandlers_Pages);

            var context = web.Context as ClientContext;

            if (!web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                context.Load(web, w => w.ServerRelativeUrl);
                context.ExecuteQueryRetry();
            }

            foreach (var pageLayout in template.PublishingPageLayouts)
            {
                var url = pageLayout.Url.ToParsedString();


                if (!url.ToLower().StartsWith(web.ServerRelativeUrl.ToLower()))
                {
                    url = UrlUtility.Combine(web.ServerRelativeUrl, url);
                }

                string projectBaseFolder = Path.GetFullPath(string.Format(@"{0}\..\..\", AppDomain.CurrentDomain.BaseDirectory));               

                string fullFilePath = Path.Combine(projectBaseFolder, pageLayout.SourceFilePath);

                context.Web.DeployPageLayout(fullFilePath, pageLayout.Title, pageLayout.Description, pageLayout.PublishingAssociatedContentType ); //url  TODO: dorobit ak url nepatri do root foldra


                //var exists = true;
                //Microsoft.SharePoint.Client.File file = null;
                //try
                //{
                //    file = web.GetFileByServerRelativeUrl(url);
                //    web.Context.Load(file);
                //    web.Context.ExecuteQuery();
                //}
                //catch (ServerException ex)
                //{
                //    if (ex.ServerErrorTypeName == "System.IO.FileNotFoundException")
                //    {
                //        exists = false;
                //    }
                //}
                //if (exists)
                //{
                //    if (pageLayout.Overwrite)
                //    {
                //        file.DeleteObject();
                //        web.Context.ExecuteQueryRetry();
                //        web.AddWikiPageByUrl(url);
                //        //web.AddLayoutToWikiPage(page.Layout, url);
                //    }
                //}
                //else
                //{
                //    web.AddWikiPageByUrl(url);

                //}

                var exists = false;
                var retryCount = 0;
                var retryAttempt = 10;
                Microsoft.SharePoint.Client.File file = null;
                do
                {
                    try
                    {
                        file = web.GetFileByServerRelativeUrl(url);
                        web.Context.Load(file);
                        web.Context.ExecuteQuery();
                        exists = true;
                    }
                    catch (ServerException ex)
                    {
                        if (ex.ServerErrorTypeName == "System.IO.FileNotFoundException")
                        {
                            exists = false;
                            Thread.Sleep(5000);
                            retryCount++;
                        }
                    }

                } while (exists == false && retryCount < retryAttempt);


                if (pageLayout.WebParts != null & pageLayout.WebParts.Any())
                {
                    var existingWebParts = web.GetWebParts(url);

                    foreach (var webpart in pageLayout.WebParts)
                    {
                        if (existingWebParts.FirstOrDefault(w => w.WebPart.Title == webpart.Title) == null)
                        {
                            WebPartEntity wpEntity = new WebPartEntity();
                            wpEntity.WebPartTitle = webpart.Title;
                            wpEntity.WebPartIndex = (int)webpart.Order;
                            wpEntity.WebPartZone = webpart.Zone;

                            if (!string.IsNullOrWhiteSpace(webpart.ListUrl))
                            {
                                var list = web.GetListByUrl(webpart.ListUrl);

                                var contents = String.Format(webpart.Contents, list.Id, list.Title);
                                wpEntity.WebPartXml = contents.ToParsedString().Trim(new[] { '\n', ' ' });
                            }
                            else
                            {                                
                                wpEntity.WebPartXml = webpart.Contents.ToParsedString().Trim(new[] { '\n', ' ' });
                            }
                    
                                
                            web.AddWebPartToWebPartPage(url, wpEntity);
                            
                        }
                    }
                }
            }
        }


        public override ProvisioningTemplate CreateEntities(Web web, ProvisioningTemplate template, ProvisioningTemplateCreationInformation creationInfo)
        {
            // Impossible to return all files in the site currently

            // If a base template is specified then use that one to "cleanup" the generated template model
            if (creationInfo.BaseTemplate != null)
            {
                template = CleanupEntities(template, creationInfo.BaseTemplate);
            }

            return template;
        }

        private ProvisioningTemplate CleanupEntities(ProvisioningTemplate template, ProvisioningTemplate baseTemplate)
        {

            return template;
        }


        public override bool WillProvision(Web web, ProvisioningTemplate template)
        {
            if (!_willProvision.HasValue)
            {
                _willProvision = template.PublishingPageLayouts.Any();
            }
            return _willProvision.Value;
        }

        public override bool WillExtract(Web web, ProvisioningTemplate template, ProvisioningTemplateCreationInformation creationInfo)
        {
            if (!_willExtract.HasValue)
            {
                _willExtract = false;
            }
            return _willExtract.Value;
        }
    }
}
