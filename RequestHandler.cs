﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Wpf;
using CefSharp;
using System.Security.Cryptography.X509Certificates;
using SoundcloudViews;
using System.IO;

namespace SoundcloudViews
{
    public class RequestHandler : IRequestHandler
    {
        

        private static Random random = new Random();
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        public const string Alphabet = "0123456789";
        public static string RandomString(int Size)
        {
            char[] chars = new char[Size];
            for (int i = 0; i < Size; i++)
            {
                chars[i] = Alphabet[random.Next(Alphabet.Length)];
            }
            return new string(chars);
        }



        public static readonly string VersionNumberString = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
            Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
        {
            return false;
        }

        bool IRequestHandler.OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return OnOpenUrlFromTab(browserControl, browser, frame, targetUrl, targetDisposition, userGesture);
        }

        protected virtual bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    //To allow certificate
                    //callback.Continue(true);
                    //return true;
                }
            }

            return false;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        CefReturnValue IRequestHandler.OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            //Example of how to set Referer
            // Same should work when setting any header

            
            //Example of setting User-Agent in every request.
            var headers = request.Headers;

            var userAgent = headers["User-Agent"];
            headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:53.0) Gecko/20100101 Firefox/" + RandomString(5);

            request.Headers = headers;


            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    if (request.Method == "POST")
                    {
                        using (var postData = request.PostData)
                        {
                            if (postData != null)
                            {
                                var elements = postData.Elements;

                                var charSet = request.GetCharSet();

                                foreach (var element in elements)
                                {
                                    if (element.Type == PostDataElementType.Bytes)
                                    {
                                        var body = element.GetBody(charSet);
                                    }
                                }
                            }
                        }
                    }

                    //Note to Redirect simply set the request Url
                    //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    request.Url = "https://github.com/";
                    //}

                    //Callback in async fashion
                    //callback.Continue(true);
                    //return CefReturnValue.ContinueAsync;
                }
            }

            return CefReturnValue.Continue;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.

            callback.Dispose();
            return false;
        }

        bool IRequestHandler.OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.

            return OnSelectClientCertificate(browserControl, browser, isProxy, host, port, certificates, callback);
        }

        protected virtual bool OnSelectClientCertificate(IWebBrowser browserControl, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
        {
            callback.Dispose();
            return false;
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
        {
            // TODO: Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
            //browserControl.Load(CefExample.RenderProcessCrashedUrl);
        }

        bool IRequestHandler.OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    //Accept Request to raise Quota
                    //callback.Continue(true);
                    //return true;
                }
            }

            return false;
        }

        void IRequestHandler.OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            //Example of how to redirect - need to check `newUrl` in the second pass
            //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase) && !newUrl.Contains("github"))
            //{
            //    newUrl = "https://github.com";
            //}
        }

        bool IRequestHandler.OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
        {
            return url.StartsWith("mailto");
        }

        void IRequestHandler.OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
        {
            
        }

        bool IRequestHandler.OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            //NOTE: You cannot modify the response, only the request
            // You can now access the headers
            //var headers = response.ResponseHeaders;

            return false;
        }

        IResponseFilter IRequestHandler.GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            
            return null;
        }

        void IRequestHandler.OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            
        }

        
    }

}
