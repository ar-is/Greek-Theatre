using GreekTheater.API.Core.Repositories;
using GreekTheater.API.Core.Services;
using GreekTheater.API.Core.Services.Sorting;
using GreekTheater.API.Persistence.DbContexts;
using GreekTheater.API.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Persistence.Services
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _disposed;
        private readonly GreekTheaterAPIContext _context;

        public IDirectorRepository Directors { get; private set; }
        public IActorRepository Actors { get; private set; }
        public IPerformanceRepository Performances { get; private set; }

        public UnitOfWork(GreekTheaterAPIContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Directors = new DirectorRepository(context);
            Actors = new ActorRepository(context);
            Performances = new PerformanceRepository(context, propertyMappingService);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _context.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
