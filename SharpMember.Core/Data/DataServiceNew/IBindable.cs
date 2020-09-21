using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.DataServiceNew
{
    interface IBindable<T> where T : IBindable<T>
    {
        T Bind(int id); // bind to the target object by id
    }
}
