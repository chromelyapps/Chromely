// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

global using System.Net;
global using System.Reflection;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Hosting.Server;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Logging;

global using Chromium.AspNetCore.Bridge;
global using Chromely.Browser;
global using Chromely.Core;
global using Chromely.Core.Configuration;
global using Chromely.Core.Defaults;
global using Chromely.Core.Host;
global using Chromely.Core.Infrastructure;
global using Chromely.Core.Logging;
global using Chromely.Core.Network;
global using Chromely.Core.Owin;
global using Xilium.CefGlue;
