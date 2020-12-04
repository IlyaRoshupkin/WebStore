using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastucture.Conventions
{
    public class WebStoreControllerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            //controller.Actions.Add(new ActionModel());
        }
    }
}
