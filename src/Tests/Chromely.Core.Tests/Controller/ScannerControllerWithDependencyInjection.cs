// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScannerControllerWithDependencyInjection.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using Chromely.Core.RestfulService;

// ReSharper disable MemberCanBePrivate.Global
namespace Chromely.Core.Tests.Controller
{
    /// <summary>
    /// The scanner controller.
    /// </summary>
    [ControllerProperty(Name = "ScannerController", Route = "scannercontroller")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScannerControllerWithDependencyInjection : ChromelyController
    {
        public const int Get1Response = 1000;
        public const string Get2Response = "Test Get 2";
        private readonly IChromelyLogger _logger;
        private readonly ITestDependency _dependency;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScannerControllerWithDependencyInjection"/> class.
        /// </summary>
        public ScannerControllerWithDependencyInjection(IChromelyLogger logger, ITestDependency dependency)
        {
            _logger = logger;
            _dependency = dependency;
            RegisterGetRequest("/scannercontroller/get1", Get1);
            RegisterGetRequest("/scannercontroller/get2", Get2);
            RegisterPostRequest("/scannercontroller/save", Save);
        }

        /// <summary>
        /// The get 1.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse Get1(ChromelyRequest request)
        {
            var response = new ChromelyResponse { Data = Get1Response };
            return response;
        }

        /// <summary>
        /// The get 2.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse Get2(ChromelyRequest request)
        {
            _logger?.Info(Get2Response);

            var text = (_dependency != null)
                ? _dependency.ReturnTest()
                : Get2Response;

            var response = new ChromelyResponse { Data = text };
            return response;
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// The <see cref="ChromelyResponse"/>.
        /// </returns>
        private ChromelyResponse Save(ChromelyRequest request)
        {
            var response = new ChromelyResponse { Data = request.PostData };
            return response;
        }
    }
}