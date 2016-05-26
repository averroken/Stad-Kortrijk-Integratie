using ASP_WEB.DAL.Context;
using System.Collections.Generic;
using System.Data.Entity;


public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{

    internal IntegratieContext context;
    internal DbSet<TEntity> dbSet;


    public GenericRepository()
    {
        this.context = new IntegratieContext();
        this.dbSet = context.Set<TEntity>();
    }

    public GenericRepository(IntegratieContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }

    public virtual IEnumerable<TEntity> All()
    {
        return dbSet;
    }

    public virtual TEntity GetByID(object id)
    {
        return dbSet.Find(id);
    }

    public virtual TEntity Insert(TEntity entity)
    {
        return dbSet.Add(entity);
    }

    public virtual void Delete(object id)
    {
        TEntity entityToDelete = dbSet.Find(id);
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public virtual void SaveChanges()
    {
        context.SaveChanges();
    }
}