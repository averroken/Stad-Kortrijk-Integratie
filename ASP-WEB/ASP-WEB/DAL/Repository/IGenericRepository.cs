using System.Collections.Generic;
#pragma warning disable 1591
public interface IGenericRepository<TEntity>
      where TEntity : class {
        IEnumerable<TEntity> All();
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        TEntity GetByID(object id);
        TEntity Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void SaveChanges();
    }