// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chromely.Core
{
	public abstract class ChromelyServiceProviderFactory
	{
		public abstract IServiceProvider BuildServiceProvider(IServiceCollection services);
	}
}