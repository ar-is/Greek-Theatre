using GreekTheater.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreekTheater.API.Core.Repositories
{
    public interface IDirectorRepository
    {
        IEnumerable<Director> GetDirectors(string name);
        IEnumerable<Director> GetDirectors(IEnumerable<Guid> directorIds);
        Director GetDirector(Guid directorId);
        bool DirectorExists(Director director);
        bool DirectorExists(Guid directorId);
        bool DirectorsExist(IEnumerable<Director> directors);
        void AddDirector(Director director);
        void UpdateDirector(Director director);
        void DeleteDirector(Director director);
    }
}
