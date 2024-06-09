using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class NotifiableModelObject : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        protected NotifiableModelObject(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}