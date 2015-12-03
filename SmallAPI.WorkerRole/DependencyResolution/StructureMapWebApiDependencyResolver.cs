// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapWebApiDependencyResolver.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace SmallApi.WorkerRole.DependencyResolution
{
    /// <summary>
    /// The structure map dependency resolver.
    /// </summary>
    public class StructureMapWebApiDependencyResolver : IDependencyResolver
    {
        #region Constructors and Destructors
        private IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapWebApiDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public StructureMapWebApiDependencyResolver(IContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The begin scope.
        /// </summary>
        /// <returns>
        /// The System.Web.Http.Dependencies.IDependencyScope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            IContainer child = this.container.GetNestedContainer();
            return new StructureMapWebApiDependencyResolver(child);
        }

        #endregion

        public void Dispose()
        {
            container.Dispose();
            container = null;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                    return container.TryGetInstance(serviceType);

                return container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances<object>().Where(x => x.GetType() == serviceType);
        }
    }
}