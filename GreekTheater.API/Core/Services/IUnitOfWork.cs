using GreekTheater.API.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Services
{
    public interface IUnitOfWork
    {
        IDirectorRepository Directors { get; }

        void Complete();
    }
}
