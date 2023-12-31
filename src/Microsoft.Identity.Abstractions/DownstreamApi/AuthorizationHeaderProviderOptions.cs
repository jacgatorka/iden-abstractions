﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.Net.Http;

namespace Microsoft.Identity.Abstractions
{
    /// <summary>
    /// Options passed-in to call downstream web APIs. To call Microsoft Graph, see rather
    /// <c>MicrosoftGraphOptions</c> in the <c>Microsoft.Identity.Web.MicrosoftGraph</c> assembly.
    /// </summary>
    public class AuthorizationHeaderProviderOptions
    {
        AcquireTokenOptions _acquireTokenOptions = new();
        string _httpMethod = "Get";
        string _protocolScheme = "Bearer";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AuthorizationHeaderProviderOptions()
        {
        }

        /// <summary>
        /// Copy constructor for <see cref="AuthorizationHeaderProviderOptions"/>
        /// </summary>
        /// <param name="other">Options to copy from.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="other"/> is <c>null</c>.</exception>
        public AuthorizationHeaderProviderOptions(AuthorizationHeaderProviderOptions other)
        {
            _ = other ?? throw new ArgumentNullException(nameof(other));

            BaseUrl = other.BaseUrl;
            RelativePath = other.RelativePath;
            AcquireTokenOptions = other.AcquireTokenOptions.Clone();
            HttpMethod = other.HttpMethod.ToString();
            CustomizeHttpRequestMessage = other.CustomizeHttpRequestMessage;
            ProtocolScheme = other.ProtocolScheme;
            RequestAppToken = other.RequestAppToken;
        }

        /// <summary>
        /// Base URL for the called downstream web API. For instance <c>"https://graph.microsoft.com/beta/"</c>.
        /// </summary>
        public string? BaseUrl { get; set; }

        /// <summary>
        /// Path relative to the <see cref="BaseUrl"/> (for instance "me").
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// HTTP method used to call this downstream web API (by default Get).
        /// </summary>
        [DefaultValue("Get")]
        public string HttpMethod
        {
            get
            {
                return _httpMethod;
            }
            set
            {
                _httpMethod = string.IsNullOrEmpty(value) ? "Get" : value;
            }
        }

        /// <summary>
        /// Provides an opportunity for the caller app to customize the HttpRequestMessage. For example,
        /// to customize the headers. This is called after the message was formed, including
        /// the Authorization header, and just before the message is sent.
        /// </summary>
        public Action<HttpRequestMessage>? CustomizeHttpRequestMessage { get; set; }

        /// <summary>
        /// Options related to token acquisition.
        /// </summary>
        public AcquireTokenOptions AcquireTokenOptions
        {
            get
            {
                return _acquireTokenOptions;
            }
            set
            {
                _acquireTokenOptions = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Name of the protocol scheme used to create the authorization header. By default: "Bearer".
        /// </summary>
        [DefaultValue("Bearer")]
        public string ProtocolScheme
        {
            get
            {
                return _protocolScheme;
            }
            set
            {
                _protocolScheme = string.IsNullOrEmpty(value) ? "Bearer" : value;
            }
        }

        /// <summary>
        /// Describes if the downstream API is called on behalf of the calling service itself
        /// (App token) or on behalf of a user processed by the service (user token).
        /// If <c>true</c>, the token is requested on behalf of the app. Otherwise, it on-behalf of the user.
        /// </summary>
        /// <remarks>This is especially usefull when the call to the downstream API is described
        /// by configuration.</remarks>
        public bool RequestAppToken { get; set; }

        /// <summary>
        /// Clone the options (to be able to override them).
        /// </summary>
        /// <returns>A clone of the options.</returns>
        public AuthorizationHeaderProviderOptions Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        /// Clone the options (to be able to override them).
        /// </summary>
        /// <returns>A clone of the options.</returns>
        protected virtual AuthorizationHeaderProviderOptions CloneInternal()
        {
            return new AuthorizationHeaderProviderOptions(this);
        }

        /// <summary>
        /// Return the downstream web API URL.
        /// </summary>
        /// <returns>URL of the downstream web API.</returns>
#pragma warning disable CA1055 // Uri return values should not be strings
        public string GetApiUrl()
#pragma warning restore CA1055 // Uri return values should not be strings
        {
            return BaseUrl?.TrimEnd('/') + $"/{RelativePath?.TrimStart('/')}";
        }
    }
}
